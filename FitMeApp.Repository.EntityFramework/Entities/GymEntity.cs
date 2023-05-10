﻿using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Gyms")]
    public class GymEntity : GymEntityBase
    {
        public GymEntity()
        {
            //Trainers = new HashSet<TrainerEntity>();
            Trainings = new HashSet<TrainingEntity>();
            Subscriptions = new HashSet<SubscriptionEntity>();
            NumberOfVisitorsPerHour = new HashSet<NumberOfVisitorsPerHourEntity>();
            WeeklyGroupTrainingsSchedule = new HashSet<GroupTrainingsScheduleEntity>();
        }


        //public ICollection<TrainerEntity> Trainers { get; set; }
        public ICollection<TrainingEntity> Trainings { get; set; }
        public ICollection<SubscriptionEntity> Subscriptions { get; set; }
        public ICollection<NumberOfVisitorsPerHourEntity> NumberOfVisitorsPerHour { get; set; }
        public ICollection<GroupTrainingsScheduleEntity> WeeklyGroupTrainingsSchedule { get; set; }
    }
}
