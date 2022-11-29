using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class EventWithNamesBase
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string TrainerId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public string Status { get; set; }
    }
}
