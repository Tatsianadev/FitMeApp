using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Models;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Models.ExcelModels;
using FitMeApp.Resources;
using FitMeApp.Services.Contracts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace FitMeApp.Controllers
{
    public sealed class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly ITrainingService _trainingService;
        private readonly IScheduleService _scheduleService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IFileService _fileService;
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;

        public ProfileController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IGymService gymService,
            ITrainerService trainerService,
            ITrainingService trainingService,
            IScheduleService scheduleService,
            ISubscriptionService subscriptionService,
            ILogger<ProfileController> logger,
            IWebHostEnvironment appEnvironment,
            IFileService fileService,
            IReportService reportService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _gymService = gymService;
            _trainerService = trainerService;
            _trainingService = trainingService;
            _scheduleService = scheduleService;
            _subscriptionService = subscriptionService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
            _appEnvironment = appEnvironment;
            _fileService = fileService;
            _reportService = reportService;
            _emailService = emailService;
        }


        //admin options

        [Authorize(Roles = "admin")]
        public IActionResult UsersList()
        {
            var users = _userManager.Users.ToList();
            ViewBag.RoleNames = _roleManager.Roles.Select(x => x.Name).ToList();
            return View(users);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DownloadUsersListExcelFile(List<string> selectedIds)
        {
            string relativePath = Resources.Resources.UsersFileDestinationPath;
            string absPath = Environment.CurrentDirectory + relativePath;
            try
            {
                await CreateUsersListExcelFileAsync(selectedIds, absPath);
            }
            catch (Exception)
            {
                return View("CustomError", "Failed attempt to create Users list. Please, try again later");
            }

            if (System.IO.File.Exists(absPath))
            {
                var fileStream = System.IO.File.OpenRead(absPath);
                var contentType = "application/vnd.ms-excel";
                var fileName = Path.GetFileName(absPath);

                return File(fileStream, contentType, fileName);
            }

            return View("CustomError", "File is not found. Please, try again later.");
        }



        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UsersList(List<string> selectedRolesNames)
        {
            if (selectedRolesNames.Count == 0)
            {
                return RedirectToAction("UsersList");
            }

            List<User> filteredUsers = new List<User>();
            foreach (var role in selectedRolesNames)
            {
                filteredUsers.AddRange(await _userManager.GetUsersInRoleAsync(role));
            }

            filteredUsers.Distinct();
            ViewBag.RoleNames = _roleManager.Roles.Select(x => x.Name).ToList();
            return View(filteredUsers);
        }



        [Authorize(Roles = "admin")]
        public IActionResult TrainerApplicationsList(bool showOnlyToUpdateLicenseList = false)
        {
            var trainerAppViewModels = new List<TrainerApplicationViewModel>();
            var allTrainerAppModels = _trainerService.GetAllTrainerApplications().ToList();
            var trainerAppModelsToDisplay = new List<TrainerApplicationModel>();
            int appCount = allTrainerAppModels.Count;
            int newAppCount = 0;
            int appToUpdateLicensesCount = 0;

            var trainersIds = _trainerService.GetAllTrainerWorkLicenses().Select(x => x.TrainerId).ToList();

            if (showOnlyToUpdateLicenseList)
            {
                trainerAppModelsToDisplay = allTrainerAppModels.Where(x => trainersIds.Contains(x.UserId)).ToList();
                appToUpdateLicensesCount = trainerAppModelsToDisplay.Count;
                newAppCount = appCount - appToUpdateLicensesCount;
            }
            else
            {
                trainerAppModelsToDisplay = allTrainerAppModels.Where(x => !trainersIds.Contains(x.UserId)).ToList();
                newAppCount = trainerAppModelsToDisplay.Count;
                appToUpdateLicensesCount = appCount - newAppCount;
            }

            foreach (var trainerAppModel in trainerAppModelsToDisplay)
            {
                var trainerAppViewModel = _mapper.MapTrainerApplicationModelToViewModel(trainerAppModel);
                trainerAppViewModel.GymName = _gymService.GetGymModel(trainerAppModel.GymId).Name;
                trainerAppViewModels.Add(trainerAppViewModel);
            }

            ViewBag.NewAppCount = newAppCount;
            ViewBag.AppToUpdateLicensesCount = appToUpdateLicensesCount;

            return View(trainerAppViewModels);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveTrainerApplication(string trainerId)
        {
            try
            {
                if (string.IsNullOrEmpty(trainerId))
                {
                    throw new ArgumentException("applicationId parameter is null or empty", nameof(trainerId));
                }

                bool approveSucceed = _trainerService.ApproveTrainerApplication(trainerId);
                if (approveSucceed)
                {
                    var user = await _userManager.FindByIdAsync(trainerId);
                    await _userManager.AddToRoleAsync(user, RolesEnum.trainer.ToString());
                    string localPath = Resources.Resources.ApproveAppMessagePath;
                    string text = string.Empty;
                    try
                    {
                        text = await _fileService.GetTextContentFromFileAsync(localPath);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    string toEmail = DefaultSettingsStorage.ReceiverEmail;
                    string fromEmail = DefaultSettingsStorage.SenderEmail;
                    string subject = "Approval application";
                    string htmlContent = "<strong>" + text + "</strong>";

                    await _emailService.SendEmailAsync(toEmail, user.FirstName, fromEmail, subject, text, htmlContent);
                }
                return RedirectToAction("TrainerApplicationsList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed to approve application for Trainer Role. Please try again";
                return View("CustomError", message);
            }

        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectTrainerApplication(string userId, int applicationId)
        {
            try
            {
                if (applicationId <= 0)
                {
                    throw new ArgumentException("Parameter applicationId equals zero or less.");
                }

                _trainerService.DeleteTrainerApplication(applicationId);
                var user = await _userManager.FindByIdAsync(userId);
                string localPath = Resources.Resources.RejectAppMessagePath;
                string text = string.Empty;
                try
                {
                    text = await _fileService.GetTextContentFromFileAsync(localPath);
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                string toEmail = DefaultSettingsStorage.ReceiverEmail;
                string fromEmail = DefaultSettingsStorage.SenderEmail;
                string subject = "Reject application";
                string htmlContent = "<strong>" + text + "</strong>";

                await _emailService.SendEmailAsync(toEmail, user.FirstName, fromEmail, subject, text, htmlContent);

                return RedirectToAction("TrainerApplicationsList");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed to reject application for Trainer Role. Please try again";
                return View("CustomError", message);
            }
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Contains(RolesEnum.trainer.ToString()))
                    {
                        int actualEventsCount = _scheduleService.GetActualEventsCountByTrainer(user.Id);
                        if (actualEventsCount > 0)
                        {
                            //todo trainer has events. alert that failed delete trying
                            return RedirectToAction("UsersList");
                        }
                        else
                        {
                            _trainerService.DeleteTrainer(user.Id);
                        }
                    }
                    await _userManager.DeleteAsync(user);
                }
                return RedirectToAction("UsersList");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with delete User. Try again, please.";
                return View("CustomError", message);
            }
        }




        //User profile

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> UserPersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.ApplyedForTrainerRole = _trainerService.GetTrainerApplicationByUser(user.Id) != null;
            return View(user);
        }


        [Authorize]
        public async Task<IActionResult> EditPersonalData(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                EditUserViewModel model = new EditUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    Year = user.Year,
                    Gender = user.Gender,
                    AvatarPath = user.AvatarPath
                };
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with edit User. Try again, please.";
                return View("CustomError", message);
            }
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPersonalData(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        bool modelEmailConfirmed = (model.Email == user.Email && user.EmailConfirmed);

                        user.UserName = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Year = model.Year;
                        user.Gender = model.Gender;

                        if (model.AvatarFile != null)
                        {
                            string absolutePath = "/Content/Upload/" + model.Id + "/AvatarPath/" + model.AvatarFile.GetHashCode() + ".jpg";
                            string fullPath = _appEnvironment.WebRootPath + absolutePath;
                            await _fileService.SaveFileAsync(model.AvatarFile, fullPath);
                            user.AvatarPath = absolutePath;

                        }

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            if (!modelEmailConfirmed)
                            {
                                return RedirectToAction("SendMailToConfirmEmail", new { userId = user.Id });
                            }

                            if (User.IsInRole("admin"))
                            {
                                return RedirectToAction("UsersList");
                            }
                            else if (User.IsInRole("trainer"))
                            {
                                return RedirectToAction("TrainerPersonalAndJobData");
                            }
                            else
                            {
                                return RedirectToAction("UserPersonalData");
                            }
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with edit User. Try again, please.";
                return View("CustomError", message);
            }
        }


        [Authorize]
        public async Task<IActionResult> SendMailToConfirmEmail(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Profile",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study cases - constant
            string fromEmail = DefaultSettingsStorage.SenderEmail;
            string plainTextContent = "To finish update your Profile please follow the link <a href=\"" + callbackUrl + "\">Finish</a>";
            string htmlContent = "<strong>To finish update your Profile please follow the link  <a href=\"" + callbackUrl + "\">Finish</a></strong>";
            string subject = "Confirm email";

            await _emailService.SendEmailAsync(toEmail, user.FirstName, fromEmail, subject, plainTextContent, htmlContent);

            return Content("To finish update your Profile check your Email, please.");
        }


        [Authorize]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
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
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            string message = "Failed to verify email address. Please, try again in you Profile";
            return View("CustomError", message);
        }




        //Change password
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                ChangePasswordViewModel model = new ChangePasswordViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email
                };
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "trainer, user")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordWithOldPassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            if (User.IsInRole("trainer"))
                            {
                                return RedirectToAction("TrainerPersonalAndJobData");
                            }
                            else
                            {
                                return RedirectToAction("UserPersonalData");
                            }

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User is not found");
                    }
                }
                return RedirectToAction("ChangePassword", new { id = model.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with change Password. Try again, please.";
                return View("CustomError", message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordWithoutOldPassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        var _passwordValidator =
                            HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                        var _passwordHasher =
                            HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                        var result = await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                            await _userManager.UpdateAsync(user);
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
                return RedirectToAction("ChangePassword", new { id = model.Id }); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with change users Password. Try again, please.";
                return View("CustomError", message);
            }
        }



        // Trainer profile

        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> TrainerPersonalAndJobData()
        {
            var trainer = await _userManager.GetUserAsync(User);
            var trainerModel = _trainerService.GetTrainerWithGymAndTrainings(trainer.Id);
            TrainerViewModel trainerViewModel = _mapper.MapTrainerModelToViewModel(trainerModel);
            trainerViewModel.Email = trainer.Email;
            trainerViewModel.Phone = trainer.PhoneNumber;
            trainerViewModel.Year = trainer.Year;
            if (trainerModel.Specialization != TrainerSpecializationsEnum.group.ToString())
            {
                trainerViewModel.PersonalTrainingPrice = _trainingService.GetPrice(trainer.Id);
            }

            ViewBag.ActualEventsCount = _scheduleService.GetActualEventsCountByTrainer(trainer.Id);
            return View(trainerViewModel);
        }


        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> EditTrainerJobData()
        {
            var trainer = await _userManager.GetUserAsync(User);
            var trainerModel = _trainerService.GetTrainerWithGymAndTrainings(trainer.Id);
            var workLicense = _trainerService.GetTrainerWorkLicenseByTrainer(trainer.Id);
            var pricePerHour = _trainingService.GetPrice(trainer.Id);
            TrainerViewModel trainerViewModel = _mapper.MapTrainerModelToViewModel(trainerModel);
            EditTrainerJobDataModel trainerJobData = new EditTrainerJobDataModel()
            {
                Id = trainerViewModel.Id,
                GymId = trainerViewModel.Gym.Id,
                GymName = trainerViewModel.Gym.Name,
                Specialization = trainerViewModel.Specialization,
                TrainingsId = trainerViewModel.Trainings.Select(x => x.Id).ToList(),
                PricePerHour = pricePerHour,
                WorkLicense = workLicense
            };

            ViewBag.AllTrainings = _trainingService.GetAllTrainingModels();
            ViewBag.AllGyms = _gymService.GetAllGymModels().Where(x => x.Id != trainerModel.Gym.Id);
            return View(trainerJobData);
        }


        [Authorize(Roles = "trainer")]
        [HttpPost]
        public IActionResult EditTrainerJobData(EditTrainerJobDataModel changedModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                    foreach (var trainingId in changedModel.TrainingsId)
                    {
                        trainings.Add(new TrainingViewModel()
                        {
                            Id = trainingId
                        });
                    }

                    string specialization = string.Empty;
                    if (trainings.Count > 0)
                    {
                        specialization = _trainerService
                            .GetTrainerSpecializationByTrainings(trainings.Select(x => x.Id).ToList()).ToString();
                    }

                    TrainerViewModel newTrainerInfo = new TrainerViewModel()
                    {
                        Id = changedModel.Id,
                        Specialization = specialization,
                        Trainings = trainings,
                        Gym = new GymViewModel()
                        {
                            Id = changedModel.GymId
                        }
                    };

                    var trainerModel = _mapper.MapTrainerViewModelToModel(newTrainerInfo);
                    _trainerService.UpdateTrainerWithGymAndTrainings(trainerModel);
                    if (specialization != TrainerSpecializationsEnum.group.ToString())
                    {
                        _trainingService.UpdatePersonalTrainingPrice(changedModel.Id, changedModel.PricePerHour);
                    }
                    return RedirectToAction("TrainerPersonalAndJobData");
                }
                else
                {
                    ModelState.AddModelError("", "The form is filled out incorrectly. At least one type of trainings should be chosen.");
                }

                ViewBag.AllTrainings = _trainingService.GetAllTrainingModels();
                ViewBag.AllGyms = _gymService.GetAllGymModels().Where(x => x.Id != changedModel.GymId);
                return View(changedModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with change users job data. Try again, please.";
                return View("CustomError", message);
            }
        }


        public async Task<IActionResult> RefuseTrainerRole(string userId)
        {
            var actualEventsCount = _scheduleService.GetActualEventsCountByTrainer(userId);
            if (actualEventsCount > 0)
            {
                return View("FailedTryToRefuseTrainerRole");
            }

            _trainerService.DeleteTrainer(userId);
            var user = await _userManager.GetUserAsync(User);
            await _userManager.RemoveFromRoleAsync(user, RolesEnum.trainer.ToString());
            return RedirectToAction("UserPersonalData");
        }


        public IActionResult UpdateTrainerWorkLicense()
        {
            ViewBag.Gyms = _gymService.GetAllGymModels();
            return View(new TrainerWorkLicenseViewModel());
        }


        [HttpPost]
        [Authorize(Roles = "trainer")]
        public IActionResult UpdateTrainerWorkLicense(TrainerWorkLicenseViewModel newLicenseModel)
        {
            try
            {
                if (string.IsNullOrEmpty(newLicenseModel.ContractNumber))
                {
                    ModelState.AddModelError("", "Write the contract number over");
                    return View(newLicenseModel);
                }

                if (newLicenseModel.StartDate >= newLicenseModel.EndDate)
                {
                    ModelState.AddModelError("", "The date of the start contract cannot be later or equal to the expiration date.");
                    return View(newLicenseModel);
                }

                var userId = _userManager.GetUserId(User);

                TrainerApplicationModel trainerAppModel = new TrainerApplicationModel()
                {
                    UserId = userId,
                    ContractNumber = newLicenseModel.ContractNumber,
                    GymId = newLicenseModel.GymId,
                    StartDate = newLicenseModel.StartDate,
                    EndDate = newLicenseModel.EndDate,
                    ApplyingDate = DateTime.Today
                };

                int appId = _trainerService.AddTrainerApplication(trainerAppModel);
                if (appId != 0)
                {
                    return RedirectToAction("TrainerPersonalAndJobData");
                }
                else
                {
                    throw new Exception("Failed attempt to add records (new TrainerApplication) into DB");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed attempt to update a work license. Please, try again later.";
                return View("CustomError", message);
            }
        }


        [Authorize(Roles = "trainer")]
        public IActionResult EditTrainerWorkHours()
        {
            string trainerId = _userManager.GetUserId(User);
            var workHoursModel = _trainerService.GetWorkHoursByTrainer(trainerId);
            List<TrainerWorkHoursViewModel> workHoursViewModel = new List<TrainerWorkHoursViewModel>();

            if (workHoursModel.Count() == 0)
            {
                foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                {
                    workHoursViewModel.Add(new TrainerWorkHoursViewModel()
                    {
                        DayName = (DayOfWeek)item,
                        StartTime = "0.00",
                        EndTime = "0.00"
                    });
                }
            }
            else
            {
                foreach (var item in workHoursModel)
                {
                    workHoursViewModel.Add(_mapper.MapTrainerWorkHoursModelToViewModel(item));
                }

                foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                {
                    if (!workHoursViewModel.Select(x => x.DayName).Contains((DayOfWeek)item))
                    {
                        workHoursViewModel.Add(new TrainerWorkHoursViewModel()
                        {
                            DayName = (DayOfWeek)item,
                            StartTime = "0.00",
                            EndTime = "0.00"
                        });
                    }
                }
            }

            List<TrainerWorkHoursViewModel> orderedWorkHours = workHoursViewModel.OrderBy(x => ((int)x.DayName)).ToList();

            return View(orderedWorkHours);
        }


        [Authorize(Roles = "trainer")]
        [HttpPost]
        public IActionResult EditTrainerWorkHours(List<TrainerWorkHoursViewModel> model)
        {
            if (ModelState.IsValid && model != null)
            {
                string trainerId = _userManager.GetUserId(User);

                //make copy of passed model for the following changes
                //fill in all required fields for NEW days
                //cut off all "day off" from the model
                var newWorkHours = new List<TrainerWorkHoursViewModel>();
                foreach (var item in model.Where((x => x.StartTime != "0.00" && x.EndTime != "0.00")))
                {
                    item.TrainerId = trainerId;
                    if (item.GymWorkHoursId == 0)
                    {
                        int gymId = _gymService.GetGymIdByTrainer(trainerId);
                        item.GymWorkHoursId = _gymService.GetGymWorkHoursId(gymId, item.DayName);
                    }
                    newWorkHours.Add(item);
                }
                
                if (!newWorkHours.Any())
                {
                    if (TryDeleteTrainerWorkHours(trainerId))
                    {
                        return RedirectToAction("TrainerPersonalAndJobData");
                    }

                    ModelState.AddModelError("NewDataConflict", "You can't delete work hours while you have actual events in your schedule.");
                    return View(model);
                }

                var newWorkHoursModels = new List<TrainerWorkHoursModel>();
                foreach (var viewModel in newWorkHours)
                {
                    newWorkHoursModels.Add(_mapper.MapTrainerWorkHoursViewModelToModel(viewModel));
                }

                try
                {
                    bool updateSuccess = _trainerService.TryUpdateTrainerWorkHours(newWorkHoursModels);
                    if (updateSuccess)
                    {
                        return RedirectToAction("TrainerPersonalAndJobData");
                    }

                    ModelState.AddModelError("NewDataConflict", "There is a conflict in the entered data. Make sure that the gym schedule or current events do not conflict with the new data.");
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    string message = "There was a problem with change work hours data. Try again, please.";
                    return View("CustomError", message);
                }
            }

            if (model == null)
            {
                _logger.LogError("Null value has passed into the EditTrainerWorkHours post method");
                return RedirectToAction("EditTrainerWorkHours");
            }

            return View(model);
        }



        [Authorize(Roles = "trainer")]
        public IActionResult ClientsList()
        {
            string trainerId = _userManager.GetUserId(User);
            List<string> clientsId = _trainerService.GetAllClientsIdByTrainer(trainerId).ToList();
            List<User> allClientsByTrainer = new List<User>();
            foreach (var clientId in clientsId)
            {
                var client = _userManager.Users.First(x => x.Id == clientId);
                allClientsByTrainer.Add(client);
            }

            return View(allClientsByTrainer);
        }


        [HttpPost]
        [Authorize(Roles = "trainer")]
        public IActionResult ClientsList(bool showOnlyActualClients)
        {
            if (showOnlyActualClients)
            {
                string trainerId = _userManager.GetUserId(User);
                List<string> clientsId = _trainerService.GetActualClientsIdByTrainer(trainerId).ToList();
                List<User> actualClientsByTrainer = new List<User>();
                foreach (var clientId in clientsId)
                {
                    var client = _userManager.Users.First(x => x.Id == clientId);
                    actualClientsByTrainer.Add(client);
                }

                return View(actualClientsByTrainer);
            }

            return RedirectToAction("ClientsList");
        }



        [Authorize(Roles = "trainer")]
        public IActionResult ClientSubscription(string clientId)
        {
            if (clientId != null)
            {
                var trainerId = _userManager.GetUserId(User);
                int gymId = _trainerService.GetTrainerWithGymAndTrainings(trainerId).Gym.Id;

                var clientSubscriptionsModels = _subscriptionService.GetUserSubscriptions(clientId).Where(x => x.GymId == gymId);
                List<UserSubscriptionViewModel> userSubscViewModels = new List<UserSubscriptionViewModel>();
                foreach (var modelItem in clientSubscriptionsModels)
                {
                    userSubscViewModels.Add(_mapper.MapUserSubscriptionModelToViewModel(modelItem));
                }
                return View(userSubscViewModels);
            }
            return RedirectToAction("ClientsList");
        }


        [Authorize]
        public async Task<IActionResult> UserSubscriptions()
        {
            var user = await _userManager.GetUserAsync(User);
            var userSubscriptionModels = _subscriptionService.GetUserSubscriptions(user.Id);

            List<UserSubscriptionViewModel> userSubscriptionViewModels = new List<UserSubscriptionViewModel>();
            foreach (var subscription in userSubscriptionModels)
            {
                userSubscriptionViewModels.Add(_mapper.MapUserSubscriptionModelToViewModel(subscription));
            }

            userSubscriptionViewModels = userSubscriptionViewModels.OrderByDescending(x => x.EndDate).ToList();
            ViewBag.Gyms = _gymService.GetAllGymModels().ToList();
            return View(userSubscriptionViewModels);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserSubscriptions(List<SubscriptionValidStatusEnum> validStatuses, List<int> gymIds)
        {
            if (validStatuses.Count == 0 && gymIds.Count == 0)
            {
                return RedirectToAction("UserSubscriptions");
            }

            if (validStatuses.Count == 0)
            {
                validStatuses = Enum.GetValues(typeof(SubscriptionValidStatusEnum)).Cast<SubscriptionValidStatusEnum>().ToList();
            }

            if (gymIds.Count == 0)
            {
                gymIds = _gymService.GetAllGymModels().Select(x => x.Id).ToList();
            }

            var user = await _userManager.GetUserAsync(User);
            List<UserSubscriptionViewModel> userSubscriptionViewModels = new List<UserSubscriptionViewModel>();
            var subscriptionModels = _subscriptionService.GetSubscriptionsByFilterByUser(user.Id, validStatuses, gymIds);

            foreach (var sudscriptionModel in subscriptionModels)
            {
                userSubscriptionViewModels.Add(_mapper.MapUserSubscriptionModelToViewModel(sudscriptionModel));
            }

            userSubscriptionViewModels = userSubscriptionViewModels.OrderByDescending(x => x.EndDate).ToList();
            ViewBag.Gyms = _gymService.GetAllGymModels().ToList();
            return View(userSubscriptionViewModels);
        }





        private async Task CreateUsersListExcelFileAsync(List<string> selectedIds, string absPath)
        {
            if (selectedIds.Count != 0)
            {
                var users = _userManager.Users.ToList().Where(x => selectedIds.Contains(x.Id));
                var usersExcel = new List<UserExcelModel>();
                var positionNumber = 0;

                foreach (var user in users)
                {
                    positionNumber++;
                    usersExcel.Add(new UserExcelModel()
                    {
                        Id = positionNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Year = user.Year
                    });
                }

                try
                {
                    DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(usersExcel), (typeof(DataTable)));
                    await _reportService.CreateUsersListReportAsync(table, absPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }

        private bool TryDeleteTrainerWorkHours(string trainerId)
        {
            int actualEventsCount = _scheduleService.GetActualEventsCountByTrainer(trainerId);
            if (actualEventsCount == 0)
            {
                _trainerService.DeleteTrainerWorkHoursByTrainer(trainerId);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
