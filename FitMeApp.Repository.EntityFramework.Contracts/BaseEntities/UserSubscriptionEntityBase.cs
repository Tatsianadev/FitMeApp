using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class UserSubscriptionEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GymSubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
