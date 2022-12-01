using FitMeApp.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public ProfileController(UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger("UsersLogger");
        }


        public async Task<IActionResult> UserPersonalData()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(user);
        }
    }
}
