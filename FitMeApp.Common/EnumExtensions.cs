using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace FitMeApp.Common
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumElement)
        {
            Type enumType = enumElement.GetType();
            MemberInfo[] memberInfo = enumType.GetMember(enumElement.ToString());
            
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                
                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute) attributes.ElementAt(0)).Description;
                }
            }

            return enumElement.ToString();
        }
    }
}
