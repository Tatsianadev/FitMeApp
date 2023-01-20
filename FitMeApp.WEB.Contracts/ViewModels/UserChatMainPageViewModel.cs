using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class UserChatMainPageViewModel
    {
        public List<string> ContactsId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
