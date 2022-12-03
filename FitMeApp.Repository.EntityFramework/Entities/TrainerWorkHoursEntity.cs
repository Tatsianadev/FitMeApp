using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class TrainerWorkHoursEntity: TrainerWorkHoursEntityBase
    {
      
        public GymWorkHoursEntity GymWorkHours { get; set; }
    }
}
