﻿using FitMeApp.Common;
using FitMeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Services.Contracts.Interfaces;
using System.Linq;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Configuration;

namespace FitMeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IGymService gymService,
            ITrainerService trainerService, IEmailService emailService, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gymService = gymService;
            _trainerService = trainerService;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = new User()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        AvatarPath = DefaultSettingsStorage.AvatarPath
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, RolesEnum.user.ToString());
                        await _signInManager.SignInAsync(user, false);

                        bool applyedForTrainerRole = model.Role == RolesEnum.trainer;
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                            "RegisterAsUserCompleted",
                            "Account",
                            new { userId = user.Id, code = code, applyedForTrainerRole = applyedForTrainerRole },
                            protocol: HttpContext.Request.Scheme);

                        string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study cases - constant
                        string fromEmail = DefaultSettingsStorage.SenderEmail;
                        string plainTextContent = "To finish Registration please follow the link <a href=\"" + callbackUrl + "\">Confirm email</a>";
                        string htmlContent = "<strong>To finish Registration please follow the link  <a href=\"" + callbackUrl + "\">Confirm email</a></strong>";
                        string subject = "Confirm email";

                        await _emailService.SendEmailAsync(toEmail, model.FirstName, fromEmail, subject, plainTextContent, htmlContent);

                        return Content("To finish your registration check your Email, please.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with registration. Try again, please."
                };
                return View("CustomError", error);
            }
        }


        public IActionResult RegisterTrainerPart(string userId)
        {
            TrainerApplicationViewModel applicationForm = new TrainerApplicationViewModel()
            {
                UserId = userId
            };
            return View(applicationForm);
        }


        [HttpPost]
        public IActionResult RegisterTrainerPart(TrainerApplicationViewModel model)
        {
            try
            {
                if (model.TrainerSubscription == false && model.Contract == false)
                {
                    ModelState.AddModelError("", "No option selected.");
                    return View(model);
                }

                if (model.Contract == true && model.ContractNumber is null)
                {
                    ModelState.AddModelError("", "Write the contract number over");
                    return View(model);
                }

                if (model.TrainerSubscription == true)
                {
                    var actualTrainerSubscription = _gymService
                        .GetUserSubscriptions(model.UserId)
                        .Where(x => x.WorkAsTrainer == true)
                        .Where(x => x.EndDate > DateTime.Today);

                    if (actualTrainerSubscription.Count() == 0)
                    {
                        ModelState.AddModelError("", "You  don't have any subscriptions for work as Trainer. Please, get one before apply for a Trainer position.");
                        return View(model);
                    }
                }

                TrainerApplicationModel trainerApplication = new TrainerApplicationModel()
                {
                    UserId = model.UserId,
                    TrainerSubscription = model.TrainerSubscription,
                    ContractNumber = model.ContractNumber,
                    ApplicationDate = DateTime.Now
                };

                int appId = _trainerService.AddTrainerApplication(trainerApplication);

                if (appId != 0)
                {
                    ViewBag.ApplyedForTrainerRole = true;
                    return View("RegisterAsUserCompleted", model.UserId);
                }
                else
                {
                    CustomErrorViewModel error = new CustomErrorViewModel()
                    {
                        Message = "There was a problem with registration trainers data." +
                                  "Please try fill form again on Profile page."
                    };
                    return View("CustomError", error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with registration trainers data." +
                    "Please try fill form again on Profile page."
                };
                return View("CustomError", error);
            }
        }


        public async Task<IActionResult> RegisterAsUserCompleted(string userId, string code, bool applyedForTrainerRole)
        {
            try
            {
                if (userId == null || code == null)
                {
                    throw new ArgumentNullException(nameof(userId), "userId or code is null");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "user does not found");
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    if (applyedForTrainerRole)
                    {
                        return RedirectToAction("RegisterTrainerPart", new {userId = userId});
                    }
                    ViewBag.ApplyedForTrainerRole = false;
                    return View("RegisterAsUserCompleted", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            CustomErrorViewModel error = new CustomErrorViewModel()
            {
                Message = "Failed to verify email address." //todo add button Send link once again to try one more time
            };
            return View("CustomError", error);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid password and(or) login");
                    }
                }
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with login. Try again, please."
                };
                return View("CustomError", error);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
