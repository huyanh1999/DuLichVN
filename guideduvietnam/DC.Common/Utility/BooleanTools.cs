using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Common.Utility
{
    public static class BooleanTools
    {
        public static bool CFToBoolean(string str)
        {
            bool value = false;

            if(bool.TryParse(str, out value))
            {
                value = Convert.ToBoolean(value);
            }
            else
            {
                decimal dec = 0;
                if (decimal.TryParse(str, out dec))
                {
                    value = Convert.ToDecimal(str) > 0 ? true : false;
                }
            }

            return value;
        }
    }
}
