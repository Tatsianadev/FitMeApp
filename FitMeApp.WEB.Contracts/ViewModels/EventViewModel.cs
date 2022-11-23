using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int TrainerId { get; set; }
        public string UserId { get; set; }
        public int TrainingId { get; set; }
        public string Status { get; set; }
    }
}
