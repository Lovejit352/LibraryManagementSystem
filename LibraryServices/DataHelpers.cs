using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServices
{
    public class DataHelpers
    {
        public static IEnumerable<string> HumanizeBusinessHours(IEnumerable<BranchHours> hours)
        {
            var h = new List<string>();

            foreach(var time in hours)
            {
                var day = HumanizeDay(time.DayOfWeek);
                var openTime = HumanizeTime(time.OpenTime);
                var closeTime = HumanizeTime(time.CloseTime);

                var timeEntry = $"{day} {openTime} to {closeTime}";
                h.Add(timeEntry);
            }

            return h;
        }

        private static object HumanizeTime(int time)
        {
            return TimeSpan.FromHours(time).ToString("hh' : 'mm");
        }

        private static object HumanizeDay(int dayOfWeek)
        {
            return Enum.GetName(typeof(DayOfWeek), dayOfWeek-1);
        }
    }
}
