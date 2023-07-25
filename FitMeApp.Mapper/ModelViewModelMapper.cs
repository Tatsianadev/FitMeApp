using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models.Chart;


namespace FitMeApp.Mapper
{
    public class ModelViewModelMapper
    {
        public GymViewModel MapGymModelToViewModelBase(GymModel gymModel)
        {
            GymViewModel gymViewModel = new GymViewModel()
            {
                Id = gymModel.Id,
                Name = gymModel.Name,
                Address = gymModel.Address,
                Phone = gymModel.Phone,
                GymImagePaths = gymModel.GymImagePaths
            };
            return gymViewModel;
        }

        public TrainerViewModel MapTrainerModelToViewModelBase(TrainerModel trainerModel)
        {
            var trainingViewModels = new List<TrainingViewModel>();
            foreach (var training in trainerModel.Trainings)
            {
                trainingViewModels.Add(MapTrainingModelToViewModelBase(training));
            }


            TrainerViewModel trainerViewModel = new TrainerViewModel()
            {
                Id = trainerModel.Id,
                FirstName = trainerModel.FirstName,
                LastName = trainerModel.LastName,
                Gender = trainerModel.Gender,
                AvatarPath = trainerModel.AvatarPath,
                Specialization = trainerModel.Specialization,
                Trainings = trainingViewModels
            };
            return trainerViewModel;
        }

        public TrainingViewModel MapTrainingModelToViewModelBase(TrainingModel training)
        {
            TrainingViewModel trainingViewModel = new TrainingViewModel()
            {
                Id = training.Id,
                Name = training.Name,
                ShortDescription = training.Description
            };
            return trainingViewModel;
        }


        public GymViewModel MapGymModelToViewModel(GymModel gymModel)
        {
            try
            {
                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainer in gymModel.Trainers)
                {
                    var trainerViewModel = MapTrainerModelToViewModelBase(trainer);
                    trainerViewModels.Add(trainerViewModel);
                }

                GymViewModel gymViewModel = MapGymModelToViewModelBase(gymModel);
                gymViewModel.Trainers = trainerViewModels;

                return gymViewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public TrainerViewModel MapTrainerModelToViewModel(TrainerModel trainerModel)
        {
            GymViewModel gymViewModel = MapGymModelToViewModelBase(trainerModel.Gym);

            List<TrainingViewModel> groupClassViewModels = new List<TrainingViewModel>();
            foreach (var groupClass in trainerModel.Trainings)
            {
                var groupClassViewModel = MapTrainingModelToViewModelBase(groupClass);
                groupClassViewModels.Add(groupClassViewModel);
            }

            TrainerViewModel trainerViewModel = MapTrainerModelToViewModelBase(trainerModel);
            trainerViewModel.Gym = gymViewModel;

            return trainerViewModel;
        }



        public TrainingViewModel MapTrainingModelToViewModel(TrainingModel trainingModel)
        {
            List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
            foreach (var trainer in trainingModel.Trainers)
            {
                var trainerViewModel = MapTrainerModelToViewModel(trainer);
                trainerViewModels.Add(trainerViewModel);
            }

            TrainingViewModel groupClassViewModel = MapTrainingModelToViewModelBase(trainingModel);
            groupClassViewModel.Trainers = trainerViewModels;

            return groupClassViewModel;
        }


        public SubscriptionViewModel MapSubscriptionModelToViewModel(SubscriptionModel subscriptionModel)
        {
            SubscriptionViewModel subscription = new SubscriptionViewModel()
            {
                Id = subscriptionModel.Id,
                GymId = subscriptionModel.GymId,
                ValidDays = subscriptionModel.ValidDays,
                GroupTraining = subscriptionModel.GroupTraining,
                DietMonitoring = subscriptionModel.DietMonitoring,
                WorkAsTrainer = subscriptionModel.WorkAsTrainer,
                Price = subscriptionModel.Price,

            };
            return subscription;
        }


        public EventViewModel MapEventModelToViewModel(EventModel eventModel)
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
                UserFirstName = eventModel.UserFirstName,
                UserLastName = eventModel.UserLastName,
                TrainingId = eventModel.TrainingId,
                TrainingName = eventModel.TrainingName,
                Status = eventModel.Status
            };
            return eventViewModel;
        }


        public GymWorkHoursViewModel MapGymWorkHoursModelToViewModel(GymWorkHoursModel gymWorkHoursModel)
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


        public TrainerWorkHoursViewModel MapTrainerWorkHoursModelToViewModel(TrainerWorkHoursModel trainerWorkHoursModel)
        {
            TrainerWorkHoursViewModel trainerWorkHoursViewModel = new TrainerWorkHoursViewModel()
            {
                Id = trainerWorkHoursModel.Id,
                TrainerId = trainerWorkHoursModel.TrainerId,
                StartTime = Common.WorkHoursTypesConverter.ConvertIntTimeToString(trainerWorkHoursModel.StartTime),
                EndTime = Common.WorkHoursTypesConverter.ConvertIntTimeToString(trainerWorkHoursModel.EndTime),
                GymWorkHoursId = trainerWorkHoursModel.GymWorkHoursId,
                DayName = trainerWorkHoursModel.DayName
            };

            return trainerWorkHoursViewModel;
        }



