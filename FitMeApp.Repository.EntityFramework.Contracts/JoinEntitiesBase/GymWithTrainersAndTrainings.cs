﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
    public class GymWithTrainersAndTrainings
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<string> GymImagePaths { get; set; }
        public IEnumerable<TrainerWithGymAndTrainingsBase> Trainers { get; set; }
       


    }
}
