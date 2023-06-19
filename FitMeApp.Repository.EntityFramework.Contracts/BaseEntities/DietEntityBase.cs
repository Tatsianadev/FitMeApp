﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class DietEntityBase
    {
        public int Id { get; set; }
        public int AnthropometricInfoId { get; set; }
        public int CurrentCalorieIntake { get; set; }
        public int DietGoalId { get; set; }
        public int RequiredCalorieIntake { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
    }
}
