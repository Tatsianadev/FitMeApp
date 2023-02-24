using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class AvatarsTestViewModel
    {
        public int Id { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
