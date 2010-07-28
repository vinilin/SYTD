using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Xml;

namespace Common
{
    public class SysConfig
    {

        /// <summary>
        /// 根据KEY获取系统配置
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private object GetValueByKey(string Key, System.Type type)
        {
            object result = "";
            try
            {
                result = new System.Configuration.AppSettingsReader().GetValue(Key, type);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetValueByKey(string Key)
        {
            string result = "";
            try
            {
                result = new System.Configuration.AppSettingsReader().GetValue(Key, System.Type.GetType("System.String")).ToString();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message, "获取配置文件中的" + Key);
            }
            return result;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnString()
        {
            string result = "";
            try
            {
                result = GetValueByKey("ConnString");
                //if (result.ToUpper().StartsWith("SERVER"))
                //{
                //    UpdateConnStringEncrypt(result);
                //}
                //else
                //{
                //    EncryptDES en = new EncryptDES();
                //    result = en.DecryptString(result);
                //}
            }
            catch (ConfigurationException ex)
            {
                Log.LogError(ex.Message, "获取数据库连接字符串");
            }
            return result;
        }

        public string GetConnString_PB()
        {
            string result = "";
            try
            {
                result = GetValueByKey("ConnString_PB");
                //if (result.ToUpper().StartsWith("SERVER"))
                //{
                //    UpdateConnStringEncrypt(result);
                //}
                //else
                //{
                //    EncryptDES en = new EncryptDES();
                //    result = en.DecryptString(result);
                //}
            }
            catch (ConfigurationException ex)
            {
                Log.LogError(ex.Message, "获取数据库连接字符串");
            }
            return result;
        }

        public string GetVodPlayUrl()
        {
            string result = "";
            try
            {
                result = GetValueByKey("GetVodPlayURL");
            }
            catch { }
            return result;
        }
        public string GetVidioPlayUrl()
        {
            string result = "";
            try
            {
                result = GetValueByKey("GetVidioPlayURL");
            }
            catch { }
            return result;
        }

        public string GetMusicPlayUrl()
        {
            string result = "";
            try
            {
                result = GetValueByKey("GetMusicdPlayURL");
            }
            catch { }
            return result;
        }
        public string GetCartoonPlayUrl()
        {
            string result = "";
            try
            {
                result = GetValueByKey("GetCartoonPlayURL");
            }
            catch { }
            return result;
        }

        public string GetWebServiceLinkPwd()
        {
            string result = "";
            try
            {
                result = GetValueByKey("WebServiceLinkPwd");
                if (!result.ToUpper().StartsWith("9999999999"))
                {
                    UpdatePwdEncrypt(result);
                }
                else
                {
                    EncryptDES en = new EncryptDES();
                    result = en.DecryptString(result);
                }
            }
            catch (ConfigurationException ex)
            {
                Log.LogError(ex.Message, "获取WEBSERVICE连接密码字符串");
            }
            return result;
        }

        private void UpdateConnStringEncrypt(string connString)
        {
            string result = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                //xmlDoc.Load("../web.Config");
                XmlNode configurationNode = xmlDoc.SelectSingleNode("configuration");
                XmlNode appSettingsNode = configurationNode.SelectSingleNode("appSettings");
                XmlNodeList addNodes = appSettingsNode.SelectNodes("add");
                foreach (XmlNode i in addNodes)
                {
                    if (i.Attributes["key"].Value.ToString().ToUpper() == "CONNSTRING")
                    {
                        EncryptDES en = new EncryptDES();
                        result = en.EncryptString(connString);
                        i.Attributes["value"].Value = result;
                        xmlDoc.Save(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "修改配置文件");
            }
        }

        private void UpdatePwdEncrypt(string connString)
        {
            string result = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                //xmlDoc.Load("../web.Config");
                XmlNode configurationNode = xmlDoc.SelectSingleNode("configuration");
                XmlNode appSettingsNode = configurationNode.SelectSingleNode("appSettings");
                XmlNodeList addNodes = appSettingsNode.SelectNodes("add");
                foreach (XmlNode i in addNodes)
                {
                    if (i.Attributes["key"].Value.ToString().ToUpper() == "WEBSERVICELINKPWD")
                    {
                        EncryptDES en = new EncryptDES();
                        result = en.EncryptString(connString);
                        i.Attributes["value"].Value = result;
                        xmlDoc.Save(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "修改配置文件");
            }
        }

        public void SetValueByKey(string key, string value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                XmlNode configurationNode = xmlDoc.SelectSingleNode("configuration");
                XmlNode appSettingsNode = configurationNode.SelectSingleNode("appSettings");
                XmlNodeList addNodes = appSettingsNode.SelectNodes("add");
                foreach (XmlNode i in addNodes)
                {
                    if (i.Attributes["key"].Value.ToString().ToUpper() == key.ToUpper())
                    {
                        i.Attributes["value"].Value = value;
                        xmlDoc.Save(HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "/web.Config");
                        break;
                    }
                }
            }
            catch { }
        }

        public string GetManterControl()
        {
            string result = "";
            try
            {
                result = GetValueByKey("masterControl_WebService");
            }
            catch (ConfigurationException ex)
            {
                Log.LogError(ex.Message, "获取主控WEBSERVICE失败");
            }
            return result;
        }
    }

}
