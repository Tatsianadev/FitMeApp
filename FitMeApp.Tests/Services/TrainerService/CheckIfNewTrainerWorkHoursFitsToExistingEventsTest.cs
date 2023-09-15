using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Repository.EntityFramework.Entities;
using FitMeApp.Services.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace FitMeApp.Tests.Services.TrainerService
{
    [TestFixture]
    class CheckIfNewTrainerWorkHoursFitsToExistingEventsTest
    {

        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToExistingEvents_FitData_ReturnsTrue()
        {
            var data = FitNewWorkHoursData();

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.GetActualEventsByTrainer(null)).Returns(GetActualEventsByTrainerFakeMethod());
            var serviceMock = new FitMeApp.Services.TrainerService(repositoryMock.Object);
            var targetMethod = typeof(FitMeApp.Services.TrainerService).GetMethod(
                "CheckIfNewTrainerWorkHoursFitsToExistingEvents", BindingFlags.NonPublic | BindingFlags.Instance);

            if (targetMethod != null)
            {
                foreach (var item in data)
                {
                    var result = targetMethod.Invoke(serviceMock, new object[] { item });
                    Assert.AreEqual(true, result);
                }
            }
        }


        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToExistingEvents_NoEvents_ReturnsTrue()
        {
            var data = FitNewWorkHoursData();

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.GetActualEventsByTrainer(null)).Returns(new List<EventEntityBase>());
            var serviceMock = new FitMeApp.Services.TrainerService(repositoryMock.Object);
            var targetMethod = typeof(FitMeApp.Services.TrainerService).GetMethod(
                "CheckIfNewTrainerWorkHoursFitsToExistingEvents", BindingFlags.NonPublic | BindingFlags.Instance);

            if (targetMethod != null)
            {
                foreach (var item in data)
                {
                    var result = targetMethod.Invoke(serviceMock, new object[] { item });
                    Assert.AreEqual(true, result);
                }
            }
        }

        [Test]
        public void CheckIfNewTrainerWorkHoursFitsToExistingEvents_NotFitData_ReturnsFalse()
        {
            var data = NotFitNewWorkHoursData();

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.GetActualEventsByTrainer(null)).Returns(GetActualEventsByTrainerFakeMethod());
            var serviceMock = new FitMeApp.Services.TrainerService(repositoryMock.Object);
            var targetMethod = typeof(FitMeApp.Services.TrainerService).GetMethod(
                "CheckIfNewTrainerWorkHoursFitsToExistingEvents", BindingFlags.NonPublic | BindingFlags.Instance);

            if (targetMethod != null)
            {
                foreach (var item in data)
                {
                    var result = targetMethod.Invoke(serviceMock, new object[] { item });
                    Assert.AreEqual(false, result);
                }
            }
        }


        private List<EventEntityBase> GetActualEventsByTrainerFakeMethod()
        {
            var output = new List<EventEntityBase>()
            {
                new EventEntity()
                {
                    Date = DateTime.Today,
                    StartTime = 800,
                    EndTime = 860
                },
                new EventEntity()
                {
                    Date = DateTime.Today.AddDays(1),
                    StartTime = 800,
                    EndTime = 860
                }
            };
            return output;
        }

        private List<List<TrainerWorkHoursModel>> FitNewWorkHoursData()
        {
            var data = new List<List<TrainerWorkHoursModel>>();
            var newWorkHours_1 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 680, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 800 , EndTime = 860,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };
            
            var newWorkHours_2 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(2).DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(3).DayOfWeek
                }
            };

            data.Add(newWorkHours_1);
            data.Add(newWorkHours_2);

            return data;
        }

        private List<List<TrainerWorkHoursModel>> NotFitNewWorkHoursData()
        {
            var data = new List<List<TrainerWorkHoursModel>>();
            var newWorkHours_1 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 860, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 480 , EndTime = 800,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            var newWorkHours_2 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(2).DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(3).DayOfWeek
                }
            };

            var newWorkHours_3 = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(2).DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(3).DayOfWeek
                }
            };

            data.Add(newWorkHours_1);
            data.Add(newWorkHours_2);
            data.Add(newWorkHours_3);

            return data;
        }
    }
}
