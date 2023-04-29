using FitMeApp.Common;
using FitMeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Services.Contracts.Interfaces;
using System.Linq;
using FitMeApp.Resources;
using FitMeApp.Services.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;


namespace FitMeApp.Controllers
{
    public sealed class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IGymService gymService,
            ITrainerService trainerService, IEmailService emailService, ILogger<AccountController> logger, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gymService = gymService;
            _trainerService = trainerService;
            _emailService = emailService;
            _logger = logger;
            _localizer = localizer;
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
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
                        AvatarPath = _localizer["DefaultAvatarPath"]
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, RolesEnum.user.ToString());
                        await _signInManager.SignInAsync(user, false);

                        bool appliedForTrainerRole = model.Role == RolesEnum.trainer;

                        //for debug - to omit sent emailConfirm part
                        //var addedUser = await _userManager.GetUserAsync(User);
                        //return RedirectToAction("RegisterTrainerPart", new { userId = addedUser.Id });
                        //---//---//


                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                            "RegisterAsUserCompleted",
                            "Account",
                            new { userId = user.Id, code = code, appliedForTrainerRole = appliedForTrainerRole },
                            protocol: HttpContext.Request.Scheme);

                        string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
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
                string message = "There was a problem with registration. Try again, please.";
                return View("CustomError", message);
            }
        }

        


        [Authorize]
        public async Task<IActionResult> RegisterAsUserCompleted(string userId, string code, bool appliedForTrainerRole)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
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
                    if (appliedForTrainerRole)
                    {
                        return RedirectToAction("RegisterTrainerPart", new { userId = userId });
                    }
                    ViewBag.AppliedForTrainerRole = false;
                    return View("RegisterAsUserCompleted", userId);
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            string message = "Failed to verify email address. Please, try again in you Profile";
            return View("CustomError", message);
        }



        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> RegisterTrainerPart(string userId)
        {
            TrainerRoleAppFormViewModel applicationForm = new TrainerRoleAppFormViewModel()
            {
                UserId = userId
            };

            ViewBag.Gyms = _gymService.GetAllGymModels();
            return View(applicationForm);
        }


        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult RegisterTrainerPart(TrainerRoleAppFormViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ContractNumber))
                {
                    ModelState.AddModelError("", "Write the contract number over");
                    return View(model);
                }

                TrainerApplicationModel trainerApplication = new TrainerApplicationModel()
                {
                    UserId = model.UserId,
                    ContractNumber = model.ContractNumber,
                    GymId = model.GymId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ApplyingDate = DateTime.Now
                };

                int appId = _trainerService.AddTrainerApplication(trainerApplication);

                if (appId != 0)
                {
                    ViewBag.AppliedForTrainerRole = true;
                    return View("RegisterAsUserCompleted", model.UserId);
                }
                else
                {
                    string message = "There was a problem with registration trainers data." +
                                   "Please try fill form again on Profile page.";
                    return View("CustomError", message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with registration trainers data." +
                                 "Please try fill form again on Profile page.";

                return View("CustomError", message);
            }
        }


        [Authorize(Roles = "user, admin")]
        public IActionResult ApplyForTrainerRoleLater()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.AppliedForTrainerRole = false;
            return View("RegisterAsUserCompleted", userId);
        }



        [HttpGet]
        [AllowAnonymous]
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
                string message = "There was a problem with login. Try again, please.";
                return View("CustomError", message);
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
