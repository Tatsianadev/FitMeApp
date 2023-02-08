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
                Status = trainerModel.Status,
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
                //trainerViewModel.Trainings = groupClassViewModels;
               

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
                //UserName = eventViewModel.UserName,
                UserFirstName = eventModel.UserFirstName,
                UserLastName = eventModel.UserLastName,
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

        public TrainerWorkHoursViewModel MappTrainerWorkHoursModelToViewModel(TrainerWorkHoursModel trainerWorkHoursModel)
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

       

        public UserSubscriptionViewModel MappUserSubscriptionModelToViewModel(UserSubscriptionModel userSubscriptionModel)
        {
            UserSubscriptionViewModel userSubscriptionViewModel = new UserSubscriptionViewModel()
            {
                Id = userSubscriptionModel.Id,
                UserId = userSubscriptionModel.UserId,
                GymSubscriptionId = userSubscriptionModel.GymSubscriptionId,
                TrainerId = userSubscriptionModel.TrainerId,
                StartDate = userSubscriptionModel.StartDate,
                EndDate = userSubscriptionModel.EndDate,
                GroupTraining = userSubscriptionModel.GroupTraining,
                DietMonitoring = userSubscriptionModel.DietMonitoring
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




        //Reverse: ViewModel -> Model

        public GymModel MappGymViewModelToModel(GymViewModel gymViewModel)
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


        public TrainerModel MappTrainerViewModelToModel(TrainerViewModel trainerViewModel)
        {
            List<TrainingModel> trainingModels = new List<TrainingModel>();

            foreach (var training in trainerViewModel.Trainings)
            {
                trainingModels.Add(new TrainingModel()
                {
                    Id = training.Id
                });
            }

            TrainerModel trainerModel = MappTrainerViewModelToModelBase(trainerViewModel);
            trainerModel.Trainings = trainingModels;          

            return trainerModel;
        }



        public TrainerModel MappTrainerViewModelToModelBase(TrainerViewModel trainerViewModel)
        {
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainerViewModel.Id,
                FirstName = trainerViewModel.FirstName,
                LastName = trainerViewModel.LastName,
                Gender = trainerViewModel.Gender,
                Picture = trainerViewModel.Picture,
                Specialization = trainerViewModel.Specialization,
                Status = trainerViewModel.Status,                
                Gym = MappGymViewModelToModel(trainerViewModel.Gym)
            };

            return trainerModel;
        }





        public TrainerWorkHoursModel MappTrainerWorkHoursViewModelToModel(TrainerWorkHoursViewModel trainerWorkHoursViewModel)
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
