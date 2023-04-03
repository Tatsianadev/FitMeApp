using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("NumberOfVisitorsPerHour")]
    public class NumberOfVisitorsPerHourEntity: NumberOfVisitorsPerHourEntityBase
    {
        public virtual GymEntity Gym { get; set; }
    }
}
