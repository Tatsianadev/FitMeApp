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
        private readonly ILogger _logger;

        public ChatRoomController(UserManager<User> userManager, IFitMeService fitMeService, ILogger<ChatRoomController> logger)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
            _logger = logger;
        }

        public async Task<IActionResult> ChatRoom()
        {
            //ViewBag.ReceiverName = "ross@gmail.com";
            //var user = await _userManager.GetUserAsync(User);
            return View();
        }

        public async Task<IActionResult> ChatOneToOne(string toId, string toName)
        {
            var user = await _userManager.GetUserAsync(User);
            string fromId = user.Id;
            string fromName = user.FirstName;
            //Dictionary<string, string> participants = new Dictionary<string, string>()
            //    { { "toId", toId},
            //        { "toName", toName},
            //        { "fromId", fromId},
            //        { "fromName", fromName}
            //    };
            Dictionary<string, string> participants = new Dictionary<string, string>()
                { { "toId", "testReceiverId"},
                    { "toName", "testReceiverName"},
                    { "fromId", fromId},
                    { "fromName", fromName}
                };
            return View(participants);
        }
    }
}
