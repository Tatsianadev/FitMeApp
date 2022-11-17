using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class UserSubscriptionEntity: UserSubscriptionEntityBase
    {
        public virtual GymSubscriptionEntity GymSubscription { get; set; }
        //public virtual TrainerEntity Trainer { get; set; }
    }
}
