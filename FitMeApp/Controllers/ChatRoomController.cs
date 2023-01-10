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

        public ChatRoomController(UserManager<User> userManager, IFitMeService fitMeService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;           
            _fitMeService = fitMeService;           
            _logger = loggerFactory.CreateLogger("ChatLogger");
        }

        public async Task<IActionResult> ChatRoom()
        {
            ViewBag.ReceiverName = "ross@gmail.com";
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
    }
}
