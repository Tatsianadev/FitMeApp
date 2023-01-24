using FitMeApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Chat
{
    public class ChatHub: Hub
    {
        private readonly ILogger _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        
        [Authorize]
        public async Task Send(string message, string receiverId, string senderId)
        {
            try
            {
                //var senderId = Context.UserIdentifier;
                await Clients.Users(senderId, receiverId).SendAsync("Send", message, receiverId, senderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }
           
            //await Clients.All.SendAsync("Send", message, receiverId);

        }

       
    }
}
