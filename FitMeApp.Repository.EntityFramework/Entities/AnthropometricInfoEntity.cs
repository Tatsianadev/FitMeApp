using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("AnthropometricInfo")]
    public class AnthropometricInfoEntity: AnthropometricInfoEntityBase
    {
        public virtual User User { get; set; }
    }
}
