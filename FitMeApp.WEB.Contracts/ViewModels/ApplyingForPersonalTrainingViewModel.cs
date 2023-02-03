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
        public GymViewModel Gym { get; set; }
        public DateTime TrainingDateTime { get; set; }
        public int Price { get; set; }
    }
}
