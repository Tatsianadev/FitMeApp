using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class GroupClassScheduleModel
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public string TrainerId { get; set; }
        public int GroupClassId { get; set; }
        public string GroupClassName { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
    }
}
