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

public partial class cartoonShow: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        Sub sub = new Sub();
        DataTable subList = sub.GetSubList(Access);
        if (subList != null && subList.Rows.Count > 0)
        {
            ucHeader.Bind(subList);
            ucFooter.Bind(subList);
            string kind1 = "", kind2 = "", param = "014";
            if (Request.QueryString["kind1"] != null && Request.QueryString["kind1"].ToString() != "")
            {
                kind1 = Request.QueryString["kind1"].ToString();
            }
            if (Request.QueryString["kind2"] != null && Request.QueryString["kind2"].ToString() != "")
            {
                kind2 = Request.QueryString["kind2"].ToString();
            }
            if (Request.QueryString["param"] != null && Request.QueryString["param"].ToString() != "")
            {
                param = Request.QueryString["param"].ToString();
            }           

            bindPath(kind1, kind2, param);

            string id = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                id = Request.QueryString["id"].ToString();
            }

            bindCartoonInfo(id);
        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    private void bindPath(string kind1, string kind2, string param)
    {//kind1，表示电影类别；PARAM表示电影，在我这边是011，你可以用你自己定义的编号
        lbPath.Text = "动漫天地";

        //查处电影类别
        //lbPath.Text += " — "+电影类别;


        //查出电影名称
        //lbPath.Text += " — "+电影名称;
    }

    private void bindCartoonInfo(string Id)
    {
        string strSql = "select BaseItem.Title,BaseItem.Brief,BaseItem.Ext1,BaseItem.Ext2,BaseItem.Ext3,BaseItem.Ext4, Cartoon.Author, PublishType.Name, FileItem.FileSetID from BaseItem ";
        strSql += " left join Cartoon on Cartoon.id=BaseItem.id ";
        strSql += " left join PublishType on PublishType.id=BaseItem.PublishType ";
        strSql += " left join FileItem on FileItem.id=BaseItem.id ";
        strSql += " where BaseItem.id=" + Id;
        //此处根据ID读出并给页面空间赋值
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        if (dt != null)
        {
            if (dt.Rows.Count != 1)
            {
                lbName.Text = "无此动漫的信息";
                return;
                //throw new ReadOnlyException("未找到该电影");
            }
            lbImg.Text = "<img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "/uploadFile/014/" + dt.Rows[0]["ext2"].ToString() + "/" + dt.Rows[0]["ext1"].ToString() + "\" border=\"0\" width=\"280\">";

            lbPath.Text += " — " + dt.Rows[0]["name"].ToString() + " — " + dt.Rows[0]["Title"].ToString();

            lbName.Text = dt.Rows[0]["Title"].ToString();
            //lbActor.Text = dt.Rows[0]["Player"].ToString();
            lbDirector.Text = dt.Rows[0]["Author"].ToString();
            lbCartoonType.Text = dt.Rows[0]["Name"].ToString();
            lbIntro.Text = dt.Rows[0]["Brief"].ToString();
            lbLanguage.Text = dt.Rows[0]["ext3"].ToString();
            lbCartoonNum.Text = dt.Rows[0]["ext4"].ToString();
        }

        // 查询文件集
       try
        {
            long fsId = Convert.ToInt64(dt.Rows[0]["FileSetID"]);
            FSM.FileSetMan fsm = new FSM.FileSetMan();
            FSM.FileSet fs = fsm.QureyFileSet(fsId);
            lbCartoonNum.Text = fs.File.Length.ToString();
            for (int i = 0; i < fs.File.Length; i++)
            {

                lbPlay.Text += "&nbsp;&nbsp;<a onclick='winOpen(" + Id + "," + i.ToString() + ");' style=\"cursor:hand;\">[第" + (i+1).ToString() + "集]</a>&nbsp;&nbsp;";
            }
        }
        catch (Exception e)
        {
            lbPlay.Text = "未能查询到该动漫的文件集: " + e.ToString();
        }
        

        //读出影片列表，给lbPlay赋值



        /*  这是以前的例子
         if (dt.Rows[0]["movieNum"].ToString()=="1")
            {
                lbCartoonNum.Text = "单集";
                lbPlay.Text = "<a onclick='winOpen(" + dt.Rows[0]["Id"].ToString() + ",1);'><img src=\"images/play.gif\" border=0></a>";
            }
            else
            {
                lbCartoonNum.Text = dt.Rows[0]["movieNum"].ToString();
                int movieNum = System.Convert.ToInt32(dt.Rows[0]["movieNum"]);
                for (int i = 1; i <= movieNum; i++)
                {
                    lbPlay.Text += "&nbsp;&nbsp;<a onclick='winOpen(" + dt.Rows[0]["Id"].ToString() + "," + i.ToString() + ");' style=\"cursor:hand;\">[第" + i.ToString() + "集]</a>&nbsp;&nbsp;";
                }
            }
         */

        Access.Dispose();
    }
}
