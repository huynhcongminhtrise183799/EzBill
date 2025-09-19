using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using FirebaseAdmin.Messaging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
	public class MessageRepository : IMessageRepository
	{
		private readonly IMongoDbContext _context;
		private const int pageSize = 10;

		public MessageRepository(IMongoDbContext context)
		{
			_context = context;
		}
		public async Task<bool> AddMessageAsync(Messages message)
		{
			try
			{
				await _context.Messages.InsertOneAsync(message);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async  Task<List<Messages>> GetMessagesByTripIdAsync(Guid tripId, int page)
		{

			return await _context.Messages.Find(m => m.TripId == tripId)
				.SortByDescending(m => m.SendAt)
				.Skip((page - 1) * pageSize)
				.Limit(pageSize)
				.ToListAsync();
		}
	}
}
