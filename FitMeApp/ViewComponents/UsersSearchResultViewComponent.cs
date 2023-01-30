using FitMeApp.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class UsersSearchResultViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(List<User> users)
        {
            return View("UsersSearchResult", users);
        }

    }
}
