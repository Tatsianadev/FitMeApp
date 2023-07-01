using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Controllers;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FitMeApp.Tests
{
    [TestFixture]
    class SplitTextTest
    {
        [Test]
        public async Task SplitTextResultTest()
        {
            var testData = new Dictionary<string, List<string>>()
            {
                {"methods, properties, events, indexers", new List<string>(){"methods", "properties", "events", "indexers"}},
                {"methods, properties, events, indexers,", new List<string>(){"methods", "properties",  "events", "indexers"}},
                {"methods properties events indexers", new List<string>(){"methods properties events indexers"}},
                {"methods,     properties, events,   indexers,   ", new List<string>(){"methods", "properties",  "events", "indexers"}},
                {",,,", new List<string>()},
                {"", new List<string>()},
                {"1,2,3,4", new List<string>(){"1", "2", "3","4"}}
            };

            var userStore = new Mock<IUserStore<User>>();
            userStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    Id = "123testUser",
                    FirstName = "TestUser"
                });

            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var dietServiceMock = new Mock<IDietService>();
            var fileServiceMock = new Mock<IFileService>();
            var loggerMock = new Mock<ILogger<DietController>>();

            var dietController = new DietController(dietServiceMock.Object, fileServiceMock.Object, userManager, loggerMock.Object);
            var splitTextMethod =
                typeof(DietController).GetMethod("SplitText", BindingFlags.NonPublic | BindingFlags.Instance);

            if (splitTextMethod != null)
            {
                foreach (var stringElement in testData)
                {
                    var result = splitTextMethod.Invoke(dietController, new object[] { stringElement.Key, ',' });
                    Assert.AreEqual(stringElement.Value, result);
                }
            }
        }
    }
}
