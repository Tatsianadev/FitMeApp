using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class PersonalTrainingPriceEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int Price { get; set; }
    }
}
