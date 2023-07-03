using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum PhysicalActivityEnum
    {
        [Description("extremely inactive")]
        extremelyInactive = 1,
        [Description("sedentary")]
        sedentary = 2,
        [Description("moderately active")]
        moderatelyActive = 3,
        [Description("vigorously active")]
        vigorouslyActive = 4,
        [Description("extremely active")]
        extremelyActive = 5
    }
}
