using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    public static class Extensions
    {
        public static bool StartsWithAny(this string str, params string[] values)
        {
            return values.Any(v => str.StartsWith(v));
        }
    }
}
