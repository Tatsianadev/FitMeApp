using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class UserSubscriptionEntity: UserSubscriptionEntityBase
    {
        public virtual GymEntity Gym { get; set; }
        public virtual SubscriptionEntity Subscription { get; set; }
    }
}
