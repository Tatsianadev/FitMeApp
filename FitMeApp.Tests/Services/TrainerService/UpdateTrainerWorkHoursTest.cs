using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services;
using FitMeApp.Services.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace FitMeApp.Tests.Services.TrainerService
{
    [TestFixture]
    class CheckIfNewTrainerWorkHoursFitsToGymScheduleTest
    {

        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToGymSchedule_FitData_ReturnsTrue()
        {
            var data = FitTestData();

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.GetWorkHoursByGym(1)).Returns(GetWorkHoursByGymFakeMethod());

            var serviceMock = new FitMeApp.Services.TrainerService(repositoryMock.Object);
            var targetMethod = typeof(FitMeApp.Services.TrainerService)
                .GetMethod("CheckIfNewTrainerWorkHoursFitsToGymSchedule",
                    BindingFlags.NonPublic | BindingFlags.Instance);

            if (targetMethod != null)
            {
                foreach (var item in data)
                {
                    var result = targetMethod.Invoke(serviceMock, new object[] { 1, item });
                    Assert.AreEqual(true, result);
                }
            }
        }

        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToGymSchedule_NotFitData_ReturnsFalse()
        {
            var data = NotFitTestData();

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.GetWorkHoursByGym(1)).Returns(GetWorkHoursByGymFakeMethod());

            var serviceMock = new FitMeApp.Services.TrainerService(repositoryMock.Object);
            var targetMethod = typeof(FitMeApp.Services.TrainerService)
                .GetMethod("CheckIfNewTrainerWorkHoursFitsToGymSchedule",
                    BindingFlags.NonPublic | BindingFlags.Instance);

            if (targetMethod != null)
            {
                foreach (var item in data)
                {
                    var result = targetMethod.Invoke(serviceMock, new object[] { 1, item });
                    Assert.AreEqual(false, result);
                }
            }
        }

        

        private IEnumerable<GymWorkHoursEntityBase> GetWorkHoursByGymFakeMethod()
        {
            var output = new List<GymWorkHoursEntityBase>()
            {

                new GymWorkHoursEntityBase()
                {
                    Id = 5,
                    DayOfWeekNumber = DayOfWeek.Monday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                },
                new GymWorkHoursEntityBase()
                {
                    Id = 9,
                    DayOfWeekNumber = DayOfWeek.Tuesday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                },
                new GymWorkHoursEntityBase()
                {
                    Id = 13,
                    DayOfWeekNumber = DayOfWeek.Wednesday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                },
                new GymWorkHoursEntityBase()
                {
                    Id = 17,
                    DayOfWeekNumber = DayOfWeek.Thursday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                },
                new GymWorkHoursEntityBase()
                {
                    Id = 21,
                    DayOfWeekNumber = DayOfWeek.Friday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                },
                new GymWorkHoursEntityBase()
                {
                    Id = 25,
                    DayOfWeekNumber = DayOfWeek.Sunday,
                    GymId = 1,
                    StartTime = 480,
                    EndTime = 1320
                }

            };

            return output;
        }

        private List<List<TrainerWorkHoursModel>> FitTestData()
        {
            var data = new List<List<TrainerWorkHoursModel>>();
            var newWorkHours_1 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 680, EndTime = 1000,
                    DayName = DayOfWeek.Wednesday,
                    GymWorkHoursId = 13
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 800, EndTime = 1000,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var newWorkHours_2 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 680, EndTime = 1320,
                    DayName = DayOfWeek.Wednesday,
                    GymWorkHoursId = 13
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 480, EndTime = 1000,
                    DayName = DayOfWeek.Thursday,
                    GymWorkHoursId = 17
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 480, EndTime = 1320,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var newWorkHours_3 = new List<TrainerWorkHoursModel>()
            {
                
                new TrainerWorkHoursModel()
                {
                    StartTime = 480, EndTime = 1320,
                    DayName = DayOfWeek.Wednesday,
                    GymWorkHoursId = 13
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 480, EndTime = 1320,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };
            
            data.Add(newWorkHours_1);
            data.Add(newWorkHours_2);
            data.Add(newWorkHours_3);
           
            return data;
        }

        private List<List<TrainerWorkHoursModel>> NotFitTestData()
        {
            var data = new List<List<TrainerWorkHoursModel>>();
            var newWorkHours_1 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 1000,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var newWorkHours_2 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 1500,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var newWorkHours_3 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 800, EndTime = 1500,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var newWorkHours_4 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 540, EndTime = 1200,
                    DayName = DayOfWeek.Sunday,
                    GymWorkHoursId = 1
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 680, EndTime = 1000,
                    DayName = DayOfWeek.Wednesday,
                    GymWorkHoursId = 13
                }
            };

            data.Add(newWorkHours_1);
            data.Add(newWorkHours_2);
            data.Add(newWorkHours_3);
            data.Add(newWorkHours_4);

            return data;
        }
    }
}
