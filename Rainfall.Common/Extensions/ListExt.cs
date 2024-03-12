using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Extensions
{
    public static class ListExt
    {
        public static bool HasRecord<T>(this List<T> s)
        {
            if (s != null
                && s.Any()
                && s.Count() >= 1)
            {
                return true;
            }

            return false;
        }
    }
}
