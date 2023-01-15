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
            //var userName = Context.User.Identity.Name;
            //if (Context.UserIdentifier != receiverName)
            //{
            //    //await Clients.User(Context.UserIdentifier).SendAsync("Receive", message, userName);
            //    //await Clients.User(receiverName).SendAsync("Receive", message, userName);
            //}

            await Clients.All.SendAsync("Send", message, userName);

        }

        //public async override Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
        //    await base.OnConnectedAsync();
        //}
    }
}
