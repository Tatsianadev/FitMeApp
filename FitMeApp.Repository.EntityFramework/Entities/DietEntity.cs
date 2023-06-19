using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Diets")]
    public class DietEntity: DietEntityBase
    {
        public virtual AnthropometricInfoEntity AnthropometricInfo { get; set; }
        public virtual DietGoalEntity DietGoal { get; set; }
    }
}
