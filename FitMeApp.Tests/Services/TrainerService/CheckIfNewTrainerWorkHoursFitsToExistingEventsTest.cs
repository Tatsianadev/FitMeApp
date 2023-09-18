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
            var data = new List<List<TrainerWorkHoursModel>>()
            {
                EventsInNewWorkHoursRange(),
                EventsOnStartWorkHoursLimit(),
                EventsOnEndWorkHoursLimit(),
                EventsOnNewWorkHoursLimits()
            } ;

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
            var data = new List<List<TrainerWorkHoursModel>>()
            {
                EventsInNewWorkHoursRange(),
                EventsOnStartWorkHoursLimit(),
                EventsOnEndWorkHoursLimit(),
                EventsOnNewWorkHoursLimits()
            };

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
            var data = new List<List<TrainerWorkHoursModel>>()
            {
               EventsOutOfWorkHoursRange(),
               EndWorkHoursDuringEvents(),
               StartWorkHoursDuringEvents(),
               EventsEndOnStartWorkHoursLimit(),
               EventsStartOnEndWorkHoursLimit(),
               OneEventOnDayOff(),
               TwoEventsOnDayOff()
            };

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

        //Fit data
        private List<TrainerWorkHoursModel> EventsInNewWorkHoursRange()
        {
            var data = new List<TrainerWorkHoursModel>()
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
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> EventsOnNewWorkHoursLimits()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 800, EndTime = 860,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 800 , EndTime = 860,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> EventsOnStartWorkHoursLimit()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 800, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 800 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }
        
        private List<TrainerWorkHoursModel> EventsOnEndWorkHoursLimit()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600, EndTime = 860,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 860,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        //Not fit data
        private List<TrainerWorkHoursModel> EventsOutOfWorkHoursRange()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 900, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 500 , EndTime = 600,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }
        
        private List<TrainerWorkHoursModel> EventsEndOnStartWorkHoursLimit()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 860, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 860 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> EventsStartOnEndWorkHoursLimit()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600, EndTime = 800,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 800,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> StartWorkHoursDuringEvents()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 850, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 850 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> EndWorkHoursDuringEvents()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 850,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 400, EndTime = 850,
                    DayName = DateTime.Today.AddDays(1).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> OneEventOnDayOff()
        {
            var data = new List<TrainerWorkHoursModel>()
            {
                new TrainerWorkHoursModel()
                {
                    StartTime = 600, EndTime = 1000,
                    DayName = DateTime.Today.DayOfWeek
                },
                new TrainerWorkHoursModel()
                {
                    StartTime = 600 , EndTime = 1000,
                    DayName = DateTime.Today.AddDays(3).DayOfWeek
                }
            };

            return data;
        }

        private List<TrainerWorkHoursModel> TwoEventsOnDayOff()
        {
            var data = new List<TrainerWorkHoursModel>()
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

            return data;
        }

    }
}
