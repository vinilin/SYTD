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

public partial class cartoonGrid : System.Web.UI.UserControl
{
    #region 参数
    private string _tblName;
    private string _strWhere;
    private string _searchKind;
    private int _pageIndex;
    private DataTable _columnDt;
    private DataTable _dataDt;
    private string _fieldCollections;
    private string _joinConditions;
    private int _pageSize = 8;
    private string _orderField;
    private bool _orderType;
    private bool _isShowHeader = false;


    #endregion

    #region 属性

    public string tblName
    {
        get { return this._tblName; }
        set { this._tblName = value; ViewState["tblName"] = value; }
    }

    public string strWhere
    {
        get { return ViewState["strWhere"].ToString(); }
        set
        {
            if (value == null)
            {
                _strWhere = "";
                ViewState["strWhere"] = "";
            }
            else
            {
                _strWhere = value; ViewState["strWhere"] = value;
            }
        }
    }
    public int pageIndex
    {
        get { return _pageIndex; }
        set
        {
            _pageIndex = value; ViewState["pageIndex"] = value;
        }
    }
    public int pageSize
    {
        set
        {
            if (value % 2 != 0)
            {
                value = value + 1;
            }
            this._pageSize = value; ViewState["pageSize"] = value;
        }
    }
    public string orderField
    {
        get { return _orderField; }
        set
        {
            _orderField = value; ViewState["orderField"] = value;
        }
    }
    public bool orderType
    {
        get { return _orderType; }
        set
        {
            _orderType = value; ViewState["orderType"] = value;
        }
    }
    public bool isShowHeader
    {
        set { this._isShowHeader = value; ViewState["isShowHeader"] = value; }
    }
    #endregion

    #region 方法
    public void databind()
    {
        ViewState["columnDt"] = _columnDt;
        createSearchSql();
        _dataDt = getData();
        createTable();
        PagerBind();
    }
    #endregion

    #region 事件

    #endregion


    #region 创建SQL

