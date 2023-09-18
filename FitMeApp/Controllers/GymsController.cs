using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FitMeApp.Models.ExcelModels;
using System.Resources;
using FitMeApp.Resources;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Localization;

namespace FitMeApp.Controllers
{
    public sealed class GymsController : Controller
    {
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly ITrainingService _trainingService;
        private readonly IFileService _fileService;
        private readonly ModelViewModelMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        
        public GymsController(IGymService gymService,
            ITrainerService trainerService,
            ITrainingService trainingService,
            IFileService fileService,
            UserManager<User> userManager,
            ILogger<GymsController> logger)
        {
            _gymService = gymService;
            _trainerService = trainerService;
            _trainingService = trainingService;
            _fileService = fileService;
            _mapper = new ModelViewModelMapper();
            _userManager = userManager;
            _logger = logger;
        }


        public IActionResult AllGyms()
        {
            try
            {
                var gymModels = _gymService.GetAllGymsWithGalleryModels();
                List<GymViewModel> gyms = new List<GymViewModel>();
                foreach (var gym in gymModels)
                {
                    gyms.Add(_mapper.MapGymModelToViewModelBase(gym));
                }

                var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
                List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                foreach (var training in trainingModels)
                {
                    trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
                }
                ViewBag.Trainings = trainings;

                return View(gyms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Gym page is not available now.";
                return View("CustomError", message);
            }
            
        }


        [HttpPost]
        public IActionResult GymsByFilter(List<int> selectedTrainingsId)
        {
            try
            {
                if (selectedTrainingsId.Count == 0)
                {
                    return RedirectToAction("AllGyms");
                }
                else
                {
                    var selectedGymModels = _gymService.GetGymsByTrainings(selectedTrainingsId);
                    List<GymViewModel> selectedGyms = new List<GymViewModel>();
                    foreach (var selectedGymModel in selectedGymModels)
                    {
                        selectedGyms.Add(_mapper.MapGymModelToViewModelBase(selectedGymModel));
                    }

                    var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
                    List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                    foreach (var training in trainingModels)
                    {
                        trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
                    }
                    ViewBag.Trainings = trainings;

                    return View("AllGyms", selectedGyms);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with using filters. Try again or not use filters, please.";

                return View("CustomError", message);
            }
        }

        public IActionResult CurrentGymInfo(int gymId)
        {
            var gymModel = _gymService.GetGymModel(gymId);
            GymViewModel gym = _mapper.MapGymModelToViewModel(gymModel);
            List<TrainingViewModel> trainings = new List<TrainingViewModel>();
            foreach (var trainer in gym.Trainers)
            {
                foreach (var training in trainer.Trainings)
                {
                    if (!trainings.Select(x => x.Id).ToList().Contains(training.Id) && training.Name != "Personal training")
                    {
                        trainings.Add(training);
                    }
                }
            }

            ViewBag.Trainings = trainings;
            ViewBag.WorkHours = _gymService.GetWorkHoursByGym(gymId);
            return View(gym);
        }



        //Gym settings

        [Authorize(Roles = "gymAdmin")]
        public async Task<IActionResult> LoadAttendanceChartData()
        {
            var user = await _userManager.GetUserAsync(User);
            int gymId = _trainerService.GetGymIdByTrainer(user.Id);
            var gym = _gymService.GetGymModel(gymId);
            string blankLocalPath = Resources.Resources.AttendanceChartBlankPath;

            AttendanceChartExcelFileViewModel model = new AttendanceChartExcelFileViewModel()
            {
                GymId = gymId,
                GymName = gym.Name,
                BlankFullPath = blankLocalPath
            };

            ViewBag.FileUploaded = false;
            return View("LoadAttendanceChartData", model);
        }


        public IActionResult DownloadAttendanceChartBlank() //todo move logic to another Controller
        {
            string relativePath = Resources.Resources.AttendanceChartBlankPath;
            string absPath = Environment.CurrentDirectory + relativePath;

            if (System.IO.File.Exists(absPath))
            {
                var fileStream = System.IO.File.OpenRead(absPath);
                var contentType = "application/vnd.ms-excel";
                var fileName = Path.GetFileName(absPath);

                return File(fileStream, contentType, fileName);
            }

            _logger.LogError($"File {Path.GetFileName(absPath)} is not found by address {absPath}");
            return RedirectToAction("LoadAttendanceChartData");
        }



        [HttpPost]
        [Authorize(Roles = "gymAdmin")]
        public async Task<IActionResult> LoadAttendanceChartFile(AttendanceChartExcelFileViewModel model)
        {
            try
            {
                if (model.AttendanceChartFile != null && Path.GetExtension(model.AttendanceChartFile.FileName) == ".xlsx")
                {
                    long length = model.AttendanceChartFile.Length;
                    byte[] buffer = new byte[length];
                    using (var fileStream = model.AttendanceChartFile.OpenReadStream())
                    {
                        await fileStream.ReadAsync(buffer, 0, (int) model.AttendanceChartFile.Length);
                    }

                    var output = await _fileService.ReadAttendanceChartFromExcelAsync(buffer, model.GymId);
                    output = output.OrderBy(x => x.DayOfWeek).ToList();
                    foreach (var attendanceChartModel in output)
                    {
                        attendanceChartModel.GymId = model.GymId;
                        attendanceChartModel.NumberOfVisitorsPerHour = attendanceChartModel.NumberOfVisitorsPerHour.OrderBy(x => x.Hour).ToList();
                    }

                    _gymService.AddVisitingChartDataToDb(output);
                    ViewBag.FileUploaded = true;
                }
                else
                {
                    ModelState.AddModelError("File incorrect", "Please, add the file .xlsx extension");
                    ViewBag.FileUploaded = false;
                }

                return View("LoadAttendanceChartData", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with loading the file. Please, try again later";

                return View("CustomError", message);
            }

        }

    }
}
