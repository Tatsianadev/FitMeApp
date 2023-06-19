using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class AnthropometricInfoEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Gender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public int PhysicalActivity { get; set; }
        public DateTime Date { get; set; }
    }
}
