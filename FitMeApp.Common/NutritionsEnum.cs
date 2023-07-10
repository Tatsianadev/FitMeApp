using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum NutrientsEnum
    {
        [Description("Proteins")]
        proteins,
        [Description("Fats")]
        fats,
        [Description("Carbohydrates")]
        carbohydrates
    }
}
