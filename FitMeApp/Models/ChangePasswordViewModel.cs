﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }       
        public string Email { get; set; }
       
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
