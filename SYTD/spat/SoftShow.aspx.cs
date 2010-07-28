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

public partial class SoftShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        Sub sub = new Sub();
        DataTable subList = sub.GetSubList(Access);
        if (subList != null && subList.Rows.Count > 0)
        {
            string currentSubCode = "";
            string requestSubCode = "";
            if (Request.QueryString["subCode"] != null && Request.QueryString["subCode"].ToString() != "")
            {
                requestSubCode = Request.QueryString["subCode"].ToString();
                currentSubCode = requestSubCode;
            }
            else
            {
                currentSubCode = sub.GetCurrentSub(Access, requestSubCode, subList.Rows[0]["subCode"].ToString());
            }

            ucHeader.Bind(subList);
            ucFooter.Bind(subList);
            string kind1 = "", kind2 = "", param = "";
            if (Request.QueryString["kind1"] != null && Request.QueryString["kind1"].ToString() != "")
            {
                kind1 = Request.QueryString["kind1"].ToString();
            }
            if (Request.QueryString["kind2"] != null && Request.QueryString["kind2"].ToString() != "")
            {
                kind2 = Request.QueryString["kind2"].ToString();
            }
            bindPath(Access, kind1);

            string id = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                id = Request.QueryString["id"].ToString();
            }

            bindSoftInfo(id);

        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    private void bindPath(DataAccess.DataAccess Access, string kind1)
    {
        lbPath.Text = "软件下载";
        if (kind1 != "")
        {
            string strSql = "select name from publishType where id='" + kind1 + "' and category="+FileShareCommon.Category.Software;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
            }
        }
    }

    private void bindSoftInfo(string id)
    {
        string strSql = "select BaseItem.Title,BaseItem.Brief,BaseItem.BrowseCount,BaseItem.Ext1,BaseItem.Ext2,BaseItem.Ext3,BaseItem.Ext4, SoftWare.Manufacturer, SoftWare.Version, PublishType.Name, FileItem.FileSetID from BaseItem ";
        strSql += " left join SoftWare on SoftWare.id=BaseItem.id ";
        strSql += " left join PublishType on PublishType.id=BaseItem.PublishType ";
        strSql += " left join FileItem on FileItem.id=BaseItem.id ";
        strSql += " where BaseItem.id=" + id;
        //此处根据ID读出并给页面空间赋值
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        if (dt != null)
        {
            if (dt.Rows.Count != 1)
            {
                lbName.Text = "无此影片的信息";
                return;
                //throw new ReadOnlyException("未找到该电影");
            }
            lbImg.Text = "<img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "/uploadFile/013/" + dt.Rows[0]["ext2"].ToString() + "/" + dt.Rows[0]["ext1"].ToString() + "\" border=\"0\" width=\"280\">";

            lbPath.Text += " — " + dt.Rows[0]["name"].ToString() + " — " + dt.Rows[0]["Title"].ToString();

            lbName.Text = dt.Rows[0]["Title"].ToString();
            lbManufacturer.Text = dt.Rows[0]["Manufacturer"].ToString();
            lbVersion.Text = dt.Rows[0]["Version"].ToString();
            lbBrowseCount.Text = dt.Rows[0]["BrowseCount"].ToString();
            lbLanguage.Text = dt.Rows[0]["EXT3"].ToString();
            lbIntro.Text = dt.Rows[0]["Brief"].ToString();
        }

        //下载这地方考虑下怎么做，是多个文件还是一个文件，如果是多个文件就显示列表， 将下载链接到SOFTDOWLOAD页面，传递下载URL和ID参数过去

        try
        {
            long fsId = Convert.ToInt64(dt.Rows[0]["FileSetID"]);
            FSM.FileSetMan fsm = new FSM.FileSetMan();
            FSM.FileSet fs = fsm.QureyFileSet(fsId);

            this.lbfsPath.Text = fs.Path;
            this.GridView1.DataSource = fs.File;
            this.GridView1.DataBind();
        }
        catch(Exception e)
        {
            Response.Write(e.Message);
        }
        Access.Dispose();
    }
    protected void btnDown_Click(object sender, EventArgs e)
    {

        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)GridView1.Rows[i].FindControl("chboxId");
            bool isChecked = chk.Checked;
            //bool isChecked = ((CheckBox)GridView1.Rows[i].FindControl("chboxId")).Checked;
            if(isChecked)
            {
            }
        }

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string title = this.lbName.Text;
        string softDownloadUrl = new Common.SysConfig().GetValueByKey("GetDownLoadURL");
        string fsPath = this.lbfsPath.Text.Replace("\\", "/");
        string url = "javascript:void window.open('" + softDownloadUrl + "/" + fsPath + "{0}');" ;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        //链接~
            ((HyperLink)e.Row.Cells[2].FindControl("HyperLink1")).NavigateUrl = 
                string.Format(url,e.Row.Cells[1].Text);
        }
    }
}
