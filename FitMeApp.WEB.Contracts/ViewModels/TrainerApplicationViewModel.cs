using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class TrainerApplicationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public bool TrainerSubscription { get; set; }
        public bool Contract { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
