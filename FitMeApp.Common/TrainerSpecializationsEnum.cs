using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitMeApp.Common
{
    public enum TrainerSpecializationsEnum
    {
        [Description("Personal training")]
        personal,
        [Description("Group training")]
        group,
        [Description("Universal")]
        universal
    }
}
