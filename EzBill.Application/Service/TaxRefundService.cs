using EzBill.Application.DTO.TaxRefund;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
    public class TaxRefundService : ITaxRefundService
    {
        private readonly ITaxRefundRepository _taxRefundRepository;
        private readonly IEventRepository _eventRepository;

        public TaxRefundService(ITaxRefundRepository taxRefundRepository, IEventRepository eventRepository)
        {
            _taxRefundRepository = taxRefundRepository;
            _eventRepository = eventRepository;
        }

        public async Task<List<TaxRefundResponseDto>> ProcessTaxRefundAsync(TaxRefundRequestDto request)
        {
            if (request.Events == null || !request.Events.Any())
                throw new InvalidOperationException("Danh sách event không được rỗng.");

            var taxRefundResponses = new List<TaxRefundResponseDto>();

            foreach (var evtReq in request.Events)
            {
                if (evtReq.Beneficiaries == null || !evtReq.Beneficiaries.Any())
                    throw new InvalidOperationException($"Event phải có người hưởng.");

                if (evtReq.RefundPercent <= 0 || evtReq.RefundPercent > 100)
                    throw new InvalidOperationException("Tỷ lệ hoàn thuế phải từ 0–100.");

                var evt = await _eventRepository.GetByIdAsync(evtReq.EventId);
                if (evt == null)
                    throw new InvalidOperationException($"Event {evtReq.EventId} không tồn tại.");

                double refundAmount = Math.Round(evt.AmountInTripCurrency * evtReq.RefundPercent / 100, 0);
                
                var taxRefund = new TaxRefund
                {
                    TaxRefundId = Guid.NewGuid(),
                    TripId = request.TripId,
                    RefundedBy = evtReq.RefundedBy,
                    ProductName = evt.EventName,
                    OriginalAmount = evt.AmountInTripCurrency,
                    RefundAmount = refundAmount,
                    RefundPercent = evtReq.RefundPercent,
                    SplitType = evtReq.SplitType,
                    TaxRefund_Usages = evtReq.Beneficiaries.Select(b => new TaxRefund_Usage
                    {
                        AccountId = b.AccountId,
                        Ratio = b.Ratio
                    }).ToList(),
                    TaxRefund_Events = new List<TaxRefund_Event>
                    {
                        new TaxRefund_Event { EventId = evt.EventId, TaxRefundId = Guid.NewGuid() }
                    }
                };

                var splitTypeEnum = Enum.Parse<TaxRefundSplitType>(evtReq.SplitType);
                ApplySplit(taxRefund, splitTypeEnum);

                await _taxRefundRepository.AddAsync(taxRefund);
                taxRefundResponses.Add(MapToDto(taxRefund));
            }

            await _taxRefundRepository.SaveChangesAsync();
            return taxRefundResponses;
        }

        public async Task<List<TaxRefundResponseDto>> GetTaxRefundsByTripAsync(Guid tripId)
        {
            var taxRefunds = await _taxRefundRepository.GetByTripIdAsync(tripId);
            return taxRefunds.Select(MapToDto).ToList();
        }

        private void ApplySplit(TaxRefund taxRefund, TaxRefundSplitType splitType)
        {
            switch (splitType)
            {
                case TaxRefundSplitType.EQUAL:
                    SplitEqual(taxRefund);
                    break;
                case TaxRefundSplitType.RATIO:
                    SplitRatio(taxRefund);
                    break;
                case TaxRefundSplitType.KEEP:
                    SplitKeep(taxRefund);
                    break;
                default:
                    throw new InvalidOperationException("Loại chia tiền không hợp lệ.");
            }
        }

        private void SplitEqual(TaxRefund taxRefund)
        {
            var count = taxRefund.TaxRefund_Usages.Count;
            if (count == 0) throw new InvalidOperationException("Không có người hưởng để chia đều.");

            double amountPerPerson = Math.Floor(taxRefund.RefundAmount / count);
            double remainder = taxRefund.RefundAmount - (amountPerPerson * count);

            int index = 0;
            foreach (var u in taxRefund.TaxRefund_Usages)
            {
                u.Ratio = Math.Round(100.0 / count, 2);
                u.AmountReceived = amountPerPerson + (index == 0 ? remainder : 0);
                index++;
            }
        }

        private void SplitRatio(TaxRefund taxRefund)
        {
            double totalRatio = taxRefund.TaxRefund_Usages.Sum(u => u.Ratio ?? 0);
            if (Math.Abs(totalRatio - 100.0) > 0.01)
                throw new InvalidOperationException("Tổng tỷ lệ phải bằng 100%.");

            foreach (var u in taxRefund.TaxRefund_Usages)
            {
                u.AmountReceived = Math.Round(
                    taxRefund.RefundAmount * (u.Ratio ?? 0) / 100, 0);
            }
        }

        private void SplitKeep(TaxRefund taxRefund)
        {
            foreach (var u in taxRefund.TaxRefund_Usages)
            {
                if (u.AccountId == taxRefund.RefundedBy)
                {
                    u.Ratio = 100;
                    u.AmountReceived = taxRefund.RefundAmount;
                }
                else
                {
                    u.Ratio = 0;
                    u.AmountReceived = 0;
                }
            }
        }

        private TaxRefundResponseDto MapToDto(TaxRefund r)
        {
            return new TaxRefundResponseDto
            {
                Message = "Hoàn thuế thành công!",
                ProductName = r.ProductName,
                OriginalAmount = $"{r.OriginalAmount:N0}₫",
                RefundPercent = $"{r.RefundPercent}%",
                RefundAmount = $"{r.RefundAmount:N0}₫",
                SplitType = r.SplitType,
                Beneficiaries = r.TaxRefund_Usages?.Select(u => new TaxRefundBeneficiaryResponseDto
                {
                    AccountId = u.AccountId,
                    Ratio = $"{u.Ratio}%",
                    AmountReceived = $"{u.AmountReceived:N0}₫"
                }).ToList() ?? new List<TaxRefundBeneficiaryResponseDto>(),
                Events = r.TaxRefund_Events?.Select(e => new TaxRefundEventResponseDto
                {
                    EventId = e.EventId,
                    EventName = e.Event?.EventName,
                    OriginalAmount = e.Event?.AmountInTripCurrency.ToString("N0") + "₫",
                    RefundAmount = r.RefundAmount.ToString("N0") + "₫"
                }).ToList() ?? new List<TaxRefundEventResponseDto>()
            };
        }
    }
}
