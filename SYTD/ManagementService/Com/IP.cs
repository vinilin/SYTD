using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace ManagementService.Com
{
    public class IP
    {
        //----------------------------------------
        //检查IP地址是否有效
        //----------------------------------------
        public bool IpCheck(string Ip)
        {
            string[] ipArray=Ip.Split('.');
            if (ipArray.Length!=4)
            {
                return false;
            }
            IPAddress ip_test;
            return IPAddress.TryParse(Ip, out ip_test);
        }

        //----------------------------------------
        //检查IP是否是迂回地址
        //----------------------------------------
        public bool IpCheckIsLookUp(string Ip)
        {
            return IPAddress.IsLoopback(IPAddress.Parse(Ip));
        }

        //----------------------------------------
        //将IP地址转换成整数
        //----------------------------------------
        public uint IpConvertInt(string Ip)
        {
            IPAddress ip_addr = IPAddress.Parse(Ip);
            uint ip_num = (uint)IPAddress.NetworkToHostOrder((int)(ip_addr.Address));
            return ip_num;
        }
    }
}
