using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainerApplicationEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SubscriptionId { get; set; }
        public string ContractNumber { get; set; }
        public int GymId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApplyingDate { get; set; }
    }
}
