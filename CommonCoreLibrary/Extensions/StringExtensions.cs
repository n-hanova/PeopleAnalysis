using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCoreLibrary.Extensions
{
    public static class StringExtensions
    {
        public static bool IsContains(this string str, IEnumerable<string> vs) => vs.Any(x => str.Contains(x));
    }
}
