﻿using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class GymWorkHoursEntity: GymWorkHoursEntityBase
    {       

        public virtual GymEntity Gym { get; set; }
    }
}
