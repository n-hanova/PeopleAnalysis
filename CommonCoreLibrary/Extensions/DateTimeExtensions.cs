using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.Extensions
{
    public class DateTimeExtensions
    {
        public static int? Convert(string date)
        {
            if (DateTime.TryParse(date, out var res))
                return DateTime.Today.Year - res.Year;
            return null;
        }
    }
}
