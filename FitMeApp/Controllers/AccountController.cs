using FitMeApp.Common;
using FitMeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Mapper;
using System.Collections.Generic;

namespace FitMeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFitMeService _fitMeService;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;
       
        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,
            IFitMeService fitMeService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fitMeService = fitMeService;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
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
                        Avatar = "defaultAvatar.jpg"
                        
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, RolesEnum.user.ToString());
                        await _signInManager.SignInAsync(user, false);
                        if (model.Role == RolesEnum.trainer)
                        {                           
                            return RedirectToAction("RegisterTrainerPart");
                        }
                        
                        return RedirectToAction("RegisterAsUserCompleted", new { applyedForTrainerRole = false });
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

        public IActionResult RegisterTrainerPart()
        {
            ViewBag.Gyms = _fitMeService.GetAllGymModels();
            ViewBag.Specializations = Enum.GetValues(typeof(TrainerSpecializationsEnum));
            ViewBag.Gender = Enum.GetValues(typeof(GenderEnum));
            ViewBag.Trainings = _fitMeService.GetAllTrainingModels();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTrainerPart(TrainerRoleAppViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                    foreach (var trainingId in model.TrainingsId)
                    {
                        trainings.Add(new TrainingViewModel()
                        {
                            Id = trainingId
                        });
                    }
                    var user = await _userManager.GetUserAsync(User);
                    user.PhoneNumber = model.PhoneNumber;
                    user.Gender = model.Gender;
                    user.Year = model.Year;

                    TrainerViewModel trainerViewModel = new TrainerViewModel()
                    {
                        Id = user.Id,                        
                        Specialization = model.Specialization,                       
                        Trainings = trainings,
                        Status = TrainerApproveStatusEnum.pending,
                        Gym = new GymViewModel()
                        {
                            Id = model.GymId
                        }
                    };

                    var trainerModel = _mapper.MappTrainerViewModelToModelBase(trainerViewModel);
                    var result = _fitMeService.AddTrainer(trainerModel);
                    if (result)
                    {
                        foreach (var trainingId in model.TrainingsId)
                        {
                            _fitMeService.AddTrainingTrainerConnection(user.Id, trainingId);                           
                        }
                       
                        return RedirectToAction("RegisterAsUserCompleted", new { applyedForTrainerRole = true } );
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add trainers data. Please check all fields and try one again");
                    }
                }

                ViewBag.Gyms = _fitMeService.GetAllGymModels();
                ViewBag.Specializations = Enum.GetValues(typeof(TrainerSpecializationsEnum));
                ViewBag.Gender = Enum.GetValues(typeof(GenderEnum));
                ViewBag.Trainings = _fitMeService.GetAllTrainingModels();

                return View(model);
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


        public async Task<IActionResult> RegisterAsUserCompleted(bool applyedForTrainerRole)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.ApplyedForTrainerRole = applyedForTrainerRole;
            return View(user);
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
