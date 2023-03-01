using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
    public class TrainerApplicationWithNamesBase: TrainerApplicationEntityBase
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
