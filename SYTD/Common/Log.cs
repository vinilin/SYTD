using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Common
{
    public class Log
    {

        public static void LogError(string errInfo, string kind)
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLog\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string strTempLogFile = path + "Error" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string strErrorTime = System.DateTime.Now.ToString();
                string strInfo = kind + "\r\n" + errInfo + "\r\n\r\n\r\n";
                FileInfo fi = new System.IO.FileInfo(strTempLogFile);
                FileStream objWriter = fi.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                byte[] temp = System.Text.Encoding.Default.GetBytes(strInfo);
                objWriter.Write(temp, 0, temp.Length);
                objWriter.Flush();
                objWriter.Close();
            }
            catch { }
        }

        public static void OpLog(string Info, string kind)
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\OpLog\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string strTempLogFile = path + "" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string strErrorTime = System.DateTime.Now.ToString();
                string strInfo = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + kind + "\r\n" + Info + "\r\n\r\n\r\n";
                FileInfo fi = new System.IO.FileInfo(strTempLogFile);
                FileStream objWriter = fi.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                byte[] temp = System.Text.Encoding.Default.GetBytes(strInfo);
                objWriter.Write(temp, 0, temp.Length);
                objWriter.Flush();
                objWriter.Close();
            }
            catch { }
        }
    }
}
