using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class SetCroupClassScheduleViewModel
    {
        public string TrainerId { get; set; }
        public List<DayOfWeek> WorkDaysOfWeek { get; set; }
        public List<DayOfWeek> SelectedDaysOfWeek { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public List<TrainingModel> GroupClasses { get; set; }
        [Required]
        public int SelectedGroupClassId { get; set; }
        public string SelectedGroupClassName { get; set; }
        [Required]
        public List<DateTime> Dates { get; set; }
        [Required]
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int DurationInMinutes { get; set; }
        [Required]
        [Range(1,20)]
        public int ParticipantsLimit { get; set; }

    }
}