        public UserSubscriptionViewModel MapUserSubscriptionModelToViewModel(UserSubscriptionModel userSubscriptionModel)
        {
            UserSubscriptionViewModel userSubscriptionViewModel = new UserSubscriptionViewModel()
            {
                Id = userSubscriptionModel.Id,
                UserId = userSubscriptionModel.UserId,
                GymName = userSubscriptionModel.GymName,
                StartDate = userSubscriptionModel.StartDate,
                EndDate = userSubscriptionModel.EndDate,
                GroupTraining = userSubscriptionModel.GroupTraining,
                DietMonitoring = userSubscriptionModel.DietMonitoring,
                WorkAsTrainer = userSubscriptionModel.WorkAsTrainer,
                Price = userSubscriptionModel.Price
            };
            return userSubscriptionViewModel;
        }

        public ChatMessageViewModel MapChatMessageModelToViewModel(ChatMessageModel messageModel)
        {
            ChatMessageViewModel messageViewModel = new ChatMessageViewModel()
            {
                Id = messageModel.Id,
                ReceiverId = messageModel.ReceiverId,
                SenderId = messageModel.SenderId,
                Message = messageModel.Message,
                Date = messageModel.Date
            };

            return messageViewModel;
        }


        public TrainerApplicationViewModel MapTrainerApplicationModelToViewModel(
            TrainerApplicationModel trainerAppModel)
        {
            TrainerApplicationViewModel trainerAppViewModel = new TrainerApplicationViewModel()
            {
                Id = trainerAppModel.Id,
                UserId = trainerAppModel.UserId,
                UserFirstName = trainerAppModel.UserFirstName,
                UserLastName = trainerAppModel.UserLastName,
                SubscriptionId = trainerAppModel.SubscriptionId,
                ContractNumber = trainerAppModel.ContractNumber,
                GymId = trainerAppModel.GymId,
                StartDate = trainerAppModel.StartDate,
                EndDate = trainerAppModel.EndDate,
                ApplyingDate = trainerAppModel.ApplyingDate
            };
            return trainerAppViewModel;
        }


        public HomeTrainingViewModel MapHomeTrainingModelToViewModel(HomeTrainingModel model)
        {
            var viewModel = new HomeTrainingViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                AgeUpTo = model.AgeUpTo,
                AverageCalConsumption = model.AverageCalConsumption,
                BMIfrom = model.BMIfrom,
                BMIupTo = model.BMIupTo,
                Duration = model.Duration,
                EquipmentIsNeeded = model.EquipmentIsNeeded,
                Gender = model.Gender,
                ShortDescriptionFile = model.ShortDescriptionFile,
                ShortDescription = model.ShortDescription
            };

            return viewModel;
        }




        //Reverse: ViewModel -> Model

        public GymModel MapGymViewModelToModel(GymViewModel gymViewModel)
        {
            GymModel gymModel = new GymModel()
            {
                Id = gymViewModel.Id,
                Name = gymViewModel.Name,
                Address = gymViewModel.Address,
                Phone = gymViewModel.Phone
            };
            return gymModel;
        }


        public TrainerModel MapTrainerViewModelToModel(TrainerViewModel trainerViewModel)
        {
            List<TrainingModel> trainingModels = new List<TrainingModel>();

            foreach (var training in trainerViewModel.Trainings)
            {
                trainingModels.Add(new TrainingModel()
                {
                    Id = training.Id
                });
            }

            TrainerModel trainerModel = MapTrainerViewModelToModelBase(trainerViewModel);
            trainerModel.Trainings = trainingModels;

            return trainerModel;
        }
        

        public TrainerModel MapTrainerViewModelToModelBase(TrainerViewModel trainerViewModel)
        {
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainerViewModel.Id,
                FirstName = trainerViewModel.FirstName,
                LastName = trainerViewModel.LastName,
                Gender = trainerViewModel.Gender,
                AvatarPath = trainerViewModel.AvatarPath,
                Specialization = trainerViewModel.Specialization,
                Gym = MapGymViewModelToModel(trainerViewModel.Gym)
            };

            return trainerModel;
        }
        

        public TrainerWorkHoursModel MapTrainerWorkHoursViewModelToModel(TrainerWorkHoursViewModel trainerWorkHoursViewModel)
        {
            TrainerWorkHoursModel trainerWorkHoursModel = new TrainerWorkHoursModel()
            {
                Id = trainerWorkHoursViewModel.Id,
                TrainerId = trainerWorkHoursViewModel.TrainerId,
                StartTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(trainerWorkHoursViewModel.StartTime),
                EndTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(trainerWorkHoursViewModel.EndTime),
                GymWorkHoursId = trainerWorkHoursViewModel.GymWorkHoursId,
                DayName = trainerWorkHoursViewModel.DayName
            };
            return trainerWorkHoursModel;
        }


        public ChatMessageModel MapChatMessageViewModelToModel(ChatMessageViewModel messageViewModel)
        {
            ChatMessageModel messageModel = new ChatMessageModel()
            {
                ReceiverId = messageViewModel.ReceiverId,
                SenderId = messageViewModel.SenderId,
                Message = messageViewModel.Message,
                Date = messageViewModel.Date
            };

            return messageModel;
        }

        
        public EventModel MapEventViewModelToModel(EventViewModel eventViewModel)
        {
            EventModel eventModel = new EventModel()
            {
                Date = eventViewModel.Date,
                StartTime = eventViewModel.StartTime,
                EndTime = eventViewModel.EndTime,
                TrainerId = eventViewModel.TrainerId,
                UserId = eventViewModel.UserId,
                TrainingId = eventViewModel.TrainingId,
                Status = eventViewModel.Status
            };

            return eventModel;
        }

    }



}
