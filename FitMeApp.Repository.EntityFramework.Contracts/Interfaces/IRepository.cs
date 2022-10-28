using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;


namespace FitMeApp.Repository.EntityFramework.Contracts.Interfaces
{
    public interface IRepository
    {
        //Gym
        IEnumerable<GymEntityBase> GetAllGyms();
        GymEntityBase GetGym(int id);
        GymEntityBase AddGym(GymEntityBase item);
        bool UpdateGym(int id, GymEntityBase newGymData);
        bool DeleteGym(int id);

        //Trainers
        IEnumerable<TrainerEntityBase> GetAllTrainers();
        TrainerEntityBase GetTrainer(int id);
        TrainerEntityBase AddTrainer(TrainerEntityBase trainer);
        bool UpdateTrainer(int id, TrainerEntityBase newTrainerData);
        bool DeleteTrainer(int id);

        //GroupClasses
        IEnumerable<GroupClassEntityBase> GetAllGroupClasses();
        GroupClassEntityBase GetGroupClass(int id);
        GroupClassEntityBase AddGroupClass(GroupClassEntityBase item);
        bool UpdateGroupClass(int id, GroupClassEntityBase newGroupClassData);
        bool DeleteGroupClass(int id);


        //Trainers - Gyms - GroupClasses 
        IEnumerable<TrainerEntityBase> GetTrainersOfGym(int gymId);
        IEnumerable<GymEntityBase> GetGymsOfTrainer(int trainerId);
        IEnumerable<GroupClassEntityBase> GetGroupClassesOfGym(int gymId);
        IEnumerable<GroupClassEntityBase> GetGroupClassesOfTrainer(int trainerId);
        IEnumerable<TrainerEntityBase> GetTrainersOfGroupClass(int groupClassId);
        IEnumerable<GymEntityBase> GetGymsOfGroupClass(int groupClassId);


    }
}
