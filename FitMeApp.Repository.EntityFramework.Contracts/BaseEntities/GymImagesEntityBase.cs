using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GymImagesEntityBase
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public string ImagePath { get; set; }
    }
}
