using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class TrainerWorkLicenseModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int SubscriptionId { get; set; }
        public string ContractNumber { get; set; }
        public int GymId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ConfirmationDate { get; set; }

    }
}
