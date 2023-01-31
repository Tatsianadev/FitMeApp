using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class UsersSearchResultViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(List<User> users, List<string> allContactsByUser)
        {
            ViewBag.AllContactsByUser = allContactsByUser;
            return View("UsersSearchResult", users);
        }

    }
}
