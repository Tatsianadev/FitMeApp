using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Models.ExcelModels;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace FitMeApp.Controllers
{
    public sealed class ChartController : Controller
    {
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
   
        public ChartController(IGymService gymService, ITrainerService trainerService, IFileService fileService, UserManager<User> userManager, ILogger<ChartController> logger)
        {
            _gymService = gymService;
            _trainerService = trainerService;
            _fileService = fileService;
            _userManager = userManager;
            _logger = logger;
        }


        public IActionResult AttendanceChart(int gymId)
        {
            try
            {
                var gymViewModel = new GymViewModel(){Id = gymId};
                return View(gymViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return RedirectToAction("CurrentGymInfo", "Gyms", new { gymId = gymId });
        }


        [Authorize(Roles = "gymAdmin")]
        public async Task<IActionResult> LoadAttendanceChartData()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                int gymId = _trainerService.GetGymIdByTrainer(user.Id);
                var gym = _gymService.GetGymModel(gymId);

                AttendanceChartExcelFileViewModel model = new AttendanceChartExcelFileViewModel()
                {
                    GymId = gymId,
                    GymName = gym.Name
                };

                ViewBag.FileUploaded = false;
                return View("LoadAttendanceChartData", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "This option is not available now.";
                return View("CustomError", message);
            }
        }


        public IActionResult DownloadAttendanceChartBlank()
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
            string message = "This option is not available now. Please, try again later.";
            return View("CustomError", message);
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
                        await fileStream.ReadAsync(buffer, 0, (int)model.AttendanceChartFile.Length);
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
