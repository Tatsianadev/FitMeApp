using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Mapper;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IChatService _chatService;
        private readonly ModelViewModelMapper _mapper;

        public ApiController(IChatService chatService)
        {
            _chatService = chatService;
            _mapper = new ModelViewModelMapper();
        }

        [HttpPost]
        [Route("addmessagetodb")]
        public int AddMessageToDb(string message, string receiverId, string senderId)
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
           

            return messageId ;
        }

    }
}
