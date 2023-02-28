using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GymImages")]
    public class GymImagesEntity: GymImagesEntityBase
    {
        public virtual GymEntity Gym { get; set; }
    }
}
