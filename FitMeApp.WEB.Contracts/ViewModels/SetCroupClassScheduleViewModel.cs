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
        public IEnumerable<DayOfWeek> WorkDaysOfWeek { get; set; }
        public IEnumerable<DayOfWeek> SelectedDaysOfWeek { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public IEnumerable<TrainingModel> GroupClasses { get; set; }
        [Required]
        public int SelectedGroupClassId { get; set; }
        [Required]
        public IEnumerable<DateTime> Dates { get; set; }
        [Required]
        public string StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        [Required]
        [Range(1,20)]
        public int ParticipantsLimit { get; set; }

    }
}
