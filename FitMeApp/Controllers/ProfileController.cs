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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitMeApp.Models.ExcelModels;
using FitMeApp.Services.Contracts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FitMeApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly ITrainingService _trainingService;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public ProfileController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IGymService gymService,
            ITrainerService trainerService,
            ITrainingService trainingService,
            IScheduleService scheduleService,
            ILogger<ProfileController> logger,
            IWebHostEnvironment appEnvironment,
            IFileService fileService,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _gymService = gymService;
            _trainerService = trainerService;
            _trainingService = trainingService;
            _scheduleService = scheduleService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
            _appEnvironment = appEnvironment;
            _fileService = fileService;
            _configuration = configuration;
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

        public IActionResult WriteUsersListToExcel() 
        {
            try
            {
                var users = _userManager.Users;
                var usersExcel = new List<UserExcelModel>();
                var positionNumber = 0;

                foreach (var user in users)
                {
                    positionNumber++;
                    usersExcel.Add(new UserExcelModel()
                    {
                        PositionNumber = positionNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        YearOfBirth = user.Year
                    });

                }

                DataTable table =  (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(usersExcel), (typeof(DataTable)));
                string fullPath = @"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\ExcelFiles\Users.xlsx";
                string tableName = "Users";
                _fileService.WriteToExcel(table, fullPath, tableName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);                
            }

            return RedirectToAction("UsersList");
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
        public IActionResult TrainerApplicationsList()
        {
            var trainerAppViewModels = new List<TrainerApplicationViewModel>();
            var allTrainerAppModels = _trainerService.GetAllTrainerApplications();
            foreach (var trainerAppModel in allTrainerAppModels)
            {
                trainerAppViewModels.Add(_mapper.MapTrainerApplicationModelToViewModel(trainerAppModel));
            }
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

                    string localPath = DefaultSettingsStorage.ApproveAppMessagePath;
                    string text = string.Empty;
                    try
                    {
                       text = await _fileService.GetTextContentFromFile(localPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return RedirectToAction("TrainerApplicationsList");
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "Failed to approve application for Trainer Role. Please try again"
                };
                return View("CustomError", error);
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

                string localPath = DefaultSettingsStorage.RejectAppMessagePath;
                string text = string.Empty;
                try
                {
                    text = await _fileService.GetTextContentFromFile(localPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return RedirectToAction("TrainerApplicationsList");
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "Failed to reject application for Trainer Role. Please try again"
                };
                return View("CustomError", error);
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
                            //передать сообщение о невозможности удаления
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with delete User. Try again, please."
                };
                return View("CustomError", error);
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


        public async Task<IActionResult> EditPersonalData(string id)
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with edit User. Try again, please."
                };
                return View("CustomError", error);
            }
        }



        [HttpPost]
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
                            string rootPath = _appEnvironment.WebRootPath;
                            string avatarPath = _fileService.SaveAvatarFileAsync(user.Id, model.AvatarFile, rootPath);
                            if (!string.IsNullOrEmpty(avatarPath))
                            {
                                user.AvatarPath = avatarPath;
                            }
                        }

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            if (!modelEmailConfirmed)
                            {
                                return RedirectToAction("SendMailToConfirmEmail", new {userId = user.Id});
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with edit User. Try again, please."
                };
                return View("CustomError", error);
            }
        }


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

            CustomErrorViewModel error = new CustomErrorViewModel()
            {
                Message = "Failed to verify email address. Please, try again in you Profile"
            };
            return View("CustomError", error);
        }




        //Change password

        public async Task<IActionResult> ChangePassword(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                ChangePasswordViewModel model = new ChangePasswordViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with change Password. Try again, please."
                };
                return View("CustomError", error);
            }
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with change Password. Try again, please."
                };
                return View("CustomError", error);
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
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with change users Password. Try again, please."
                };
                return View("CustomError", error);
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

            ViewBag.ActualEventsCount = _scheduleService.GetActualEventsCountByTrainer(trainer.Id);
            return View(trainerViewModel);

        }

        public async Task<IActionResult> EditTrainerJobData()
        {
            var trainer = await _userManager.GetUserAsync(User);
            var trainerModel = _trainerService.GetTrainerWithGymAndTrainings(trainer.Id);
            TrainerViewModel trainerViewModel = _mapper.MapTrainerModelToViewModel(trainerModel);
            EditTrainerJobDataModel trainerJobData = new EditTrainerJobDataModel()
            {
                Id = trainerViewModel.Id,
                GymId = trainerViewModel.Gym.Id,
                GymName = trainerViewModel.Gym.Name,
                Specialization = trainerViewModel.Specialization,
                TrainingsId = trainerViewModel.Trainings.Select(x => x.Id).ToList()
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
                    var availableTrainerSubscriptionsBySelectedGym = _gymService.GetSubscriptionsByFilterByUser(changedModel.Id,
                          new List<SubscriptionValidStatusEnum>() { SubscriptionValidStatusEnum.validNow },
                          new List<int>() { changedModel.GymId });
                    if (availableTrainerSubscriptionsBySelectedGym is null || availableTrainerSubscriptionsBySelectedGym.Count() == 0)
                    {
                        ModelState.AddModelError("", "You don't have available trainer subscription for selected Gym.");
                    }
                    else
                    {
                        List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                        foreach (var trainingId in changedModel.TrainingsId)
                        {
                            trainings.Add(new TrainingViewModel()
                            {
                                Id = trainingId
                            });
                        }

                        TrainerViewModel newTrainerInfo = new TrainerViewModel()
                        {
                            Id = changedModel.Id,
                            Specialization = changedModel.Specialization,
                            Trainings = trainings,
                            Gym = new GymViewModel()
                            {
                                Id = changedModel.GymId
                            }
                        };

                        int previousGymId = _gymService.GetGymIdByTrainer(changedModel.Id);
                        var trainerModel = _mapper.MapTrainerViewModelToModel(newTrainerInfo);
                        _trainerService.UpdateTrainerWithGymAndTrainings(trainerModel);

                        if (previousGymId != changedModel.GymId)
                        {
                            _trainerService.DeleteTrainerWorkHoursByTrainer(changedModel.Id);
                            return RedirectToAction("EditTrainerWorkHours");
                        }

                        return RedirectToAction("TrainerPersonalAndJobData");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "the form is filled out incorrectly");
                }

                ViewBag.AllTrainings = _trainingService.GetAllTrainingModels();
                ViewBag.AllGyms = _gymService.GetAllGymModels().Where(x => x.Id != changedModel.GymId);
                return View(changedModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with change users job data. Try again, please."
                };
                return View("CustomError", error);
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
        public IActionResult EditTrainerWorkHours(List<TrainerWorkHoursViewModel> newWorkHours)
        {
            try
            {
                newWorkHours.RemoveAll(x => x.StartTime == "0.00" && x.EndTime == "0.00"); //Cut off all "day off" from model
                foreach (var model in newWorkHours)                                        
                {
                    int startTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.StartTime);
                    int endTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.EndTime);
                    if (startTimeInt > endTimeInt)
                    {
                        ModelState.AddModelError("NewDataConflict", "Start work time can't be later than End work time");
                        return View(newWorkHours);
                    }
                }

                string trainerId = _userManager.GetUserId(User);
                foreach (var model in newWorkHours)                         //full required fields in work hours model for NEW days
                {
                    model.TrainerId = trainerId;
                    if (model.GymWorkHoursId == 0)
                    {
                        int gymId = _gymService.GetGymIdByTrainer(trainerId);
                        model.GymWorkHoursId = _gymService.GetGymWorkHoursId(gymId, model.DayName);
                    }
                }
               
                var newWorkHoursModels = new List<TrainerWorkHoursModel>();

                foreach (var viewModel in newWorkHours)
                {
                    newWorkHoursModels.Add(_mapper.MapTrainerWorkHoursViewModelToModel(viewModel));
                }

                bool result = _trainerService.CheckFacilityUpdateTrainerWorkHours(newWorkHoursModels);
                if (result)
                {
                    bool updateSuccess = _trainerService.UpdateTrainerWorkHours(newWorkHoursModels);
                    if (!updateSuccess)
                    {
                        _logger.LogError("Update  trainer work hours failed", $"Update work hours for user id: {trainerId} failed");
                    }
                    return RedirectToAction("TrainerPersonalAndJobData");
                }
                else
                {
                    foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                    {
                        if (!newWorkHours.Select(x => x.DayName).Contains((DayOfWeek)item))
                        {
                            newWorkHours.Add(new TrainerWorkHoursViewModel()
                            {
                                DayName = (DayOfWeek)item,
                                StartTime = "0.00",
                                EndTime = "0.00"
                            });
                        }
                    }
                    List<TrainerWorkHoursViewModel> orderedWorkHours = newWorkHours.OrderBy(x => ((int)x.DayName)).ToList();
                    ModelState.AddModelError("NewDataConflict", "There is a conflict in the entered data. Make sure that the gym schedule or current events do not conflict with new data.");
                    return View(orderedWorkHours);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with change work hours data. Try again, please."
                };
                return View("CustomError", error);
            }
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
                var clientSubscriptionsModels = _gymService.GetUserSubscriptions(clientId);
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
            var userSubscriptionModels = _gymService.GetUserSubscriptions(user.Id);

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
            var subscriptionModels = _gymService.GetSubscriptionsByFilterByUser(user.Id, validStatuses, gymIds);

            foreach (var sudscriptionModel in subscriptionModels)
            {
                userSubscriptionViewModels.Add(_mapper.MapUserSubscriptionModelToViewModel(sudscriptionModel));
            }

            userSubscriptionViewModels = userSubscriptionViewModels.OrderByDescending(x => x.EndDate).ToList();
            ViewBag.Gyms = _gymService.GetAllGymModels().ToList();
            return View(userSubscriptionViewModels);
        }

    }
}
