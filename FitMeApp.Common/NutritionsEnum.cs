using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum NutrientsEnum
    {
        [Description("Protein")]
        proteins,
        [Description("Fat")]
        fats,
        [Description("Carbohydrate")]
        carbohydrates
    }
}
