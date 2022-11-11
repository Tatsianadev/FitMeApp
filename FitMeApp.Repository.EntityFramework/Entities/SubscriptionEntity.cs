using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table ("Subscriptions")]
    public class SubscriptionEntity:SubscriptionEntityBase
    {
        public SubscriptionEntity()
        {
            this.Gyms = new HashSet<GymEntity>();
        }
        public ICollection<GymEntity> Gyms { get; set; }
    }
}
