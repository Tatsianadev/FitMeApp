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
        IEnumerable<ClassEntityBase> GetAllGroupClasses();
        ClassEntityBase GetGroupClass(int id);
        ClassEntityBase AddGroupClass(ClassEntityBase item);
        bool UpdateGroupClass(int id, ClassEntityBase newGroupClassData);
        bool DeleteGroupClass(int id);

        //Gym - Trainer - GroupClass connection
        GymWithTrainersAndGroupBase GetGymWithStaffAndGroup(int gymId);
        TrainerWithGymAndGroupBase GetTrainerWithGymAndGroup(int trainerId);
        GroupClassWithGymsAndTrainersBase GetGroupClassWithGymsAndTrainers(int groupClassId);

        //Filters
        IEnumerable<GymWithTrainersAndGroupBase> GetGymsOfGroupClasses(List<int> groupClassesId);


    }
}
