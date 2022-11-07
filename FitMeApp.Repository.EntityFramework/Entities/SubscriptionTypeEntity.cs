using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class SubscriptionTypeEntity:SubscriptionTypeEntityBase
    {        

        public GymEntity Gym { get; set; }
    }
}
