using EzBill.Application.DTO.Event;
using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;


namespace EzBill.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITripRepository _tripRepository;
		private readonly ITripMemberRepository _tripMemberRepository;

        public EventService(IEventRepository eventRepository, ITripRepository tripRepository, ITripMemberRepository tripMemberRepository)
        {
            _eventRepository = eventRepository;
            _tripRepository = tripRepository;
			_tripMemberRepository = tripMemberRepository;
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

			var trip = await _tripRepository.GetByIdAsync(request.TripId);
			if (trip == null) throw new Exception("Không tìm thấy trip.");

			var allMembers = await _tripRepository.GetTripMembersAsync(request.TripId);
			if (allMembers == null || !allMembers.Any())
				throw new Exception("Không tìm thấy thành viên nào trong chuyến đi.");

			var userIds = request.EventUses != null && request.EventUses.Any()
				? request.EventUses.Select(x => x.AccountId).Distinct().ToList()
				: allMembers.Select(m => m.AccountId).ToList();

			if (!userIds.Any())
				throw new ArgumentException("Phải có ít nhất 1 người sử dụng hóa đơn.");

			var beneficiaries = new List<BeneficiaryDto>();

			Dictionary<Guid, double> allocation = new();

			switch (request.SplitType)
			{
				case SplitType.ONE_FOR_ALL:
					double oneShare = totalAmount / allMembers.Count;
					foreach (var member in allMembers)
						allocation[member.AccountId] = oneShare;
					break;

				case SplitType.EQUAL:
					double equalShare = totalAmount / userIds.Count;
					foreach (var userId in userIds)
						allocation[userId] = equalShare;
					break;

				case SplitType.RATIO:
					double ratioSum = request.EventUses?.Sum(eu => eu.Ratio) ?? 0;
					if (ratioSum != 100)
						throw new ArgumentException("Tổng tỉ lệ phải bằng 100%");
					foreach (var eu in request.EventUses)
						allocation[eu.AccountId] = totalAmount * (eu.Ratio / 100);
					break;
			}
			if (request.IsGroupMoney)
			{
				
				if (trip.Budget.HasValue && trip.Budget > 0)
				{
					if (trip.Budget < totalAmount)
						throw new AppException("Ngân sách Trip không đủ để chi trả hóa đơn này.", 400);
					trip.Budget -= totalAmount;
					await _tripRepository.UpdateTripAsync(trip);
				}
			}
			

			
			foreach (var member in allMembers)
			{
                double needToPay = allocation.ContainsKey(member.AccountId) ? allocation[member.AccountId] : 0;

                double fromGroup = 0;
                double fromPersonal = 0;

                if (needToPay > 0)
                {
                    if (request.IsGroupMoney) 
                    {
                        if (member.AmountRemainInTrip.HasValue && member.AmountRemainInTrip.Value > 0)
                        {
                            if (member.AmountRemainInTrip.Value >= needToPay)
                            {
                                fromGroup = needToPay;
                                member.AmountRemainInTrip -= needToPay;
                            }
                            else
                            {
                                fromGroup = member.AmountRemainInTrip.Value;
                                fromPersonal = needToPay - fromGroup;
                                member.AmountRemainInTrip = 0;
                            }
                            await _tripMemberRepository.UpdateAmountRemain(trip.TripId, member.AccountId, (double)member.AmountRemainInTrip);
                        }
                        else
                        {
                            fromPersonal = needToPay;
                        }
                    }
                    else 
                    {
                        fromPersonal = needToPay;
                    }
                }

                evt.Event_Use.Add(new Event_Use
                {
                    EventId = evt.EventId,
                    AccountId = member.AccountId,
                    AmountFromGroup = fromGroup,
                    AmountFromPersonal = fromPersonal
                });

                beneficiaries.Add(new BeneficiaryDto
                {
                    AccountId = member.AccountId,
                    Amount = needToPay,
                    Avartar = member.Account?.AvatarUrl,
                    NickName = member.Account?.NickName
                });
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
				AvartarPaidBy = e.Account?.AvatarUrl,
				NickNamePaidBy = e.Account?.NickName,
				AmountInTripCurrency = e.AmountInTripCurrency,
                Beneficiaries = e.Event_Use.Select(u => new BeneficiaryDto
                {
                    AccountId = u.AccountId,
                    Amount = (u.AmountFromGroup ?? 0) + (u.AmountFromPersonal ?? 0),
                    AmountFromGroup = u.AmountFromGroup ?? 0,
                    AmountFromPersonal = u.AmountFromPersonal ?? 0,
                    Avartar = u.Account?.AvatarUrl,
					NickName = u.Account?.NickName
				}).ToList()
            }).ToList();
        }
    }
}
