using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.ApplicationAPI
{
    public partial class TimeSpan
    {
        public override string ToString()
        {
            return $"{(int)TotalSeconds / 60 / 60}h {(int)TotalSeconds / 60 % 60}m {(int)TotalSeconds % 60}s";
        }
    }
}
