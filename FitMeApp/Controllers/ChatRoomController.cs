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
                var allContactsIdByUser = _chatService.GetAllContactsIdByUser(user.Id);
                UserChatMainPageViewModel viewModel = new UserChatMainPageViewModel();
                foreach (var contactId in allContactsIdByUser)
                {
                    viewModel.ContactsId.Add(contactId);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }

            return View();
        }


        public async Task<IActionResult> ContactToChatSelected(List<string> allContactsIdByUser, string receiverId)
        {
            var sender = await _userManager.GetUserAsync(User);
            UserChatMainPageViewModel viewModel = new UserChatMainPageViewModel();
            viewModel.ContactsId = allContactsIdByUser;
            viewModel.SenderId = sender.Id;
            viewModel.ReceiverId = receiverId;

            return View("UserChat", viewModel);
        }




    }
}
