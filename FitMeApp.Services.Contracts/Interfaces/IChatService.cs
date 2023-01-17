using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IChatService
    {
        IEnumerable<ChatMessageModel> GetAllMessagesByUser(string userId);
    }
}
