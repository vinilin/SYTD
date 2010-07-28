using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class EncrypeMD5
    {
        public string MD5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] value, hash;
            value = System.Text.Encoding.UTF8.GetBytes(str);
            hash = md.ComputeHash(value);
            md.Clear();
            string temp = "";
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                temp += hash[i].ToString("x").PadLeft(2, '0');
            }
            return temp;
        }
    }
}
