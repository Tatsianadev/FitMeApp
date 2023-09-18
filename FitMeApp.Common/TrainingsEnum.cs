using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum TrainingsEnum
    {
        [Description("Yoga")]
        yoga = 1,
        [Description("Pilates")]
        pilates = 2,
        [Description("HIIT")]
        hiit = 3,
        [Description("Water Aerobics")]
        wateraerobics = 4,
        [Description("Cycling")]
        cycling = 5,
        [Description("Zumba")]
        zumba = 6,
        [Description("Kickboxing")]
        kickboxing = 7,
        [Description("Personal training")]
        personaltraining = 8
    }
}
