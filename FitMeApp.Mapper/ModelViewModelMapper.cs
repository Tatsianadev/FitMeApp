using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Mapper
{
    public class ModelViewModelMapper
    {
        private GymViewModel ConvertGymModelToViewModelBase(GymModel gymModel)
        {
            GymViewModel gymViewModel = new GymViewModel()
            {
                Id = gymModel.Id,
                Name = gymModel.Name,
                Address = gymModel.Address,
                Phone = gymModel.Phone
            };
            return gymViewModel;
        }

        private TrainerViewModel ConvertTrainerModelToViewModelBase(TrainerModel trainerModel)
        {
            TrainerViewModel trainerViewModel = new TrainerViewModel()
            {
                Id = trainerModel.Id,
                FirstName = trainerModel.FirstName,
                LastName = trainerModel.LastName,
                Gender = trainerModel.Gender,
                Picture = trainerModel.Picture,
                Specialization = trainerModel.Specialization
            };
            return trainerViewModel;
        }

        private GroupClassViewModel ConvertGroupClassModelToViewModelBase(GroupClassModel groupClassModel)
        {
            GroupClassViewModel groupClassViewModel = new GroupClassViewModel()
            {
                Id = groupClassModel.Id,
                Name = groupClassModel.Name,
                Description = groupClassModel.Description
            };
            return groupClassViewModel;
        }


        public GymViewModel MappGymModelToViewModel(GymModel gymModel)
        {
            try
            {
                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainer in gymModel.TrainerStaff)
                {
                    var trainerViewModel = ConvertTrainerModelToViewModelBase(trainer);
                    trainerViewModels.Add(trainerViewModel);
                }

                List<GroupClassViewModel> groupClassViewModels = new List<GroupClassViewModel>();
                foreach (var groupClass in gymModel.GroupClasses)
                {
                    var groupClassViewModel = ConvertGroupClassModelToViewModelBase(groupClass);
                    groupClassViewModels.Add(groupClassViewModel);
                }

                GymViewModel gymViewModel = ConvertGymModelToViewModelBase(gymModel);
                gymViewModel.TrainerStaff = trainerViewModels;
                gymViewModel.GroupClasses = groupClassViewModels;

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
                GymViewModel gymViewModel = ConvertGymModelToViewModelBase(trainerModel.Gym);               

                List<GroupClassViewModel> groupClassViewModels = new List<GroupClassViewModel>();
                foreach (var groupClass in trainerModel.GroupClasses)
                {
                    var groupClassViewModel = ConvertGroupClassModelToViewModelBase(groupClass);
                    groupClassViewModels.Add(groupClassViewModel);
                }

                TrainerViewModel trainerViewModel = ConvertTrainerModelToViewModelBase(trainerModel);
                trainerViewModel.Gym = gymViewModel;
                trainerViewModel.GroupClasses = groupClassViewModels;

                return trainerViewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }


        public GroupClassViewModel MappGroupClassModelToViewModel(GroupClassModel groupClassModel)
        {
            try
            {
                List<GymViewModel> gymViewModels = new List<GymViewModel>();
                foreach (var gymModel in groupClassModel.Gyms)
                {
                    var gymViewModel = ConvertGymModelToViewModelBase(gymModel);
                    gymViewModels.Add(gymViewModel);
                }

                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainer in groupClassModel.Trainers)
                {
                    var trainerViewModel = ConvertTrainerModelToViewModelBase(trainer);
                    trainerViewModels.Add(trainerViewModel);
                }

                GroupClassViewModel groupClassViewModel = ConvertGroupClassModelToViewModelBase(groupClassModel);
                groupClassViewModel.Gyms = gymViewModels;
                groupClassViewModel.Trainers = trainerViewModels;

                return groupClassViewModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
