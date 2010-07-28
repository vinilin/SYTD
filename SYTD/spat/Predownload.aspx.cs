using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Predownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SM.MasterControl mc = new SM.MasterControl();
            mc.Url = new Common.SysConfig().GetValueByKey("SM.MasterControl");
            string strId = Request.QueryString["ID"];
            if (!string.IsNullOrEmpty(strId))
            {
                SM.FileSet fs = mc.QueryFileSetOnSrc(Convert.ToUInt32(strId));
                IDictionary<string, string> list = new Dictionary<string, string>();
                for (int i = 0; i < fs.File.Length; ++i)
                {
                    string url = new Common.SysConfig().GetValueByKey("GetDownLoadURL");
                    url += "/" + fs.Path + fs.File[i].FileName;
                    list.Add(fs.File[i].ID.ToString(),"<a href=\""+url+"\">"+url+"</a>");
                                             
                }

                Repeater1.DataSource = list;

                Repeater1.DataBind();               
                
            }

        }
    }
}
