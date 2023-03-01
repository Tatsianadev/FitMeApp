using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainerApplicationEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool TrainerSubscription { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
