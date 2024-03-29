﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class SenderRecieverMessagesCollectionViewModel
    {
        public string SenderId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderAvatarPath { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAvatarPath { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; }


    }
}
