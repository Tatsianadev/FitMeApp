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
        public async Task Send(string message, string receiverName)
        {
            var userName = Context.User.Identity.Name;
            if (Context.UserIdentifier != receiverName)
            {
                await Clients.User(Context.UserIdentifier).SendAsync("Receive", message, userName);
                await Clients.User(receiverName).SendAsync("Receive", message, userName);
            }
            
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
