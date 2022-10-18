using FitMeApp.Contracts;
using FitMeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult UsersList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = new User()
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Year = model.Year,
                        Gender = model.Gender
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UsersList");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                EditUserViewModel model = new EditUserViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Year = user.Year,
                    Gender = user.Gender
                };
                return View(model);

            }
            catch (Exception e)
            {

                throw e;
            }            
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        user.UserName = model.Name;
                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Year = model.Year;
                        user.Gender = model.Gender;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("UsersList");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }                    
                }
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                }
                return RedirectToAction("UsersList");
            }
            catch (Exception e)
            {
                throw e;
            }           
        }
    }
}
