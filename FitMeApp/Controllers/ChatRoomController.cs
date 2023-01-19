using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ChatRoomController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFitMeService _fitMeService;
        private readonly IChatService _chatService;
        private readonly ILogger _logger;

        public ChatRoomController(UserManager<User> userManager, IFitMeService fitMeService, IChatService chatService, ILogger<ChatRoomController> logger)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
            _chatService = chatService;
            _logger = logger;
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
                var messagesModels = _chatService.GetSenderReceiwerMessagesCollection(user.Id);
               
                return View(messagesModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }

            //return View();
        }

      



        public async Task<IActionResult> ChatOneToOne(string toId, string toName)
        {
           
            return View();
        }
    }
}
