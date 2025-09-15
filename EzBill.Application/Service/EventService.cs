using EzBill.Application.DTO.Event;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;


namespace EzBill.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITripRepository _tripRepository;

        public EventService(IEventRepository eventRepository, ITripRepository tripRepository)
        {
            _eventRepository = eventRepository;
            _tripRepository = tripRepository;
        }

        public async Task<CreateEventResponse> CreateEventAsync(CreateEventRequest request)
        {
            double exchangeRate = (double)request.ExchangeRate;
            double totalAmount = request.Currency.ToUpper() != "VND"
                ? request.AmountOriginal * exchangeRate
                : request.AmountOriginal;

            var evt = new Event
            {
                EventId = Guid.NewGuid(),
                EventName = request.EventName,
                EventDescription = request.EventDescription,
                EventDate = request.EventDate,
                ReceiptUrl = request.ReceiptUrl,
                TripId = request.TripId,
                PaidBy = request.PaidBy,
                Currency = request.Currency,
                AmountOriginal = request.AmountOriginal,
                ExchangeRate = request.ExchangeRate,
                AmountInTripCurrency = totalAmount,
                SplitType = request.SplitType.ToString(),
                Event_Use = new List<Event_Use>()
            };

            var allMembers = await _tripRepository.GetTripMembersAsync(request.TripId);
            if (allMembers == null || !allMembers.Any())
                throw new Exception("Không tìm thấy thành viên nào trong chuyến đi.");

            var userIds = request.EventUses != null && request.EventUses.Any()
                ? request.EventUses.Select(x => x.AccountId).Distinct().ToList()
                : allMembers.Select(m => m.AccountId).ToList(); 

            if (!userIds.Any())
                throw new ArgumentException("Phải có ít nhất 1 người sử dụng hóa đơn.");

            var beneficiaries = new List<BeneficiaryDto>();

            switch (request.SplitType)
            {
                case SplitType.ONE_FOR_ALL:
                    foreach (var member in allMembers)
                    {
                        double amount = member.AccountId == request.PaidBy ? totalAmount : 0;

                        evt.Event_Use.Add(new Event_Use
                        {
                            EventId = evt.EventId,
                            AccountId = member.AccountId,
                            Amount = amount
                        });

                        beneficiaries.Add(new BeneficiaryDto
                        {
                            AccountId = member.AccountId,
                            Amount = amount
                        });
                    }
                    break;

                case SplitType.EQUAL:
                    double equalShare = totalAmount / userIds.Count;
                    foreach (var member in allMembers)
                    {
                        double amount = userIds.Contains(member.AccountId) ? equalShare : 0;

                        evt.Event_Use.Add(new Event_Use
                        {
                            EventId = evt.EventId,
                            AccountId = member.AccountId,
                            Amount = amount
                        });

                        beneficiaries.Add(new BeneficiaryDto
                        {
                            AccountId = member.AccountId,
                            Amount = amount
                        });
                    }
                    break;

                case SplitType.RATIO:
                    double ratioSum = request.EventUses?.Sum(eu => eu.Ratio) ?? 0;
                    if (ratioSum != 100)
                        throw new ArgumentException("Tổng tỉ lệ phải bằng 100%");

                    foreach (var member in allMembers)
                    {
                        double userRatio = request.EventUses?
                            .FirstOrDefault(x => x.AccountId == member.AccountId)?.Ratio ?? 0;
                        double amount = totalAmount * (userRatio / 100);

                        evt.Event_Use.Add(new Event_Use
                        {
                            EventId = evt.EventId,
                            AccountId = member.AccountId,
                            Amount = amount
                        });

                        beneficiaries.Add(new BeneficiaryDto
                        {
                            AccountId = member.AccountId,
                            Amount = amount
                        });
                    }
                    break;
            }

            await _eventRepository.AddEventAsync(evt);
            await _eventRepository.SaveChangesAsync();

            return new CreateEventResponse
            {
                EventId = evt.EventId,
                Message = "Hóa đơn đã lưu và chia tiền thành công!",
                TotalAmount = totalAmount,
                Beneficiaries = beneficiaries
            };
        }
        public async Task<List<EventDto>> GetEventsByTripAsync(Guid tripId)
        {
            var events = await _eventRepository.GetByTripIdAsync(tripId);

            return events.Select(e => new EventDto
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                PaidBy = e.PaidBy,
                AmountInTripCurrency = e.AmountInTripCurrency,
                Beneficiaries = e.Event_Use.Select(u => new BeneficiaryDto
                {
                    AccountId = u.AccountId,
                    Amount = u.Amount ?? 0
                }).ToList()
            }).ToList();
        }
    }
}
