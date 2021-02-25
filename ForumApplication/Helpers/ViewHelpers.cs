using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumApplication.Helpers
{
    public class ViewHelpers
    {
        public static string DateCounter(DateTime date)
        {
            DateTime currentTime = DateTime.Now;
            var result = currentTime - date;
        
            int days = result.Days;
            int hours = result.Hours;
            int min = result.Minutes;
            int sec = result.Seconds;
            int count;

            if (days == 0)
            {
                if (hours == 0)
                {
                    if (min == 0)
                    {
                        return $"{sec} sek. temu";
                    }
                    else
                    {
                        return $"{min} min. temu";
                    }
                }
                else
                {
                    return $"{hours} h temu";
                }
            }
            else
            {
                if (days < 6)
                {
                    return $"{days} dni temu";
                }
                else if (days < 30)
                {
                    count = days / 7;
                    return $"{count} tyg. temu";
                }
                else if (days < 364)
                {
                    count = days / 30;
                    return $"{count} mc temu";
                }
                else
                {
                    count = days / 365;
                    return $"{count} lat temu";
                }
            }
        }



    }
}