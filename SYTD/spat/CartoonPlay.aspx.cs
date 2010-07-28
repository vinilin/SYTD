using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class cartoonPlay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMessage.Text = "";
        string Id = "";
        string num = "";
        if (Request.QueryString["Id"] != null)
        {
            Id = Request.QueryString["Id"].ToString();
        }
        if (Request.QueryString["num"] != null)
        {
            num = Request.QueryString["num"].ToString();
        }
        //DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        bind(Id, num);
        //Access.Dispose();
    }

    private void bind(string Id, string num)
    {
        try
        {
            string strSql = "select BaseItem.Title, FileItem.FileSetID from BaseItem ";
            strSql += " left join FileItem on FileItem.id=BaseItem.id ";
            strSql += " where BaseItem.id=" + Id;
            //此处根据ID读出并给页面空间赋值
            DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
            DataTable dt = Access.execSql(strSql);
            
            long fsId = 0;
            string title = "";
            string fileName = "";
            int index = Convert.ToInt32(num);

            if (dt != null)
            {
                if (dt.Rows.Count != 1)
                {
                    return;
                    //throw new ReadOnlyException("未找到该电影");
                }
                fsId = Convert.ToInt64(dt.Rows[0]["FileSetID"].ToString());
                title = dt.Rows[0]["Title"].ToString();
            }
            Access.Dispose();

            // 查询文件集
            FSM.FileSetMan fsm = new FSM.FileSetMan();
            FSM.FileSet fs = fsm.QureyFileSet(fsId);
            if (fs.File.Length < index || index < 0)
            {
                return;
            }
            fileName = fs.File[index].FileName;

            // 构造路径
            string path = fs.Path.ToString().Replace("\\", "/");
            //string Url = new Common.SysConfig().GetVodPlayUrl() + "/" + title + "/" + fileName;
            string Url = new Common.SysConfig().GetCartoonPlayUrl() + "/" + path + "/" + fileName;
            this.Response.Redirect(Url);
            /*
            string str = "";
            str += "<object id=\"player\" name=\"player\" classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" width=\"496\" height=\"300\">";
            str += "<param name=\"AUTOSTART\" value=\"-1\">";
            str += "<param name=\"SHUFFLE\" value=\"0\">";
            str += "<param name=\"PREFETCH\" value=\"0\">";
            str += "<param name=\"NOLABELS\" value=\"0\">";
            str += "<param name=\"CONTROLS\" value=\"Imagewindow\">";
            str += "<param name=\"CONSOLE\" value=\"clip1\">";
            str += "<param name=\"LOOP\" value=\"0\">";
            str += "<param name=\"NUMLOOP\" value=\"0\">";
            str += "<param name=\"CENTER\" value=\"1\">";
            str += "<param name=SRC value=\"" + Url + "\">";
            str += "<param name=\"MAINTAINASPECT\" value=\"1\">";
            str += "<param name=\"BACKGROUNDCOLOR\" value=\"#000000\">";
            str += "</object>";
             * */
            //lbReponse.Text = str;
        }
        catch (Exception e)
        {
            //lbPlay.Text = "未能查询到该电影的文件集: " + e.ToString();
        }


        //以下是以前的代码，从数据库根据电影ID，和文件集ID可以读出
        /*
        string strSql = "select vodList_pub.name,vodList_Pub.movieType,kindItem.itemName as movieTypeName,vodFile_pub.fileName  from vodList_pub ";
        strSql += " left join kindItem on kindItem.itemCode=vodList_pub.movieType and kindItem.kindId='movieType' ";
        strSql += " left join vodFile_pub on vodFile_pub.vodId=vodList_pub.Id ";
        strSql += " where vodList_pub.Id=" + Id + " and vodFile_pub.orderNumber='" + num.PadLeft(4, '0') + "'";
        DataTable dt = Access.execSql(strSql);
        Access.execSqlNoQuery1("update vodList_pub set playtimes=playtimes+1 where Id=" + Id);
        if (dt != null && dt.Rows.Count > 0)
        {
            this.Title = dt.Rows[0]["movieTypeName"].ToString() + " — " + dt.Rows[0]["name"].ToString();
            Random rd = new Random();
            string port = arrPort[rd.Next(0, arrPort.Length - 1)].ToString();
            string Url = new Common.SysConfig().GetVodPlayUrl() + "/" + dt.Rows[0]["title"].ToString() + "/" + dt.Rows[0]["fileName"].ToString();
            string str = "";
            str += "<object id=\"player\" name=\"player\" classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" width=\"496\" height=\"300\">";
            str += "<param name=\"AUTOSTART\" value=\"-1\">";
            str += "<param name=\"SHUFFLE\" value=\"0\">";
            str += "<param name=\"PREFETCH\" value=\"0\">";
            str += "<param name=\"NOLABELS\" value=\"0\">";
            str += "<param name=\"CONTROLS\" value=\"Imagewindow\">";
            str += "<param name=\"CONSOLE\" value=\"clip1\">";
            str += "<param name=\"LOOP\" value=\"0\">";
            str += "<param name=\"NUMLOOP\" value=\"0\">";
            str += "<param name=\"CENTER\" value=\"1\">";
            str += "<param name=SRC value=\"" + Url + "\">";
            str += "<param name=\"MAINTAINASPECT\" value=\"1\">";
            str += "<param name=\"BACKGROUNDCOLOR\" value=\"#000000\">";
            str += "</object>";
            lbReponse.Text = str;
        }
        else
        {
            Jscript.Alert("你播放的视频文件未找到，请与网站管理员联系。", lbReponse);
        }
         */
    }
}
