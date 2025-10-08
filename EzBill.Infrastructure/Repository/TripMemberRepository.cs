using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
	public class TripMemberRepository : ITripMemberRepository
	{
		private readonly EzBillDbContext _context;

		public TripMemberRepository(EzBillDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddTripMember(Guid accountId, Guid tripId)
		{
			// Kiểm tra xem thành viên đã tồn tại trong chuyến đi chưa
			bool exists = await _context.TripMembers
				.AnyAsync(tm => tm.TripId == tripId && tm.AccountId == accountId);

			if (exists)
				return false; // Thành viên đã tồn tại

			var newTripMember = new TripMember
			{
				AccountId = accountId,
				TripId = tripId,
				Amount = 0,
				AmountRemainInTrip = 0, 
				Status = TripMemberStatus.ACTIVE.ToString(),
			};

			await _context.TripMembers.AddAsync(newTripMember);
			await _context.SaveChangesAsync();

			return true;
		}


		public async Task<bool> UpdateAmountRemain(Guid tripId, Guid accountId, double newAmount)
		{
			var tripMember = await _context.TripMembers.FirstOrDefaultAsync(tm => tm.TripId == tripId && tm.AccountId == accountId);
			if (tripMember == null)
			{
				return false; // Trip member not found
			}
			tripMember.AmountRemainInTrip = newAmount;
		 	await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateTripMember(List<TripMember> newMembers, Guid tripId)
		{
			// Lấy danh sách hiện tại trong DB
			var existingMembers = _context.TripMembers
				.Where(tm => tm.TripId == tripId)
				.ToList();

			var newMemberIds = newMembers.Select(m => m.AccountId).ToList();
			var existingMemberIds = existingMembers.Select(m => m.AccountId).ToList();

			var membersToAdd = newMembers
				.Where(m => !existingMemberIds.Contains(m.AccountId))
				.ToList();

			foreach (var member in membersToAdd)
			{
				member.TripId = tripId;
				member.Status = TripMemberStatus.ACTIVE.ToString();
				member.AmountRemainInTrip = member.Amount; 
				_context.TripMembers.Add(member);
			}

			var membersToDeactivate = existingMembers
				.Where(m => !newMemberIds.Contains(m.AccountId))
				.ToList();

			foreach (var member in membersToDeactivate)
			{
				member.Status = TripMemberStatus.INACTIVE.ToString();
				_context.TripMembers.Update(member);
			}

			var membersToUpdate = existingMembers
				.Where(m => newMemberIds.Contains(m.AccountId))
				.ToList();

			foreach (var member in membersToUpdate)
			{
				var newMember = newMembers.First(m => m.AccountId == member.AccountId);


					member.Amount = newMember.Amount;
					member.AmountRemainInTrip = newMember.Amount; 
				

				member.Status = TripMemberStatus.ACTIVE.ToString();

				_context.TripMembers.Update(member);
			}

			await _context.SaveChangesAsync();
			return true;
		}


	}
}
