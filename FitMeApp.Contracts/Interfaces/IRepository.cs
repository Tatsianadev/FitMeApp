using FitMeApp.Contracts.BaseEntities;
using System.Collections.Generic;


namespace FitMeApp.Contracts.Interfaces
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

        //TrainersGym
        IEnumerable<TrainerEntityBase> GetTrainersOfGym(int gymId);
        IEnumerable<GymEntityBase> GetGymsOfTrainer(int trainerId);
        IEnumerable<GroupClassEntityBase> GetGroupClassesOfTrainer(int trainerId);
    }
}
