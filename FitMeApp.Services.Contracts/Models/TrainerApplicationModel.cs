using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class TrainerApplicationModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public bool TrainerSubscription { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
