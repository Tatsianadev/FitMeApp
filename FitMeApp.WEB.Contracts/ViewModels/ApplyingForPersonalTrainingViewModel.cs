using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class ApplyingForPersonalTrainingViewModel
    {
        public string TrainerId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        public string GymName { get; set; }
        public string GymAddress { get; set; }
        public List<string> AvailableTime { get; set; }
        public DateTime SelectedDate { get; set; }
        public int SelectedStartTime { get; set; }
        public int SelectedEndTime { get; set; }
        public int Price { get; set; }
    }
}
