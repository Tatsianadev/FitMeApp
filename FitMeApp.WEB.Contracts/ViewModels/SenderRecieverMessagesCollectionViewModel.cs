using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class SenderRecieverMessagesCollectionViewModel
    {
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderAvatar { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAvatar { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; }


    }
}
