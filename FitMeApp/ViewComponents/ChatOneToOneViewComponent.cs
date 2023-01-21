using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class ChatOneToOneViewComponent: ViewComponent
    {
        private readonly IChatService _chatService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ChatOneToOneViewComponent(IChatService chatService, UserManager<User> userManager,
            ILogger<ChatListViewComponent> logger)
        {
            _chatService = chatService;
            _userManager = userManager;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
        }

        public IViewComponentResult Invoke(string senderId, string receiverId)
        {
            var sender = _userManager.Users.Where(x=> x.Id == senderId).First();
            var receiver = _userManager.Users.Where(x => x.Id == receiverId).First(); ;
            var messageModels = _chatService.GetAllMessagesBetweenTwoUsers(sender.Id, receiverId);
            SenderRecieverMessagesCollectionViewModel chatBetweenToPeople = new SenderRecieverMessagesCollectionViewModel();
            chatBetweenToPeople.SenderFirstName = sender.FirstName;
            chatBetweenToPeople.SenderLastName = sender.LastName;
            chatBetweenToPeople.SenderAvatar = sender.Avatar;
            chatBetweenToPeople.ReceiverFirstName = receiver.FirstName;
            chatBetweenToPeople.ReceiverLastName = receiver.LastName;
            chatBetweenToPeople.ReceiverAvatar = receiver.Avatar;

            List<ChatMessageViewModel> messagesViewModels = new List<ChatMessageViewModel>();
            foreach (var messageModel in messageModels)
            {
                messagesViewModels.Add(_mapper.MapChatMessageModelToViewModel(messageModel));
            }

            chatBetweenToPeople.Messages = messagesViewModels;
            return View();
        }
    }
}
