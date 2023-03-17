using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Services
{
    public class ChatService: IChatService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        private readonly EntityModelMapper _mapper;

        public ChatService(IRepository repository, ILogger<ChatService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();
        }


        public IEnumerable<ChatMessageModel> GetAllMessagesBetweenTwoUsers(string senderId, string receiverId)
        {
            if (senderId == null || receiverId == null)
            {
                throw new ArgumentNullException(nameof(senderId));
            }

            var messagesEntityBase = _repository.GetAllMessagesBetweenTwoUsers(senderId, receiverId);
            List<ChatMessageModel> messagesModels = new List<ChatMessageModel>();
            foreach (var entity in messagesEntityBase)
            {
                messagesModels.Add(_mapper.MapChatMessageEntityBaseToModel(entity));
            }

            return messagesModels;
        }


        public IEnumerable<string> GetAllContactsIdByUser(string userId)
        {
            var allContactsId = _repository.GetAllContactsIdByUser(userId);
            return allContactsId;
        }


        public int AddMessage(ChatMessageModel message)
        {
            var messageEntityBase = _mapper.MapChatMessageModelToEntityBase(message);
           int messageId = _repository.AddMessage(messageEntityBase);
            return messageId;
        }


        public ChatMessageModel GetMessage(int messageId)
        {
            var messageEntityBase = _repository.GetMessage(messageId);
            ChatMessageModel messageModel = _mapper.MapChatMessageEntityBaseToModel(messageEntityBase);
            return messageModel;
        }

        public void AddContact(string userId, string interlocutorId)
        {
             _repository.AddContact(userId, interlocutorId);
        }

    }
}
