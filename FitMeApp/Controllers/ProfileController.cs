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
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFitMeService _fitMeService;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ProfileController(UserManager<User> userManager, IFitMeService fitMeService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
            _logger = loggerFactory.CreateLogger("ProfileLogger");
            _mapper = new ModelViewModelMapper();
        }


        //admin options

        [Authorize(Roles = "admin")]
        public IActionResult UsersList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "admin")]
        public IActionResult TrainersList()
        {
            var trainersModels = _fitMeService.GetAllTrainersNames();
            List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
            foreach (var trainerModel in trainersModels)
            {
                trainerViewModels.Add(_mapper.MappTrainerModelToViewModel(trainerModel));
            }
            return View(trainerViewModels);
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
                    if (userRoles.Contains(Common.RolesEnum.trainer.ToString()))
                    {
                        int actualEventsCount = _fitMeService.GetActualEventsCountByTrainer(user.Id);
                        int actualSubscriptionsCount = _fitMeService.GetActualSubscriptionsCountByTrainer(user.Id);
                        if (actualEventsCount > 0 || actualSubscriptionsCount > 0)
                        {
                            //передать сообщение о невозможности удаления
                            return RedirectToAction("UsersList");
                        }
                        else
                        {
                            bool deleteFromTrainersTableResult = _fitMeService.DeleteTrainer(user.Id);
                            bool deleteAllTrainingsTrainerConnectionResult = _fitMeService.DeleteAllTrainingTrainerConnectionsByTrainer(user.Id);
                            bool deleteTrainerWorkHoursResult = _fitMeService.DeleteTrainerWorkHoursByTrainer(user.Id);
                            if (deleteFromTrainersTableResult == false || deleteAllTrainingsTrainerConnectionResult == false || deleteTrainerWorkHoursResult == false)
                            {
                                CustomErrorViewModel error = new CustomErrorViewModel()
                                {
                                    Message = "There was a problem with delete User. Try again, please."
                                };
                                return View("CustomError", error);
                            }
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
                    PhoneNumber = user.PhoneNumber,
                    Year = user.Year,
                    Gender = user.Gender,
                    Avatar = user.Avatar
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
                        user.UserName = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Year = model.Year;
                        user.Gender = model.Gender;
                        user.Avatar = model.Avatar;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
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
                return View(model);
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
            var trainerModel = _fitMeService.GetTrainerWithGymAndTrainings(trainer.Id);
            TrainerViewModel trainerViewModel = _mapper.MappTrainerModelToViewModel(trainerModel);
            trainerViewModel.Email = trainer.Email;
            trainerViewModel.Phone = trainer.PhoneNumber;
            trainerViewModel.Year = trainer.Year;

            List<TrainerWorkHoursViewModel> workHoursViewModel = new List<TrainerWorkHoursViewModel>();
            var workHoursModel = _fitMeService.GetWorkHoursByTrainer(trainer.Id);
            foreach (var item in workHoursModel)
            {
                workHoursViewModel.Add(_mapper.MappTrainerWorkHoursModelToViewModel(item));
            }

            List<string> workDays = new List<string>();
            Dictionary<DayOfWeek, string> startTime = new Dictionary<DayOfWeek, string>();
            Dictionary<DayOfWeek, string> endTime = new Dictionary<DayOfWeek, string>();
            foreach (var item in workHoursViewModel)
            {
                workDays.Add(item.DayName.ToString());
                startTime.Add(item.DayName, item.StartTime);
                endTime.Add(item.DayName, item.EndTime);
            }

            ViewBag.WorkDays = workDays;
            ViewBag.StartHours = startTime;
            ViewBag.EndHours = endTime;

            return View(trainerViewModel);

        }

        public async Task<IActionResult> EditTrainerJobData()
        {
            var trainer = await _userManager.GetUserAsync(User);
            var trainerModel = _fitMeService.GetTrainerWithGymAndTrainings(trainer.Id);
            TrainerViewModel trainerViewModel = _mapper.MappTrainerModelToViewModel(trainerModel);
            EditTrainerJobDataModel trainerJobData = new EditTrainerJobDataModel()
            {
                Id = trainerViewModel.Id,
                GymId = trainerViewModel.Gym.Id,
                GymName = trainerViewModel.Gym.Name,
                Specialization = trainerViewModel.Specialization,
                Status = trainerViewModel.Status,
                TrainingsId = trainerViewModel.Trainings.Select(x => x.Id).ToList()
            };

            ViewBag.AllTrainings = _fitMeService.GetAllTrainingModels();
            ViewBag.ActualEventsCount = _fitMeService.GetActualEventsCountByTrainer(trainer.Id);
            ViewBag.ActualSubscriptionsCount = _fitMeService.GetActualSubscriptionsCountByTrainer(trainer.Id);
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

                    TrainerViewModel newTrainerInfo = new TrainerViewModel()
                    {
                        Id = changedModel.Id,
                        Specialization = changedModel.Specialization,
                        Status = changedModel.Status,
                        Trainings = trainings,
                        Gym = new GymViewModel()
                        {
                            Id = changedModel.GymId
                        }
                    };

                    var trainerModel = _mapper.MappTrainerViewModelToModel(newTrainerInfo);
                    var result = _fitMeService.UpdateTrainerWithGymAndTrainings(trainerModel);

                    return RedirectToAction("TrainerPersonalAndJobData");
                }
                else
                {
                    ModelState.AddModelError("", "the form is filled out incorrectly");
                    return View(changedModel);
                }

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
            var workHoursModel = _fitMeService.GetWorkHoursByTrainer(trainerId);

            List<TrainerWorkHoursViewModel> workHoursViewModel = new List<TrainerWorkHoursViewModel>();
            foreach (var item in workHoursModel)
            {
                workHoursViewModel.Add(_mapper.MappTrainerWorkHoursModelToViewModel(item));
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
            List<TrainerWorkHoursViewModel> orderedWorkHours = workHoursViewModel.OrderBy(x => ((int)x.DayName)).ToList();

            return View(orderedWorkHours);
        }


        [Authorize(Roles = "trainer")]
        [HttpPost]
        public IActionResult EditTrainerWorkHours(List<TrainerWorkHoursViewModel> newWorkHours)
        {
            try
            {
                newWorkHours.RemoveAll(x => x.StartTime == "0.00" && x.EndTime == "0.00"); //удаляем все нерабочие дни из графика
                foreach (var model in newWorkHours)                                        //проверяем, что время начала работы не позднее времени окончания
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
                foreach (var model in newWorkHours)                         //заполняем недостающие данные модели для НОВЫХ рабочих дней
                {
                    model.TrainerId = trainerId;
                    if (model.GymWorkHoursId == 0)
                    {
                        int gymId = _fitMeService.GetGymIdByTrainer(trainerId);
                        model.GymWorkHoursId = _fitMeService.GetGymWorkHoursId(gymId, model.DayName);
                    }
                }
                var newWorkHoursModels = newWorkHours.Select(model => _mapper.MappTrainerWorkHoursViewModelToModel(model)).ToList();

                bool result = _fitMeService.CheckFacilityUpdateTrainerWorkHours(newWorkHoursModels);
                if (result)
                {
                    bool updateSuccess = _fitMeService.UpdateTrainerWorkHours(newWorkHoursModels);
                    if (updateSuccess)
                    {
                        return RedirectToAction("TrainerPersonalAndJobData");
                    }
                    else
                    {
                        return View(newWorkHours);
                    }
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
            List<string> clientsId = _fitMeService.GetAllClientsIdByTrainer(trainerId).ToList();
            List<User> allClientsByTrainer = new List<User>();
            foreach (var clientId in clientsId)
            {
                var client = _userManager.Users.Where(x => x.Id == clientId).First();
                allClientsByTrainer.Add(client);
            }

            return View(allClientsByTrainer);
        }


        [Authorize(Roles = "trainer")]
        public IActionResult ClientSubscription(string clientId)
        {
            if (clientId != null)
            {
                var clientSubscriptionsModels = _fitMeService.GetUserSubscriptions(clientId);
                List<UserSubscriptionViewModel> userSubscViewModels = new List<UserSubscriptionViewModel>();
                foreach (var modelItem in clientSubscriptionsModels)
                {
                    userSubscViewModels.Add(_mapper.MappUserSubscriptionModelToViewModel(modelItem));
                }
                return View(userSubscViewModels);
            }
            return RedirectToAction("ClientsList");
        }



    }
}
