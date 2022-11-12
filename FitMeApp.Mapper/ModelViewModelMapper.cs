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
               ValidDays = subscriptionModel.ValidDays,
               GroupTraining = subscriptionModel.GroupTraining,
               DietMonitoring = subscriptionModel.DietMonitoring,
               Price = subscriptionModel.Price,
               //Image = GetImageName(subscriptionModel)
            };
            return subscription;
        }

        //private string GetImageName(SubscriptionModel subscriptionModel)
        //{
        //    var name = "Training";
        //    var fileExtension = ".jpg";
        //    if (subscriptionModel.DietMonitoring)
        //    {
        //        name += nameof(subscriptionModel.DietMonitoring);
        //    }

        //    if (subscriptionModel.GroupTrainingInclude)
        //    {
        //        name += nameof(subscriptionModel.GroupTraining);
        //    }

        //    return name + fileExtension;
        //}
    }
}
