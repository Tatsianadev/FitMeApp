using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Mapper;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IChatService _chatService;
        private readonly ModelViewModelMapper _mapper;

        public ApiController(IChatService chatService)
        {
            _chatService = chatService;
            _mapper = new ModelViewModelMapper();
        }

        //delete if it won't be used 

    }
}
