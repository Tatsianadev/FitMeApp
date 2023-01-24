using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IChatService _chatService;

        public ApiController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        [Route("addmessagetodb")]
        public bool AddMessageToDb(string message, string receiverId)
        {
            return true;
        }

    }
}
