using EzBill.Application.DTO.TaxRefund;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
    public class TaxRefundService : ITaxRefundService
    {
        private readonly ITaxRefundRepository _taxRefundRepository;

        public TaxRefundService(ITaxRefundRepository taxRefundRepository)
        {
            _taxRefundRepository = taxRefundRepository;
        }

        public async Task ProcessTaxRefundAsync(TaxRefund taxRefund)
        {
            if (taxRefund.TaxRefund_Usages == null || !taxRefund.TaxRefund_Usages.Any())
                throw new InvalidOperationException("Danh sách người hưởng không được rỗng.");

            if (taxRefund.TaxRefundId == Guid.Empty)
                taxRefund.TaxRefundId = Guid.NewGuid();

            if (taxRefund.TripId == Guid.Empty || !(await _taxRefundRepository.TripExistsAsync(taxRefund.TripId)))
            {
                var fallbackEventId = await _taxRefundRepository.GetAnyTripIdAsync();
                if (fallbackEventId == Guid.Empty)
                    throw new InvalidOperationException("Không tìm thấy Trip hợp lệ để gắn vào TaxRefund.");
                taxRefund.TripId = fallbackEventId;
            }

            foreach (var usage in taxRefund.TaxRefund_Usages)
            {
                usage.TaxRefundId = taxRefund.TaxRefundId;
            }

            taxRefund.RefundAmount = Math.Round(
                taxRefund.OriginalAmount * taxRefund.RefundPercent / 100, 0);

            var splitType = Enum.Parse<TaxRefundSplitType>(taxRefund.SplitType);

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

            try
            {
                await _taxRefundRepository.AddAsync(taxRefund);
                await _taxRefundRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var errorMsg = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine("ERROR: " + errorMsg);
                throw new InvalidOperationException("Không thể lưu dữ liệu: " + errorMsg);
            }
        }

        private void SplitEqual(TaxRefund taxRefund)
        {
            var count = taxRefund.TaxRefund_Usages.Count;
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
        public async Task<List<TaxRefundDto>> GetTaxRefundsByTripAsync(Guid tripId)
        {
            var taxRefunds = await _taxRefundRepository.GetByTripIdAsync(tripId);

            return taxRefunds.Select(r => new TaxRefundDto
            {
                TaxRefundId = r.TaxRefundId,
                ProductName = r.ProductName,
                OriginalAmount = r.OriginalAmount,
                RefundPercent = r.RefundPercent,
                RefundAmount = r.RefundAmount,
                RefundedBy = r.RefundedBy,
                SplitType = r.SplitType,
                Beneficiaries = r.TaxRefund_Usages.Select(u => new RefundBeneficiaryDto
                {
                    AccountId = u.AccountId,
                    Ratio = u.Ratio ?? 0,
                    AmountReceived = u.AmountReceived
                }).ToList()
            }).ToList();
        }
    }
}
