using FitMeApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Chat
{
    public class ChatHub: Hub
    {
        
        [Authorize]
        public async Task Send(string message, string userName)
        {
            var userId = Context.UserIdentifier;
            await Clients.All.SendAsync("Send", message, userName);

        }

       
    }
}
