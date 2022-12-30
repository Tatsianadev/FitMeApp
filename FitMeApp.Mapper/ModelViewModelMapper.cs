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




        //Reverse: ViewModel -> Model

        public TrainerModel MappTrainerViewModelToModel(TrainerViewModel trainerViewModel)
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
                Status = trainerViewModel.Status,
                Gym = new GymModel()
                {
                    Id = trainerViewModel.GymId
                    //Name = trainerViewModel.Gym.Name,
                    //Address = trainerViewModel.Gym.Address,
                    //Phone = trainerViewModel.Picture
                },
                Trainings = trainingModels
            };

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
                GymId = trainerViewModel.Gym.Id
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





        //private string ConvertIntTimeToString(int intTime)
        //{
        //    string stringTime;


        //    if (intTime % 60 == 0)
        //    {
        //        stringTime = (intTime / 60).ToString() + ".00";
        //    }
        //    else
        //    {
        //        string remainder = (intTime % 60).ToString();
        //        stringTime = Math.Truncate((decimal)intTime / 60).ToString() + remainder;
        //    }

        //    return stringTime;
        //}

        //public int ConvertStringTimeToInt(string stringTime)
        //{
        //    //определение целой части
        //    string integerPart = stringTime.Substring(0, stringTime.Length - 3);

        //    //oпределение дробной части
        //    int pointIndex = stringTime.Length - 3;            
        //    string remainder = stringTime.Remove(0, pointIndex);

        //    int intTime = int.Parse(integerPart) * 60 + int.Parse(remainder);
        //    return intTime;
        //}





    }
}
