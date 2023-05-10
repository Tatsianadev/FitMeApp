using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class GroupTrainingsScheduleModel
    {
        //public int Id { get; set; }
        //public int TrainingTrainerId { get; set; }
        //public int GymId { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
    }
}
