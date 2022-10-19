using FitMeApp.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult RolesList()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var roles = _roleManager.Roles.ToList();
                    foreach (var role in roles)
                    {
                        if (role.Name == name)
                        {
                            ViewBag.RoleExistMessage = $"Role {name} already exist";
                            return View(roles);
                        }
                    }

                    var result = await _roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RolesList");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(name);               
            }
            catch (Exception e)
            {
                throw e;
            }          

           
        }
    }
}
