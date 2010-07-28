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

public partial class musicShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack == true) 
        {
            return;
        } 
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
            string kind1 = "", kind2 = "", param = "012";
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

            string id = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                id = Request.QueryString["id"].ToString();
            }

            bindPath(Access, kind1, kind2, param);
            bindKind(Access, kind1, kind2, param);
            bindPH();
            bindMusicList(id);
        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    // 内容路径
    private void bindPath(DataAccess.DataAccess Access, string kind1, string kind2, string param)
    {
        lbPath.Text = "音乐欣赏";
        string strSql = "select NAME as text from PublishType where Category=2 ";
        DataTable tempDt = Access.execSql(strSql);
        if (tempDt != null && tempDt.Rows.Count > 0)
        {
            lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
        }
    }

    private void bindKind(DataAccess.DataAccess Access, string kind1, string kind2, string param)
    {
        //string strSql = "select code,text from T_SystemKind where kind='" + param + "' order by listOrder";
        string strSql = "select ID as code, NAME as text from PublishType where Category=2 ";
        DataTable dt = Access.execSql(strSql);
        if (dt != null && dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell td;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();
                td = new TableCell();
                td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"music.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                td.Height = Unit.Pixel(30);
                td.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(td);
                tb_kind.Rows.Add(tr);
            }
        }
    }
    private void bindPH()
    {
        //读出播放次数最高的音乐
        string strSql = "select TOP 10 BaseItem.id,BaseItem.Title,BaseItem.category from BaseItem ";
        strSql += " where BaseItem.category=" + FileShareCommon.Category.Music;
        strSql += "  order by BaseItem.BrowseCount desc";

        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        if (dt != null)
        {
            int rowCount = dt.Rows.Count;
            for (int i = 0; i < 8 - rowCount; i++)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = 0;
                dr["title"] = "";
                dr["category"] = 0;
                dt.Rows.Add(dr);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tr = new TableRow();

                TableCell td2 = new TableCell();
                td2.Height = Unit.Pixel(25);
                if (dt.Rows[i]["title"].ToString() != "")
                {
                    if (dt.Rows[i]["title"].ToString().Length <= 10)
                    {
                        td2.Text = "<a href=\"musicShow.aspx?Id=" + dt.Rows[i]["Id"].ToString() + "\" target=_blank>" + (i + 1).ToString() + ". " + dt.Rows[i]["title"].ToString() + "</a>";
                    }
                    else
                    {
                        td2.Text = "<a href=\"musicShow.aspx?Id=" + dt.Rows[i]["Id"].ToString() + "\" target=_blank>" + (i + 1).ToString() + ". " + dt.Rows[i]["title"].ToString().Substring(0, 12) + "...</a>";
                    }
                }
                else
                {
                    td2.Text = "&nbsp;";
                }
                tr.Cells.Add(td2);

                tablePlayTimes.Rows.Add(tr);

            }
        }
        Access.Dispose();
    }
    private void bindMusicList(string id)
    {
        string strSql = "select BaseItem.BrowseCount, BaseItem.Title,BaseItem.Brief,BaseItem.Ext1,BaseItem.Ext2,BaseItem.Ext3,BaseItem.Ext4, Music.Singer, Music.Author, PublishType.Name, FileItem.FileSetID from BaseItem ";
        strSql += " left join Music on Music.id=BaseItem.id ";
        strSql += " left join PublishType on PublishType.id=BaseItem.PublishType ";
        strSql += " left join FileItem on FileItem.id=BaseItem.id ";
        strSql += " where BaseItem.id=" + id;
        //此处根据ID读出并给页面空间赋值
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        
        //this.GridView1
        if (dt != null)
        {
            if (dt.Rows.Count != 1)
            {
                //lbName.Text = "无此影片的信息";
                return;
                //throw new ReadOnlyException("未找到该电影");
            }
            lbImg.Text = "<img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "/uploadFile/012/" + dt.Rows[0]["ext2"].ToString() + "/" + dt.Rows[0]["ext1"].ToString() + "\" border=\"0\" width=\"280\">";

            lbPath.Text += " — " + dt.Rows[0]["name"].ToString() + " — " + dt.Rows[0]["Title"].ToString();

            lbTitle.Text = dt.Rows[0]["Title"].ToString();
            lbSonger.Text = dt.Rows[0]["Singer"].ToString();
            lbInfo.Text = dt.Rows[0]["Brief"].ToString();
        }

        // 查询文件集
        try
        {
            long fsId = Convert.ToInt64(dt.Rows[0]["FileSetID"]);
            FSM.FileSetMan fsm = new FSM.FileSetMan();
            FSM.FileSet fs = fsm.QureyFileSet(fsId);
            
            this.GridView1.DataSource = fs.File;
            this.GridView1.DataBind();
            this.lbfsPath.Text = fs.Path;
            /*
            for (int i = 0; i < fs.File.Length; i++)
            {

                TableRow tr = new TableRow();
                TableCell td1 = new TableCell();
                TableCell td2 = new TableCell();
                TableCell td3 = new TableCell();
                TableCell td4 = new TableCell();

                td1.Text = (i + 1).ToString();
                td1.HorizontalAlign = HorizontalAlign.Center;

                td2.Text = fs.File[i].FileName;  //音乐名称
                td2.HorizontalAlign = HorizontalAlign.Center;

                td3.Text = "<a style=\"cursor:hand\" onclick=\"javascript:window.open ('musicPlay.aspx?id=" + fsId.ToString() + "&fileId=" + fs.File[i].ID.ToString() + "','newwindow', 'height=120, width=400, top=150, left=250, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no');\">试听</a>";
                td3.HorizontalAlign = HorizontalAlign.Center;

                td4.Text = dt.Rows[0]["BrowseCount"].ToString();  //这里显示每首歌的点击次数
                td4.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(td1);
                tr.Cells.Add(td2);
                tr.Cells.Add(td3);
                tr.Cells.Add(td4);

                tbList.Rows.Add(tr);
                //lbPlay.Text += "&nbsp;&nbsp;<a onclick='winOpen(" + Id + "," + i.ToString() + ");' style=\"cursor:hand;\">[第" + (i + 1).ToString() + "集]</a>&nbsp;&nbsp;";
            }
             * */
        }
        catch (Exception e)
        {
            //lbPlay.Text = "未能查询到该电影的文件集: " + e.ToString();
        }
        /////////////////////////////////////////
        //DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        //读出该专辑的音乐文件
        /*
        string strSql = "";
       // DataTable dt = Access.execSql(strSql);

        DataTable dt = new DataTable();
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tr = new TableRow();
                TableCell td1 = new TableCell();
                TableCell td2 = new TableCell();
                TableCell td3 = new TableCell();
                TableCell td4 = new TableCell();

                td1.Text = (i + 1).ToString();
                td1.HorizontalAlign = HorizontalAlign.Center;

                //td2.Text = dt.rows[i][""].ToString();  //音乐名称
                td2.HorizontalAlign = HorizontalAlign.Center;

                td3.Text = "<a style=\"cursor:hand\" onclick=\"javascript:window.open ('musicPlay.aspx?id="+dt.Rows[i]["id"].ToString()+"&fileId="+dt.Rows[i]["fileId"].ToString()+"','newwindow', 'height=120, width=400, top=150, left=250, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no');\">试听</a>";
                td3.HorizontalAlign = HorizontalAlign.Center;

                //td4.Text = dt.Rows[i][""].ToString();  //这里显示每首歌的点击次数
                td4.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(td1);
                tr.Cells.Add(td2);
                tr.Cells.Add(td3);
                tr.Cells.Add(td4);

                tbList.Rows.Add(tr);
            }
        }
         * */
        Access.Dispose();
    }
    protected void btnPlay_Click(object sender, EventArgs e)
    {
        string title = this.lbTitle.Text;
        /*
        string id="";
        if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
        {
            id = Request.QueryString["id"].ToString();
        }
        string strSql = "select BaseItem.BrowseCount, BaseItem.Title,BaseItem.Brief,BaseItem.Ext1,BaseItem.Ext2,BaseItem.Ext3,BaseItem.Ext4, Music.Singer, Music.Author, PublishType.Name, FileItem.FileSetID from BaseItem ";
        strSql += " left join Music on Music.id=BaseItem.id ";
        strSql += " left join PublishType on PublishType.id=BaseItem.PublishType ";
        strSql += " left join FileItem on FileItem.id=BaseItem.id ";
        strSql += " where BaseItem.id=" + id;
        //此处根据ID读出并给页面空间赋值
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        long fsId = Convert.ToInt64(dt.Rows[0]["FileSetID"]);
        FSM.FileSetMan fsm = new FSM.FileSetMan();
        FSM.FileSet fs = fsm.QureyFileSet(fsId);
         * */

        // 写m3u文件
        string webPath = Request.PhysicalApplicationPath;
        //string webPath2 = Request.PhysicalPath;
        string playListFile = webPath + "\\MusicPlayList\\a.m3u";
        //if (!System.IO.File.Exists(playListFile))
        {
            //System.IO.FileStream f = System.IO.File.Create(playListFile);
            System.IO.FileStream f = System.IO.File.Open(playListFile,  System.IO.FileMode.OpenOrCreate | System.IO.FileMode.Truncate);
            f.Close();
        }
        System.IO.StreamWriter f2 = new System.IO.StreamWriter(playListFile, true, System.Text.Encoding.GetEncoding("gb2312"));

        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)GridView1.Rows[i].FindControl("chboxId");
            bool isChecked = chk.Checked;
            //bool isChecked = ((CheckBox)GridView1.Rows[i].FindControl("chboxId")).Checked;
            if(isChecked)
            {
                string musicPlayUrl = new Common.SysConfig().GetMusicPlayUrl();
                string fsPath = this.lbfsPath.Text.Replace("\\", "/");
                string url = musicPlayUrl + "/" + fsPath + "/" + this.GridView1.Rows[i].Cells[2].Text;
                f2.WriteLine(url + "\r\n");
            }
        }
        f2.Close();
        f2.Dispose();

        // 播放
        //Response.Redirect("musicPlay.aspx?filename="+fs.File[0].FileName+"&title="+dt.Rows[0]["Title"]+"&type1=ff");
        Response.Redirect("musicPlay.aspx?filename=" + "a.m3u" + "&title=" + title + "&type1=playlist");
    }
    protected void btnSel_Click(object sender, EventArgs e)
    {
        for(int i = 0 ; i < this.GridView1.Rows.Count; ++i)
        {
            ((CheckBox) this.GridView1.Rows[i].FindControl("chboxId")).Checked = true;
        }
    }
    protected void btnUnsel_Click(object sender, EventArgs e)
    {
        for(int i = 0 ; i < this.GridView1.Rows.Count; ++i)
        {
            ((CheckBox) this.GridView1.Rows[i].FindControl("chboxId")).Checked = false;
        }
    }
}