using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum DietGoalsEnum
    {
        [Description("lose weight")]
        loseWeight = 1,
        [Description("keep weight")]
        keepWeight = 2,
        [Description("put weight on")]
        putWeightOn = 3,
        [Description("put muscles on")]
        putMusclesOn = 4
    }
}
