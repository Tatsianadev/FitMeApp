using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase
{

    //this class created to be able to pass JoinResult as a parameter in Method
    public class TrainerWithGymAndTrainingsJoin
    {
        public string TrainerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }
        public string Specialization { get; set; }
        public TrainerApproveStatusEnum Status { get; set; }

        public int GymId { get; set; }
        public string GymName { get; set; }
        public string GymAddress { get; set; }

        public int TrainingId { get; set; }
        public string TrainingName { get; set; }       
    }
}
