using EzBill.Application.DTO.Event;
using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Event;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.Extensions.Logging;


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
                    double oneShare = totalAmount / userIds.Count;
                    foreach (var userId in userIds)
                        allocation[userId] = oneShare;
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

            double groupFundRemain = (double)trip.Budget;
            foreach (var member in allMembers)
			{
                double needToPay = allocation.ContainsKey(member.AccountId) ? allocation[member.AccountId] : 0;

                double fromGroup = 0;
                double fromPersonal = 0;

                if (needToPay > 0)
                {
                    if (request.IsGroupMoney)
                    {
                        fromGroup = needToPay;
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

		public async Task<bool> DeleteEvent(Guid eventId)
		{
			return await _eventRepository.DeleteEvent(eventId);
		}

		public async Task<EventDto?> GetEventById(Guid eventId)
		{
			var @event = await _eventRepository.GetEventById(eventId);
			if (@event == null) throw new AppException("Event không tồn tại", 404);
			return new EventDto
			{
				EventId = @event.EventId,
				EventName = @event.EventName,
				EventDescription = @event.EventDescription,
				EventDate = @event.EventDate,
				PaidBy = @event.PaidBy,
				ReceiptUrl = @event.ReceiptUrl,
				AvartarPaidBy = @event.Account?.AvatarUrl,
				NickNamePaidBy = @event.Account?.NickName,
				AmountInTripCurrency = @event.AmountInTripCurrency,
				Beneficiaries = @event.Event_Use.Select(u => new BeneficiaryDto
				{
					AccountId = u.AccountId,
					Amount = (u.AmountFromGroup ?? 0) + (u.AmountFromPersonal ?? 0),
					AmountFromGroup = u.AmountFromGroup ?? 0,
					AmountFromPersonal = u.AmountFromPersonal ?? 0,
					Avartar = u.Account?.AvatarUrl,
					NickName = u.Account?.NickName
				}).ToList()
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

		public async Task<bool> UpdateEvent(UpdateEventModel request)
		{
			var existingEvent = await _eventRepository.GetEventById(request.EventId);
			if (existingEvent == null) throw new AppException("Event không tồn tại", 404);
			var tripWithEvent = await _tripRepository.GetByIdAsync(existingEvent.TripId);
			if (tripWithEvent == null) throw new AppException("Không tìm thấy trip.", 404);
			tripWithEvent.Budget += existingEvent.AmountInTripCurrency;
			await _tripRepository.UpdateTripAsync(tripWithEvent);


			double exchangeRate = (double)request.ExchangeRate;
			double totalAmount = request.Currency.ToUpper() != "VND"
				? request.AmountOriginal * exchangeRate
				: request.AmountOriginal;

			var evt = new Event
			{
				EventId = request.EventId,
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
					double oneShare = totalAmount / userIds.Count;
					foreach (var userId in userIds)
						allocation[userId] = oneShare;
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

			double groupFundRemain = (double)trip.Budget;
			foreach (var member in allMembers)
			{
				double needToPay = allocation.ContainsKey(member.AccountId) ? allocation[member.AccountId] : 0;

				double fromGroup = 0;
				double fromPersonal = 0;

				if (needToPay > 0)
				{
					if (request.IsGroupMoney)
					{
						fromGroup = needToPay;
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

			await _eventRepository.UpdateEvent(evt);

			return true;
		}
	}
}
