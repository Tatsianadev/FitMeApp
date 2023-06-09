using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FitMeApp.Tests
{
    [TestFixture]
    public class TrainingsEnumTests
    {
        [Test]
        public void EnumConsistencyTest()
        {
            var trainings = CreateTestData();
            var trainingsEnum = Enum.GetValues(typeof(Common.TrainingsEnum));
            bool consists = false;

            if (trainings.Count != trainingsEnum.Length)
            {
                consists = false;
            }
            else
            {
                foreach (var value in trainingsEnum)
                {
                    if (!trainings.ContainsKey((int)value))
                    {
                        consists = false;
                        break;
                    }
                    else
                    {
                        string trainingNameToCompare = trainings[(int)value].ToLower().Replace(" ", "");
                        if (trainingNameToCompare != value.ToString())
                        {
                            consists = false;
                            break;
                        }

                        consists = true;
                    }
                }
            }

            Assert.IsTrue(consists);
        }


        private Dictionary<int, string> CreateTestData()
        {
            var data = new Dictionary<int, string>()
            {
                {1, "Yoga"},
                {2, "Pilates"},
                {3, "HIIT"},
                {4, "Water Aerobics"},
                {5, "Cycling"},
                {6, "Zumba"},
                {7, "Kickboxing"},
                {8, "Personal training"},
            };

            return data;
        }
    }
}
