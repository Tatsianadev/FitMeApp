using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Mapper;

namespace FitMeApp.Controllers
{
    public class ChatRoomController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFitMeService _fitMeService;
        private readonly IChatService _chatService;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ChatRoomController(UserManager<User> userManager, IFitMeService fitMeService, IChatService chatService, ILogger<ChatRoomController> logger)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
            _chatService = chatService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
        }

        public async Task<IActionResult> ChatRoom()
        {

            return View();
        }

        public async Task<IActionResult> UserChat()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var allContactsIdByUser = _chatService.GetAllContactsIdByUser(user.Id).ToList();
                UserChatMainPageViewModel viewModel = new UserChatMainPageViewModel() { ContactsId = allContactsIdByUser };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with Chat page. Please try again later."
                };
                return View("CustomError", error);
            }
        }


        public async Task<IActionResult> SelectContactToChat(string receiverId)
        {
            try
            {
                var sender = await _userManager.GetUserAsync(User);
                var allContactsIdBySender = _chatService.GetAllContactsIdByUser(sender.Id).ToList();
                UserChatMainPageViewModel viewModel = new UserChatMainPageViewModel()
                {
                    ContactsId = allContactsIdBySender,
                    SenderId = sender.Id,
                    ReceiverId = receiverId
                };

                return View("UserChat", viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "Failed to display chat with current user. Please try again later."
                };
                return View("CustomError", error);
            }
        }


        public async Task<IActionResult> ShowNewMessageInChat(int messageId)
        {
            var messageModel = _chatService.GetMessage(messageId);
            ChatMessageViewModel messageViewModel = _mapper.MapChatMessageModelToViewModel(messageModel);
            var user = await _userManager.GetUserAsync(User);
            return ViewComponent("MessageInPrivateChat", new { message = messageViewModel, userId = user.Id });
        }

        
        public async Task<IActionResult> GetUsersByLetters(string letters)
        {
            string lettersLow = letters.ToLower();
            var currentUser = await _userManager.GetUserAsync(User);
            List<User> users = _userManager.Users
                .Where(x => x.FirstName.ToLower().StartsWith(lettersLow) || x.LastName.ToLower().StartsWith(lettersLow))
                .Where(x => x.UserName != currentUser.UserName)
                .ToList();

            return ViewComponent("UsersSearchResult", new {users = users});
        }


        public async Task<IActionResult> AddChat(string contactId)
        {
            var user = await _userManager.GetUserAsync(User);
            ChatMessageViewModel firstMessageViewModel = new ChatMessageViewModel()
            {
                SenderId = user.Id,
                ReceiverId = contactId,
                Message = "Hi!",
                Date = DateTime.Now

            };
            var firstMessageModel = _mapper.MapChatMessageViewModelToModel(firstMessageViewModel);
            _chatService.AddMessage(firstMessageModel);
            var allContactsIdByUser = _chatService.GetAllContactsIdByUser(user.Id).ToList();
            return ViewComponent("ChatList", new { contactsId = allContactsIdByUser });
        }

    }
}
