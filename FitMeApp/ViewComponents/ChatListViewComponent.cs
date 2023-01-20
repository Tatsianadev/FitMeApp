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
        private readonly IChatService _chatService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ChatListViewComponent(IChatService chatService, UserManager<User> userManager, ILogger<ChatListViewComponent> logger)
        {
            _chatService = chatService;
            _userManager = userManager;
            _logger = logger;
            _mapper = new ModelViewModelMapper();

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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with ChatPage. Please, try again."
                };
                return View("CustomError", error);
            }
            
                
            
           
                
          
           
        }
    }
}
