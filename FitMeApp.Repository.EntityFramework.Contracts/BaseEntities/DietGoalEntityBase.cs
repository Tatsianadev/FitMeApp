using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class DietGoalEntityBase
    {
        public int Id { get; set; }
        public string Goal { get; set; }
    }
}
