using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Mapper
{
    public class ModelViewModelMapper
    {
        public GymViewModel MappGymModelToViewModelBase(GymModel gymModel)
        {
            GymViewModel gymViewModel = new GymViewModel()
            {
                Id = gymModel.Id,
                Name = gymModel.Name,
                Address = gymModel.Address,
                Phone = gymModel.Phone,
                
            };
            return gymViewModel;
        }

        public TrainerViewModel MappTrainerModelToViewModelBase(TrainerModel trainerModel)
        {
            var trainingViewModels = new List<TrainingViewModel>();
            foreach (var training in trainerModel.Trainings)
            {
                trainingViewModels.Add(MappTrainingModelToViewModelBase(training));
            }


            TrainerViewModel trainerViewModel = new TrainerViewModel()
            {
                Id = trainerModel.Id,
                FirstName = trainerModel.FirstName,
                LastName = trainerModel.LastName,
                Gender = trainerModel.Gender,
                Picture = trainerModel.Picture,
                Specialization = trainerModel.Specialization,
                Trainings = trainingViewModels
            };
            return trainerViewModel;
        }

        public TrainingViewModel MappTrainingModelToViewModelBase(TrainingModel training)
        {
            TrainingViewModel trainingViewModel = new TrainingViewModel()
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description
            };
            return trainingViewModel;
        }


        public GymViewModel MappGymModelToViewModel(GymModel gymModel)
        {
            try
            {
                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainer in gymModel.Trainers)
                {
                    var trainerViewModel = MappTrainerModelToViewModelBase(trainer);
                    trainerViewModels.Add(trainerViewModel);
                }

                //List<GroupClassViewModel> groupClassViewModels = new List<GroupClassViewModel>();
                //foreach (var groupClass in gymModel.GroupClasses)
                //{
                //    var groupClassViewModel = ConvertGroupClassModelToViewModelBase(groupClass);
                //    groupClassViewModels.Add(groupClassViewModel);
                //}

                GymViewModel gymViewModel = MappGymModelToViewModelBase(gymModel);
                gymViewModel.Trainers = trainerViewModels;
                

                return gymViewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }           
            
        }


        public TrainerViewModel MappTrainerModelToViewModel(TrainerModel trainerModel)
        {
            try
            {
                GymViewModel gymViewModel = MappGymModelToViewModelBase(trainerModel.Gym);               

                List<TrainingViewModel> groupClassViewModels = new List<TrainingViewModel>();
                foreach (var groupClass in trainerModel.Trainings)
                {
                    var groupClassViewModel = MappTrainingModelToViewModelBase(groupClass);
                    groupClassViewModels.Add(groupClassViewModel);
                }

                TrainerViewModel trainerViewModel = MappTrainerModelToViewModelBase(trainerModel);
                trainerViewModel.Gym = gymViewModel;
                trainerViewModel.Trainings = groupClassViewModels;

                return trainerViewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }


        public TrainerModel MappTrainerModelToBase(TrainerViewModel trainerViewModel)
        {
            List<TrainingModel> trainingModels = new List<TrainingModel>();
            foreach (var training in trainerViewModel.Trainings)
            {
                trainingModels.Add(new TrainingModel()
                {
                    Id = training.Id,
                    Name = training.Name,
                    Description = training.Description
                });
            }

            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainerViewModel.Id,
                FirstName = trainerViewModel.FirstName,
                LastName = trainerViewModel.LastName,
                Gender = trainerViewModel.Gender,
                Picture = trainerViewModel.Picture,
                Specialization = trainerViewModel.Specialization,
                Gym = new GymModel()
                {
                    Id = trainerViewModel.Gym.Id,
                    Name = trainerViewModel.Gym.Name,
                    Address = trainerViewModel.Gym.Address,
                    Phone = trainerViewModel.Picture
                },
                Trainings = trainingModels
            };

            return trainerModel;
        }


        public TrainingViewModel MappTrainingModelToViewModel(TrainingModel trainingModel)
        {
            try
            {
                List<GymViewModel> gymViewModels = new List<GymViewModel>();
                foreach (var gymModel in trainingModel.Gyms)
                {
                    var gymViewModel = MappGymModelToViewModelBase(gymModel);
                    gymViewModels.Add(gymViewModel);
                }

                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainer in trainingModel.Trainers)
                {
                    var trainerViewModel = MappTrainerModelToViewModelBase(trainer);
                    trainerViewModels.Add(trainerViewModel);
                }

                TrainingViewModel groupClassViewModel = MappTrainingModelToViewModelBase(trainingModel);
                groupClassViewModel.Gyms = gymViewModels;
                groupClassViewModel.Trainers = trainerViewModels;

                return groupClassViewModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public SubscriptionViewModel MappSubscriptionModelToViewModel(SubscriptionModel subscriptionModel)
        {
            SubscriptionViewModel subscription = new SubscriptionViewModel()
            {
               Id = subscriptionModel.Id,
               GymId = subscriptionModel.GymId,
               ValidDays = subscriptionModel.ValidDays,
               GroupTraining = subscriptionModel.GroupTraining,
               DietMonitoring = subscriptionModel.DietMonitoring,
               Price = subscriptionModel.Price,
               
            };
            return subscription;
        }


        public EventViewModel MappEventModelToViewModel(EventModel eventModel)
        {
            EventViewModel eventViewModel = new EventViewModel()
            {
                Id = eventModel.Id,
                Date = eventModel.Date,
                StartTime = eventModel.StartTime,
                EndTime = eventModel.EndTime,
                TrainerId = eventModel.TrainerId,
                TrainerFirstName = eventModel.TrainerFirstName,
                TrainerLastName = eventModel.TrainerLastName,
                GymId = eventModel.GymId,
                GymName = eventModel.GymName,
                UserId = eventModel.UserId,
                UserName = eventModel.UserName,
                TrainingId = eventModel.TrainingId,
                TrainingName = eventModel.TrainingName,
                Status = eventModel.Status
            };
            return eventViewModel;
        }


        public GymWorkHoursViewModel MappGymWorkHoursModelToViewModel(GymWorkHoursModel gymWorkHoursModel)
        {
            GymWorkHoursViewModel gymWorkHoursViewModel = new GymWorkHoursViewModel()
            {
                Id = gymWorkHoursModel.Id,
                DayName = gymWorkHoursModel.DayName,
                GymId = gymWorkHoursModel.GymId,
                StartTime = gymWorkHoursModel.StartTime,
                EndTime = gymWorkHoursModel.EndTime
            };
            return gymWorkHoursViewModel;
        }

        public TrainerWorkHoursViewModel MappTrainerWorkHoursWithDaysBaseToViewModel(TrainerWorkHoursModel trainerWorkHoursModel)
        {
            TrainerWorkHoursViewModel trainerWorkHoursViewModel = new TrainerWorkHoursViewModel()
            {
                Id = trainerWorkHoursModel.Id,
                TrainerId = trainerWorkHoursModel.TrainerId,
                StartTime = trainerWorkHoursModel.StartTime,
                EndTime = trainerWorkHoursModel.EndTime,
                GymWorkHoursId = trainerWorkHoursModel.GymWorkHoursId,
                DayName = trainerWorkHoursModel.DayName
            };
            return trainerWorkHoursViewModel;
        }





    }
}
