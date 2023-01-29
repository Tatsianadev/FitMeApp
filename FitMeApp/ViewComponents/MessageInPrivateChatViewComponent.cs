using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using FitMeApp.Common;
using Microsoft.Extensions.Logging;

namespace FitMeApp.ViewComponents
{
    public class MessageInPrivateChatViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public MessageInPrivateChatViewComponent(UserManager<User> userManager, ILogger<MessageInPrivateChatViewComponent> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IViewComponentResult Invoke(ChatMessageViewModel message, string userId)
        {
            string senderName = _userManager.Users.Where(x => x.Id == message.SenderId).First().FirstName;
            ViewBag.UserId = userId;
            ViewBag.SenderName = senderName;
            return View("MessageInPrivateChat", message);
        }
    }
}
