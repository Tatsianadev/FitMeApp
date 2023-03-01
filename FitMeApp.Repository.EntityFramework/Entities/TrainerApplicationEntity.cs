using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("TrainerApplications")]
    public class TrainerApplicationEntity: TrainerApplicationEntityBase
    {
        public virtual User User { get; set; }
    }
}
