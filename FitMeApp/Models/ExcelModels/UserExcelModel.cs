using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models.ExcelModels
{
    public class UserExcelModel
    {
        public int PositionNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int YearOfBirth { get; set; }
    }
}
