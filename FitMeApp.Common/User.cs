﻿using Microsoft.AspNetCore.Identity;
using System;

namespace FitMeApp.Common
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }
    }
}
