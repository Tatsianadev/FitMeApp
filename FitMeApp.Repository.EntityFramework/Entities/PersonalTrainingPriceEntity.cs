using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("PersonalTrainingPrice")]
    public class PersonalTrainingPriceEntity: PersonalTrainingPriceEntityBase
    {
        public virtual TrainerEntity Trainer { get; set; }
    }
}
