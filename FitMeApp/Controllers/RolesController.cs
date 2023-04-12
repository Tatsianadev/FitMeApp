using FitMeApp.Common;
using FitMeApp.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public sealed class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public RolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<RolesController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with creat new Role. Try again, please."
                };
                return View("CustomError", error);
            } 
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
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
                return RedirectToAction("RolesList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with delete Role. Try again, please."
                };
                return View("CustomError", error);
            }           
        }

        
        public IActionResult UserWithRolesList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> EditCurrentUserRoles(string userId)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();

                ChangeCurrentUserRolesViewModel model = new ChangeCurrentUserRolesViewModel()
                {
                    UserId = user.Id,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with edit Role. Try again, please."
                };
                return View("CustomError", error);
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditCurrentUserRoles(string userId, IList<string> roles)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var removedRoles = userRoles.Except(roles);
                var addedRoles = roles.Except(userRoles).ToList();

                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                await _userManager.AddToRolesAsync(user, addedRoles);

                return RedirectToAction("UserWithRolesList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with edit Role. Try again, please."
                };
                return View("CustomError", error);
            }
        }
    }
}
