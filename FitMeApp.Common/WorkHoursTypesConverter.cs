using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Common
{
    public static class WorkHoursTypesConverter
    {
        public static string ConvertIntTimeToString(int intTime)
        {
            string stringTime;


            if (intTime % 60 == 0)
            {
                stringTime = (intTime / 60).ToString() + ".00";
            }
            else
            {
                string remainder = (intTime % 60).ToString();
                stringTime = Math.Truncate((decimal)intTime / 60).ToString() + remainder;
            }

            return stringTime;
        }

        public static int ConvertStringTimeToInt(string stringTime)
        {
            //определение целой части
            string integerPart = stringTime.Substring(0, stringTime.Length - 3);

            //oпределение дробной части
            int pointIndex = stringTime.Length - 3;
            string remainder = stringTime.Remove(0, pointIndex+1);

            int intTime = int.Parse(integerPart) * 60 + int.Parse(remainder);
            return intTime;
        }
    }
}
