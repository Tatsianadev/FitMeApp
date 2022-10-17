using Microsoft.AspNetCore.Identity;
using System;

namespace FitMeApp.Contracts
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
        public string Gender { get; set; }       
    }
}
