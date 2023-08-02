using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FitMeApp.ViewComponents
{
    public class ChatListViewComponent: ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public ChatListViewComponent(UserManager<User> userManager, ILogger<ChatListViewComponent> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IViewComponentResult Invoke(List<string> contactsId)
        {
            try
            {
                if (contactsId.Count == 0)
                {
                    return View("ChatListIsEmpty");
                }

                List<User> contacts = new List<User>();
                foreach (var contactId in contactsId)
                {
                    var contact = _userManager.Users.Where(x => x.Id == contactId).First();
                    contacts.Add(contact);
                }
                
                return View("ChatList", contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with ChatPage. Please, try again.";
                return View("CustomError", message);
            }
            
                
            
           
                
          
           
        }
    }
}