    private void createSearchSql()
    {

        this._fieldCollections += this._tblName + ".title,";
        this._fieldCollections += this._tblName + ".category,";
        this._fieldCollections += "publishType.name as categoryName,";
        this._fieldCollections += this._tblName + ".IssueDate,";
        this._fieldCollections += this._tblName + ".BrowseCount,";
        this._fieldCollections += this._tblName + ".ext1,";  //图片文件名
        this._fieldCollections += this._tblName + ".ext2,";  //SEQUENCE
        this._fieldCollections += this._tblName + ".ext3,";  //语言
        this._fieldCollections += this._tblName + ".ext4,";  //文件数
        this._fieldCollections += "Cartoon.author,";
        this._fieldCollections += this._tblName + ".Id,";
        this._fieldCollections += this._tblName + "." + ViewState["orderField"].ToString();
        this._joinConditions = " left join cartoon on cartoon.id="+this._tblName + ".id";
        //this._joinConditions += " left join publishType on publishType.Category=" + this._tblName + ".category";
        this._joinConditions += " left join publishType on publishType.id=" + this._tblName + ".PublishType";
        //this._joinConditions += " where BaseItem.Category=" + FileShareCommon.Category.Cartoon;

        ViewState["fieldCollections"] = _fieldCollections;
        ViewState["joinConditions"] = _joinConditions;

    }
    private DataTable getData()
    {
        if (ViewState["pageSize"] != null)
        {
            this._pageSize = (int)ViewState["pageSize"];
        }
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataAccess.ClsProcedureParameter objPara = new DataAccess.ClsProcedureParameter();
        objPara.AddValue("@tblName", this._tblName);
        objPara.AddValue("@fieldCollections", this._fieldCollections);
        objPara.AddValue("@pageSize", this._pageSize);
        objPara.AddValue("@pageIndex", this._pageIndex);
        objPara.AddValue("@orderField", this._orderField);
        objPara.AddValue("@orderType", this._orderType);
        if (strWhere != null && strWhere.Trim() != "")
        {
            objPara.AddValue("@strWhere", this._strWhere.TrimEnd());
        }
        if (this._joinConditions != null && this._joinConditions.Trim() != "")
        {
            objPara.AddValue("@joinConditions", this._joinConditions.TrimEnd());
        }
        DataTable dt = Access.ExecQuery("commonMyPage", objPara);
        if (dt != null)
        {
            int rowCount = dt.Rows.Count;
            for (int i = 0; i < (int)ViewState["pageSize"] - rowCount; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        Access.Dispose();
        return dt;
    }

    public int ItemTotal(string strTable, string strWhere, string orderField, int pageSize)
    {
        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataAccess.ClsProcedureParameter objPara = new DataAccess.ClsProcedureParameter();
        objPara.AddValue("@tblName", strTable);
        objPara.AddValue("@fieldCollections", orderField);
        objPara.AddValue("@orderField", orderField);
        if (strWhere != null && strWhere.Trim() != "")
        {
            objPara.AddValue("@strWhere", strWhere.TrimEnd());
        }
        objPara.AddValue("@IsCount", true);

        DataTable dt = Access.ExecQuery("commonMyPage", objPara);
        int result = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            try
            {
                result = System.Convert.ToInt32(dt.Rows[0]["Total"].ToString());
            }
            catch
            { }
        }
        Access.Dispose();
        dt.Clear();
        return result;
    }
    private void PagerBind()
    {
        int pageCount = 0;
        int TotalCount = ItemTotal(_tblName, _strWhere, _orderField, _pageSize);

        if ((System.Convert.ToDouble(TotalCount) / _pageSize) == System.Convert.ToInt32(TotalCount / _pageSize))
        {
            pageCount = TotalCount / _pageSize;
        }
        else
        {
            pageCount = System.Convert.ToInt32(TotalCount / _pageSize) + 1;
        }
        lbPageSummary.Text = "&nbsp;&nbsp;共<font color=red>" + pageCount + "</font>页，每页<font color=red>" + _pageSize + "</font>条数据，共<font color=red>" + TotalCount + "</font>条数据";
        PagerBind(_pageIndex, pageCount);
        ViewState["pageCount"] = pageCount;
    }
    private void pageIndexChangeed()
    {
        _strWhere = ViewState["strWhere"].ToString();
        _tblName = ViewState["tblName"].ToString();
        _fieldCollections = ViewState["fieldCollections"].ToString();
        _orderField = ViewState["orderField"].ToString();
        _orderType = System.Convert.ToBoolean(ViewState["orderType"]);
        _joinConditions = ViewState["joinConditions"].ToString();
        _columnDt = (DataTable)ViewState["columnDt"];
        _dataDt = getData();
        createTable();
    }
    #endregion

    #region 绑定表格

    private void createTable()
    {
        //EncryptDES encryptDES = new EncryptDES();
        string show = "";

        show += "<div>\r\n";
        if (ViewState["isShowHeader"] != null && (bool)ViewState["isShowHeader"])
        {
            show += "<table id=\"tabletest\" style=\"border-collapse:collapse;\" width=\"660\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#95B5DC\">\r\n";
        }
        else
        {
            show += "<table id=\"tabletest\" style=\"border-collapse:collapse;\" width=\"660\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#95B5DC\">\r\n";
        }
        show += "<thead>\r\n";
        show += "<tr style=\"display:none\">\r\n";
        show += "<th bgcolor=\"#EDF6FF\" width=\"310\"></th>";
        show += "<th bgcolor=\"#EDF6FF\" width=\"30\"></th>";
        show += "<th bgcolor=\"#EDF6FF\" width=\"310\"></th>";
        show += "</tr>\r\n";
        show += "<thead>\r\n";
        if (_dataDt != null && _dataDt.Rows.Count > 0)
        {
            show += "<tbody>\r\n";
            for (int i = 0; i < _dataDt.Rows.Count; i++)
            {
                show += "<tr><td height=\"10\" colspan=\"3\"></tr>" + "\r\n";
                show += "<tr>" + "\r\n";
                show += "<td>" + "\r\n";
                show += "   <table>" + "\r\n";
                show += "       <tr><td rowSpan=\"8\" width=\"144\">";
                if (_dataDt.Rows[i]["ext1"].ToString() != "")
                {
                    show += "       <img src=\""+new Common.SysConfig().GetValueByKey("ManagementUrl")+"/uploadFile/014/"+_dataDt.Rows[i]["ext2"].ToString()+"/"+_dataDt.Rows[i]["ext1"].ToString()+"\" border=\"0\" width=\"144\" height=\"180\"</td>";
                }
                else
                {
                    show += "&nbsp;";
                }
                show += "           <td rowSpan=\"8\" width=\"6\">" + "\r\n";
                show += "           <td width=\"160\" height=\"5\"></td></tr>" + "\r\n";
                if (_dataDt.Rows[i]["title"].ToString() != "")
                {
                    show += "       <tr><td height=\"25\" width=\"160\">名称：" + _dataDt.Rows[i]["title"].ToString() + "</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">作者：" + _dataDt.Rows[i]["author"].ToString() + "</td></tr>" + "\r\n";
                    //show += "       <tr><td height=\"25\" width=\"160\">导演：" + _dataDt.Rows[i]["director"].ToString() + "</td></tr>" + "\r\n";
                    //show += "       <tr><td height=\"25\" width=\"160\">语言：" + _dataDt.Rows[i]["ext3"].ToString() + "</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">类别：<a href='cartoon.aspx?kind1=" + _dataDt.Rows[i]["category"].ToString() + "'>" + _dataDt.Rows[i]["categoryName"].ToString() + "</a></td></tr>" + "\r\n";
                    if (_dataDt.Rows[i]["ext1"].ToString() == "1")
                    {
                        show += "       <tr><td height=\"25\" width=\"160\">集数：单集</td></tr>" + "\r\n";
                    }
                    else
                    {
                        show += "       <tr><td height=\"25\" width=\"160\">集数：" + _dataDt.Rows[i]["ext4"].ToString() + "</td></tr>" + "\r\n";
                    }
                    show += "       <tr><td height=\"25\"><a href='cartoonShow.aspx?kind1=" + _dataDt.Rows[i]["category"].ToString() + "&Id=" + _dataDt.Rows[i]["Id"].ToString() + "' target=_blank><img src=\"images/enter.gif\" border=\"0\"></a></td></tr>" + "\r\n";
                }
                else
                {
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    // show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                }
                show += "   </table>" + "\r\n";
                show += "</td>" + "\r\n";
                show += "<td>&nbsp;</td>" + "\r\n";
                i++;
                show += "<td>" + "\r\n";
                show += "   <table>" + "\r\n";
                show += "       <tr><td rowSpan=\"8\" width=\"144\">";
                if (_dataDt.Rows[i]["ext1"].ToString() != "")
                {
                    show += "       <img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "/uploadFile/014/" + _dataDt.Rows[i]["ext2"].ToString() + "/" + _dataDt.Rows[i]["ext1"].ToString() + "\" border=\"0\" width=\"144\" height=\"180\"</td>";
                }
                else
                {
                    show += "&nbsp;";
                }
                show += "           <td rowSpan=\"8\" width=\"6\">" + "\r\n";
                show += "           <td width=\"160\" height=\"5\"></td></tr>" + "\r\n";
                if (_dataDt.Rows[i]["title"].ToString() != "")
                {
                    show += "       <tr><td height=\"25\" width=\"160\">名称：" + _dataDt.Rows[i]["title"].ToString() + "</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">作者：" + _dataDt.Rows[i]["author"].ToString() + "</td></tr>" + "\r\n";
                    //show += "       <tr><td height=\"25\" width=\"160\">导演：" + _dataDt.Rows[i]["director"].ToString() + "</td></tr>" + "\r\n";
                    // show += "       <tr><td height=\"25\" width=\"160\">语言：" + _dataDt.Rows[i]["ext3"].ToString() + "</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">类别：<a href='cartoon.aspx?kind1=" + _dataDt.Rows[i]["category"].ToString() + "'>" + _dataDt.Rows[i]["categoryName"].ToString() + "</a></td></tr>" + "\r\n";
                    if (_dataDt.Rows[i]["ext1"].ToString() == "1")
                    {
                        show += "       <tr><td height=\"25\" width=\"160\">集数：单集</td></tr>" + "\r\n";
                    }
                    else
                    {
                        show += "       <tr><td height=\"25\" width=\"160\">集数：" + _dataDt.Rows[i]["ext4"].ToString() + "</td></tr>" + "\r\n";
                    }
                    show += "       <tr><td height=\"25\"><a href='cartoonShow.aspx?kind1=" + _dataDt.Rows[i]["category"].ToString() + "&Id=" + _dataDt.Rows[i]["Id"].ToString() + "'><img src=\"images/enter.gif\" border=\"0\"></a></td></tr>" + "\r\n";
                }
                else
                {
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                    show += "       <tr><td height=\"25\" width=\"160\">&nbsp;</td></tr>" + "\r\n";
                  //  show += "       <tr><td height=\"25\">&nbsp;</td></tr>" + "\r\n";
                }
                show += "   </table>" + "\r\n";
                show += "</td>" + "\r\n";
                show += "</tr>" + "\r\n";
            }
            show += "</tbody>\r\n";
        }
        show += "</table>\r\t";
        show += "<div>\r\t<br>";

        tableCell1.Text = show;
    }
    #endregion

    #region 分页

    #region 分页显示
    private void PagerBind(int pageIndex, int pageCount)
    {
        if (pageCount == 0)
        {
            lbtnFristPage.Visible = true;
            lbtnLastPage.Visible = true;
            lbtnPrePage.Visible = true;
            lbtnNextPage.Visible = true;
            lbtnFristPage.Enabled = false;
            lbtnLastPage.Enabled = false;
            lbtnPrePage.Enabled = false;
            lbtnNextPage.Enabled = false;

            lbtnPreDivision.Visible = false;
            lbtnNextDivision.Visible = false;

            lbtnPage1.Visible = true;
            lbtnPage1.Enabled = false;
            lbtnPage2.Visible = false;
            lbtnPage3.Visible = false;
            lbtnPage4.Visible = false;
            lbtnPage5.Visible = false;
            lbtnPage6.Visible = false;
            lbtnPage7.Visible = false;
            lbtnPage8.Visible = false;
            lbtnPage9.Visible = false;
            lbtnPage10.Visible = false;
        }
        else
        {
            lbtnFristPage.Visible = true;
            lbtnPrePage.Visible = true;
            lbtnLastPage.Visible = true;
            lbtnNextPage.Visible = true;

            if (pageCount == 1)
            {
                lbtnFristPage.Enabled = false;
                lbtnPrePage.Enabled = false;
                lbtnLastPage.Enabled = false;
                lbtnNextPage.Enabled = false;

            }
            else
            {
                if (pageIndex == 0)
                {
                    lbtnFristPage.Enabled = false;
                    lbtnPrePage.Enabled = false;
                    lbtnLastPage.Enabled = true;
                    lbtnNextPage.Enabled = true;

                }
                else if (pageIndex == (pageCount - 1))
                {
                    lbtnFristPage.Enabled = true;
                    lbtnPrePage.Enabled = true;
                    lbtnLastPage.Enabled = false;
                    lbtnNextPage.Enabled = false;

                }
                else
                {
                    lbtnFristPage.Enabled = true;
                    lbtnPrePage.Enabled = true;
                    lbtnLastPage.Enabled = true;
                    lbtnNextPage.Enabled = true;

                }
            }
            /////////////////////////////////////////////////////////////////
            int pageDivision = Decimal.ToInt32((pageCount + 9) / 10);
            int currentDivision = Decimal.ToInt32((pageIndex + 10) / 10);
            if (pageDivision == 1)
            {
                lbtnPreDivision.Visible = false;
                lbtnNextDivision.Visible = false;

            }
            else
            {
                if (currentDivision == 1)
                {
                    lbtnPreDivision.Visible = false;
                    lbtnNextDivision.Visible = true;
                    lbtnNextDivision.Enabled = true;

                }
                else if (currentDivision == pageDivision)
                {
                    lbtnPreDivision.Visible = true;
                    lbtnNextDivision.Visible = false;

                }
                else
                {
                    lbtnPreDivision.Visible = true;
                    lbtnNextDivision.Visible = true;
                    lbtnPreDivision.Enabled = true;
                    lbtnNextDivision.Enabled = true;

                }
            }
            /////////////////////////////////////////////////////////////////
            LinkButton[] lbtn = new LinkButton[10];

            lbtn[0] = lbtnPage1;
            lbtn[1] = lbtnPage2;
            lbtn[2] = lbtnPage3;
            lbtn[3] = lbtnPage4;
            lbtn[4] = lbtnPage5;
            lbtn[5] = lbtnPage6;
            lbtn[6] = lbtnPage7;
            lbtn[7] = lbtnPage8;
            lbtn[8] = lbtnPage9;
            lbtn[9] = lbtnPage10;

            int i = 0;
            for (i = 0; i < 10; i++)
            {
                lbtn[i].Visible = false;
            }
            if (pageDivision == 1)
            {
                for (i = 0; i < pageCount; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(i + 1) + "</font>";
                    if ((i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                    }
                }
            }
            else if (pageDivision == currentDivision)
            {
                for (i = 0; i < pageCount - (currentDivision - 1) * 10; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    if ((((currentDivision - 1) * 10) + i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                    }
                }
            }
            else
            {
                for (i = 0; i < 10; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    if ((((currentDivision - 1) * 10) + i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                    }
                }
            }

        }
    }
    #endregion

    #region 分页事件
    public void lbtnFristPage_Click(object sender, System.EventArgs e)
    {
        _pageIndex = 0;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnLastPage_Click(object sender, System.EventArgs e)
    {
        _pageIndex = System.Convert.ToInt32(ViewState["pageCount"]) - 1;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPrePage_Click(object sender, System.EventArgs e)
    {
        _pageIndex = System.Convert.ToInt32(ViewState["pageIndex"]) - 1;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnNextPage_Click(object sender, System.EventArgs e)
    {
        _pageIndex = System.Convert.ToInt32(ViewState["pageIndex"]) + 1;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPreDivision_Click(object sender, System.EventArgs e)
    {
        _pageIndex = Decimal.ToInt32((System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 - 1;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnNextDivision_Click(object sender, System.EventArgs e)
    {
        _pageIndex = Decimal.ToInt32((System.Convert.ToInt32(ViewState["pageIndex"]) / 10) + 1) * 10;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage1_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage2_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 1;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage3_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 2;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage4_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 3;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage5_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 4;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage6_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 5;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage7_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 6;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage8_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 7;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage9_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 8;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }

    public void lbtnPage10_Click(object sender, System.EventArgs e)
    {
        _pageIndex = (Decimal.ToInt32(System.Convert.ToInt32(ViewState["pageIndex"]) / 10)) * 10 + 9;
        ViewState["pageIndex"] = _pageIndex;
        PagerBind(pageIndex, System.Convert.ToInt32(ViewState["pageCount"]));
        pageIndexChangeed();
    }
    #endregion

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }

}
