using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class IpCheck
    {
        public static bool IsIn(string src, string begin, string end)
        {
            if (src == null || begin == null || end == null)
            {
                return false;
            }
            string[] arr = new string[4];
            arr = src.Split('.');
           long IPNUMS = Convert.ToInt64(arr[0]) * 16777216 + Convert.ToInt64(arr[1]) * 65536 + Convert.ToInt64(arr[2]) * 256 + Convert.ToInt64(arr[3]) - 1;
           arr = begin.Split('.');
           long lbeg = Convert.ToInt64(arr[0]) * 16777216 + Convert.ToInt64(arr[1]) * 65536 + Convert.ToInt64(arr[2]) * 256 + Convert.ToInt64(arr[3]) - 1;
           arr = end.Split('.');
           long lend = Convert.ToInt64(arr[0]) * 16777216 + Convert.ToInt64(arr[1]) * 65536 + Convert.ToInt64(arr[2]) * 256 + Convert.ToInt64(arr[3]) - 1;
           return (IPNUMS > lbeg) && (IPNUMS < lend);
        }


    }
}
