using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
    public class EventFullInfoBase : EventEntityBase
    {
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string TrainingName { get; set; }
    }
}
