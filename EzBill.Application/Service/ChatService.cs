using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Chat;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Servers;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class ChatService : IChatService
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IChatNotifier _chatNotifier;
		private readonly IAccountRepository _accountRepository;
		private readonly ITripRepository _tripRepository;
		public ChatService(IMessageRepository messageRepository, IChatNotifier chatNotifier, IAccountRepository accountRepository, ITripRepository tripRepository)
		{
			_messageRepository = messageRepository;
			_chatNotifier = chatNotifier;
			_accountRepository = accountRepository;
			_tripRepository = tripRepository;
		}

		public async Task<List<GetMessageModel>> GetMessagesByTripAsync(Guid tripId, int page)
		{
			var trip = await _tripRepository.GetByIdAsync(tripId);
			if (trip == null) throw new AppException("Không tìm thấy trip", 404);
			var messages = await _messageRepository.GetMessagesByTripIdAsync(tripId, page);
			var senderIds = messages.Select(m => m.SenderId).Distinct().ToList();
			var accounts = await _accountRepository.GetByIdsAsync(senderIds);
			var result = from m in messages
						 join a in accounts on m.SenderId equals a.AccountId into acc
						 from a in acc.DefaultIfEmpty()
						 select new GetMessageModel
						 {
							 MessageId = m.MessageId,
							 TripId = m.TripId,
							 SenderId = m.SenderId,
							 NickName = a?.NickName ?? "Unknown",
							 AvatarUrl = a?.AvatarUrl,
							 Content = m.Content,
							 MessageType = m.MessageType,
							 FileUrl = m.FileUrl,
							 SentAt = m.SendAt
						 };

			return result.ToList();
		}

		public async Task<bool> SendMessageAsync(ChatModel chatModel)
		{
			var message = new Messages
			{
				MessageId = Guid.NewGuid(),
				TripId = chatModel.TripId,
				SenderId = chatModel.SenderId,
				Content = chatModel.Content,
				MessageType = chatModel.Type,
				FileUrl = chatModel.FileUrl,
				SendAt = chatModel.SentAt,
				IsDeleted = false
			};
			var result = await _messageRepository.AddMessageAsync(message);
			if (result)
			{
				await _chatNotifier.NotifyMessageAsync(message.TripId, chatModel);
				return true;
			}
			return false;
		}
	}
}
