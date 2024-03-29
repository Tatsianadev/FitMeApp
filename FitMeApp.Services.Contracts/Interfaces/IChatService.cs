﻿using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IChatService
    {
        IEnumerable<ChatMessageModel> GetAllMessagesBetweenTwoUsers(string senderId, string receiverId);
        IEnumerable<string> GetAllContactsIdByUser(string userId);
        int AddMessage(ChatMessageModel message);
        ChatMessageModel GetMessage(int messageId);
        void AddContact(string userId, string interlocutorId);
    }
}
