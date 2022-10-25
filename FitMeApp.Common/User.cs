using Microsoft.AspNetCore.Identity;
using System;

namespace FitMeApp.Common
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
        public string Gender { get; set; }       
    }
}
