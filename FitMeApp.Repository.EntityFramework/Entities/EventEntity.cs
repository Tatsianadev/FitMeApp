using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Events")]
    public class EventEntity:EventEntityBase
    {
        public virtual TrainerEntity Trainer { get; set; }

    }
}
