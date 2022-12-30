using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class EditTrainerJobDataModel
    {
        public string Id { get; set; }
        public string Specialization { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public List<int> TrainingsId { get; set; }
        public TrainerConfirmStatusEnum Status { get; set; }
    }
}
