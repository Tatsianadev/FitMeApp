using FitMeApp.WEB.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class TrainerWorkHoursViewComponent
    {
        public string Invoke(int startTime, int endTime)
        {
            string startTimeToPrint;
            string endTimeToPrint;

            if (startTime%60 == 0)
            {
                startTimeToPrint = (startTime / 60).ToString()+".00";
            }
            else
            {
                string remainder = (startTime % 60).ToString();
                startTimeToPrint = Math.Truncate((decimal)startTime / 60).ToString() + remainder;
            }

            if (endTime % 60 == 0)
            {
                endTimeToPrint = (endTime / 60).ToString() + ".00";
            }
            else
            {
                string remainder = (endTime % 60).ToString();
                endTimeToPrint = Math.Truncate((decimal)endTime / 60).ToString() + remainder;
            }

            return $"{startTimeToPrint} - {endTimeToPrint}";
        }
    }
}
