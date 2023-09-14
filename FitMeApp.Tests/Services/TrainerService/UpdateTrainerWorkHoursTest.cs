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
    class UpdateTrainerWorkHoursTest
    {
       
        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToGymScheduleTest()
        {
            var data = FitsToGymScheduleTestData();

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
                    var result = targetMethod.Invoke(serviceMock, new object[] { 1, item.Key });
                    Assert.AreEqual(item.Value, result);
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

        private Dictionary<List<TrainerWorkHoursModel>, bool> FitsToGymScheduleTestData()
        {
            var data = new Dictionary<List<TrainerWorkHoursModel>, bool>();
            var trueResultData_1 = new List<TrainerWorkHoursModel>()
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

            var trueResultData_2 = new List<TrainerWorkHoursModel>()
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

            var falseResultData_1 = new List<TrainerWorkHoursModel>()
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
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 800, EndTime = 1000,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var falseResultData_2 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 1000,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };

            var falseResultData_3 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 1500,
                    DayName = DayOfWeek.Friday,
                    GymWorkHoursId = 21
                }
            };
            
            data.Add(trueResultData_1, true);
            data.Add(trueResultData_2, true);
            data.Add(falseResultData_1, false);
            data.Add(falseResultData_2, false);
            data.Add(falseResultData_3, false);

            return data;
        }
    }
}
