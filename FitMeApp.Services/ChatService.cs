﻿using System;
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

        public IEnumerable<ChatMessageModel> GetAllMessagesByUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var messagesEntityBase = _repository.GetAllMessagesByUser(userId);
            List<ChatMessageModel> messagesModels = new List<ChatMessageModel>();
            foreach (var entity in messagesEntityBase)
            {
                messagesModels.Add(_mapper.MapChatMessageEntityBaseToModel(entity));
            }

            return messagesModels;
        }

    }
}