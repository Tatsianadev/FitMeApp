using FitMeApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;

namespace FitMeApp.Chat
{
    public class ChatHub: Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
        }
        
        [Authorize]
        public async Task Send(string message, string receiverId, string senderId)
        {
            try
            {
                DateTime messageTime = DateTime.Now;
                ChatMessageViewModel messageViewModel = new ChatMessageViewModel()
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Message = message,
                    Date = messageTime
                };

                var messageModel = _mapper.MapChatMessageViewModelToModel(messageViewModel);
                int messageId = _chatService.AddMessage(messageModel);
                await Clients.Users(senderId, receiverId).SendAsync("Get", messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }
        }
    }
}
