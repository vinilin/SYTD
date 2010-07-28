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


public partial class commonGrid : System.Web.UI.UserControl
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
    private string _attachLink;
    private int _pageSize = 20;
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
    public string searchKind
    {
        set { this._searchKind = value; ViewState["searchKind"] = value; }
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
        set { this._pageSize = value; ViewState["pageSize"] = value; }
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
    public string attachLink
    {
        set { this._attachLink = value; ViewState["attachLink"] = value; }
    }
    #endregion

    #region 方法
    public void databind()
    {
        _columnDt = getColumnData();
        ViewState["columnDt"] = _columnDt;
        createSearchSql();
        _dataDt = getData();
        createTable();
        PagerBind();
    }
    #endregion

    #region 事件

    #endregion


    #region
    private DataTable getColumnData()
    {
        DataAccess.ClsProcedureParameter objPara = new DataAccess.ClsProcedureParameter();
        objPara.AddValue("@tblName", this._tblName);
        objPara.AddValue("@searchKind", this._searchKind);
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        return Access.ExecQuery("commonMyPagePara", objPara);
    }
    private void createSearchSql()
    {
        if (_columnDt != null && _columnDt.Rows.Count > 0)
        {
            for (int i = 0; i < _columnDt.Rows.Count; i++)
            {
                try
                {
                    //组合查询字段
                    string tempFieldCollections = "";
                    if (_columnDt.Rows[i]["cnameKind"].ToString() == "as")
                    {
                        tempFieldCollections += _columnDt.Rows[i]["fields"].ToString() + " as " + _columnDt.Rows[i]["fieldsCname"].ToString();
                    }
                    else if (_columnDt.Rows[i]["cnameKind"].ToString() == "=")
                    {
                        tempFieldCollections += _columnDt.Rows[i]["fieldsCname"].ToString() + " = " + _columnDt.Rows[i]["fields"].ToString();
                    }
                    else if (_columnDt.Rows[i]["cnameKind"].ToString() == "select")
                    {
                        tempFieldCollections = " '' as " + _columnDt.Rows[i]["fieldsCname"].ToString();
                    }
                    if (_fieldCollections != null && _fieldCollections.Trim() != "")
                    {
                        _fieldCollections += "," + tempFieldCollections;
                    }
                    else
                    {
                        _fieldCollections += tempFieldCollections;
                    }
                    //组合查询关联
                    _joinConditions += " " + _columnDt.Rows[i]["joinConditions"].ToString();
                }
                catch (Exception ex)
                {
                    Common.Log.LogError(ex.ToString(), "生成查询出错");
                }
            }
            if (ViewState["IncludeState"] != null && (bool)ViewState["IncludeState"])
            {
                _fieldCollections += "," + this._tblName + ".state," + this._tblName + ".fileCount";
            }
            _fieldCollections = _fieldCollections + "," + this._tblName + "." + this.orderField + " as ids," + this._tblName + ".Id";
            ViewState["fieldCollections"] = _fieldCollections;
            ViewState["joinConditions"] = _joinConditions;
        }
    }
    private DataTable getData()
    {
        if (ViewState["pageSize"] != null)
        {
            this._pageSize = (int)ViewState["pageSize"];
        }
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
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
        Access.Dispose();
        return dt;
    }

    public int ItemTotal(string strTable, string strWhere, string orderField, int pageSize)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
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
        lbPageSummary_top.Text = "&nbsp;&nbsp;共<font color=red>" + pageCount + "</font>页，每页<font color=red>" + _pageSize + "</font>条数据，共<font color=red>" + TotalCount + "</font>条数据";
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
        Common.EncryptDES encryptDES = new Common.EncryptDES();
        string show = "";
        if (_columnDt != null && _columnDt.Rows.Count > 0)
        {
            show += "<div>\r\n";
            if (ViewState["isShowHeader"] != null && (bool)ViewState["isShowHeader"])
            {
                show += "<table id=\"tabletest\" style=\"border-collapse:collapse;border-width:1px;border-style:solid;border-color:#95B5DC\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n";
            }
            else
            {
                show += "<table id=\"tabletest\" style=\"border-collapse:collapse;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#95B5DC\">\r\n";
            }
            show += "<thead>\r\n";
            if (ViewState["isShowHeader"] != null && (bool)ViewState["isShowHeader"])
            {
                show += "<tr height=25>\r\n";
            }
            else
            {
                show += "<tr style=\"display:none\">\r\n";
            }
            int tableWidth = 0;
            for (int i = 0; i < _columnDt.Rows.Count; i++)
            {
                if (!(bool)_columnDt.Rows[i]["colHidden"])
                {
                    show += "<th align=center valign=center bgcolor=\"#EDF6FF\"";
                    if (System.Convert.ToInt32(_columnDt.Rows[i]["width"]) > 0)
                    {
                        show += " width=" + _columnDt.Rows[i]["width"].ToString();
                        tableWidth += System.Convert.ToInt32(_columnDt.Rows[i]["width"]);
                    }
                    else
                    {
                        tableWidth += 100;
                    }
                    show += ">";
                    show += _columnDt.Rows[i]["headText"].ToString();
                    show += "</th>\r\n";
                }
            }
            show += "</tr>\r\n";
            show += "<thead>\r\n";
            if (_dataDt != null && _dataDt.Rows.Count > 0)
            {
                //_dataKey = new object[_dataDt.Rows.Count];
                show += "<tbody>\r\n";

                for (int rowIndex = 0; rowIndex < _dataDt.Rows.Count; rowIndex++)
                {
                    show += "<tr height=25>";
                    //if ((System.Convert.ToDecimal(rowIndex) / 2) == (rowIndex / 2))
                    //{
                    //    show += "bgcolor=#ffffff>";
                    //}
                    //else
                    //{
                    //    show += "bgcolor=WhiteSmoke>";
                    //}
                    for (int columnIndex = 0; columnIndex < _columnDt.Rows.Count; columnIndex++)
                    {
                        if (!(bool)_columnDt.Rows[columnIndex]["colHidden"])
                        {
                            show += "<td border=\"0\" valign=" + _columnDt.Rows[columnIndex]["valign"].ToString() + " align=" + _columnDt.Rows[columnIndex]["align"].ToString();
                            show += " width=" + _columnDt.Rows[columnIndex]["width"].ToString();
                            if (System.Convert.ToBoolean(_columnDt.Rows[columnIndex]["nowarp"]))
                            {
                                show += " wrap=false>&nbsp;&nbsp;";
                            }
                            else
                            {
                                show += " wrap=true>&nbsp;&nbsp;";
                            }

                            if ((bool)_columnDt.Rows[columnIndex]["isLink"] && ViewState["attachLink"] != null)
                            {
                                bool defineLink = false;
                                string linkUrl = "";
                                try
                                {
                                    if (_dataDt.Rows[rowIndex]["defaultPic"].ToString() == "999999999")
                                    {
                                        defineLink = true;
                                    }
                                    linkUrl = _dataDt.Rows[rowIndex]["articleContent"].ToString();
                                }
                                catch { }
                                if (defineLink)
                                {
                                    show += "<a href=\"" + linkUrl + "\" target=\"_blank\">";
                                }
                                else
                                {
                                    show += "<a href=\"" + ViewState["attachLink"].ToString() + "&id=" + _dataDt.Rows[rowIndex]["Id"].ToString() + "\" target=\"_blank\">";
                                }
                            }

                            ///////////////////////格式化数据////////////////////////////////////
                            if (_columnDt.Rows[columnIndex]["formatString"].ToString() == "")
                            {
                                show += _dataDt.Rows[rowIndex][columnIndex].ToString();
                            }
                            /////////日期
                            else if (_columnDt.Rows[columnIndex]["formatString"].ToString() == "showDate")
                            {
                                try
                                {
                                    show += System.Convert.ToDateTime(_dataDt.Rows[rowIndex][columnIndex]).ToString("yyyy-MM-dd");
                                }
                                catch
                                {
                                    show += _dataDt.Rows[rowIndex][columnIndex].ToString();
                                }
                            }
                            /////////日期时间
                            else if (_columnDt.Rows[columnIndex]["formatString"].ToString() == "showDateTime")
                            {
                                try
                                {
                                    show += System.Convert.ToDateTime(_dataDt.Rows[rowIndex][columnIndex]).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    show += _dataDt.Rows[rowIndex][columnIndex].ToString();
                                }
                            }
                            /////////货币
                            else if (_columnDt.Rows[columnIndex]["formatString"].ToString() == "currency")
                            {
                                try
                                {
                                    show += System.Convert.ToDecimal(_dataDt.Rows[rowIndex][columnIndex]).ToString("C");
                                }
                                catch
                                {
                                    show += _dataDt.Rows[rowIndex][columnIndex].ToString();
                                }
                            }
                            else
                            {
                                show += _dataDt.Rows[rowIndex][columnIndex].ToString();
                            }
                            /////////////////////////////////////////////////////////////////////
                            if ((bool)_columnDt.Rows[columnIndex]["isLink"] && ViewState["attachLink"] != null)
                            {
                                show += "</a>";
                            }
                            show += "</td>\r\n";
                        }
                    }
                    show += "</tr>\r\n";
                    show += "<tr><td height=\"1\" background=\"images/line.gif\" colspan=" + _columnDt.Rows.Count.ToString() + "></td></tr>";
                    if (ViewState["isShowHeader"] != null && !(bool)ViewState["isShowHeader"])
                    {
                        if (rowIndex < _dataDt.Rows.Count - 1)
                        {
                            show += "<tr>\r\n";
                            show += "<td height=\"1\" background=\"images/default/dot.gif\" colspan=\"" + _columnDt.Rows.Count + "\"></a>";
                            show += "</tr>\r\n";
                        }
                    }
                }
                show += "</tbody>\r\n";

            }
            show += "</table>\r\t";
            show += "<div>\r\t<br>";
            show += "<script language=javascript>document.getElementById('tabletest').width=" + tableWidth + ";</script>";
        }
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

            lbtnFristPage_top.Visible = true;
            lbtnLastPage_top.Visible = true;
            lbtnPrePage_top.Visible = true;
            lbtnNextPage_top.Visible = true;
            lbtnFristPage_top.Enabled = false;
            lbtnLastPage_top.Enabled = false;
            lbtnPrePage_top.Enabled = false;
            lbtnNextPage_top.Enabled = false;

            lbtnPreDivision_top.Visible = false;
            lbtnNextDivision_top.Visible = false;

            lbtnPage1_top.Visible = true;
            lbtnPage1_top.Enabled = false;
            lbtnPage2_top.Visible = false;
            lbtnPage3_top.Visible = false;
            lbtnPage4_top.Visible = false;
            lbtnPage5_top.Visible = false;
            lbtnPage6_top.Visible = false;
            lbtnPage7_top.Visible = false;
            lbtnPage8_top.Visible = false;
            lbtnPage9_top.Visible = false;
            lbtnPage10_top.Visible = false;
        }
        else
        {
            lbtnFristPage.Visible = true;
            lbtnPrePage.Visible = true;
            lbtnLastPage.Visible = true;
            lbtnNextPage.Visible = true;

            lbtnFristPage_top.Visible = true;
            lbtnPrePage_top.Visible = true;
            lbtnLastPage_top.Visible = true;
            lbtnNextPage_top.Visible = true;

            if (pageCount == 1)
            {
                lbtnFristPage.Enabled = false;
                lbtnPrePage.Enabled = false;
                lbtnLastPage.Enabled = false;
                lbtnNextPage.Enabled = false;

                lbtnFristPage_top.Enabled = false;
                lbtnPrePage_top.Enabled = false;
                lbtnLastPage_top.Enabled = false;
                lbtnNextPage_top.Enabled = false;
            }
            else
            {
                if (pageIndex == 0)
                {
                    lbtnFristPage.Enabled = false;
                    lbtnPrePage.Enabled = false;
                    lbtnLastPage.Enabled = true;
                    lbtnNextPage.Enabled = true;

                    lbtnFristPage_top.Enabled = false;
                    lbtnPrePage_top.Enabled = false;
                    lbtnLastPage_top.Enabled = true;
                    lbtnNextPage_top.Enabled = true;
                }
                else if (pageIndex == (pageCount - 1))
                {
                    lbtnFristPage.Enabled = true;
                    lbtnPrePage.Enabled = true;
                    lbtnLastPage.Enabled = false;
                    lbtnNextPage.Enabled = false;

                    lbtnFristPage_top.Enabled = true;
                    lbtnPrePage_top.Enabled = true;
                    lbtnLastPage_top.Enabled = false;
                    lbtnNextPage_top.Enabled = false;
                }
                else
                {
                    lbtnFristPage.Enabled = true;
                    lbtnPrePage.Enabled = true;
                    lbtnLastPage.Enabled = true;
                    lbtnNextPage.Enabled = true;

                    lbtnFristPage_top.Enabled = true;
                    lbtnPrePage_top.Enabled = true;
                    lbtnLastPage_top.Enabled = true;
                    lbtnNextPage_top.Enabled = true;
                }
            }
            /////////////////////////////////////////////////////////////////
            int pageDivision = Decimal.ToInt32((pageCount + 9) / 10);
            int currentDivision = Decimal.ToInt32((pageIndex + 10) / 10);
            if (pageDivision == 1)
            {
                lbtnPreDivision.Visible = false;
                lbtnNextDivision.Visible = false;

                lbtnPreDivision_top.Visible = false;
                lbtnNextDivision_top.Visible = false;
            }
            else
            {
                if (currentDivision == 1)
                {
                    lbtnPreDivision.Visible = false;
                    lbtnNextDivision.Visible = true;
                    lbtnNextDivision.Enabled = true;

                    lbtnPreDivision_top.Visible = false;
                    lbtnNextDivision_top.Visible = true;
                    lbtnNextDivision_top.Enabled = true;
                }
                else if (currentDivision == pageDivision)
                {
                    lbtnPreDivision.Visible = true;
                    lbtnNextDivision.Visible = false;

                    lbtnPreDivision_top.Visible = true;
                    lbtnNextDivision_top.Visible = false;
                }
                else
                {
                    lbtnPreDivision.Visible = true;
                    lbtnNextDivision.Visible = true;
                    lbtnPreDivision.Enabled = true;
                    lbtnNextDivision.Enabled = true;

                    lbtnPreDivision_top.Visible = true;
                    lbtnNextDivision_top.Visible = true;
                    lbtnPreDivision_top.Enabled = true;
                    lbtnNextDivision_top.Enabled = true;
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

            LinkButton[] lbtn_top = new LinkButton[10];

            lbtn_top[0] = lbtnPage1_top;
            lbtn_top[1] = lbtnPage2_top;
            lbtn_top[2] = lbtnPage3_top;
            lbtn_top[3] = lbtnPage4_top;
            lbtn_top[4] = lbtnPage5_top;
            lbtn_top[5] = lbtnPage6_top;
            lbtn_top[6] = lbtnPage7_top;
            lbtn_top[7] = lbtnPage8_top;
            lbtn_top[8] = lbtnPage9_top;
            lbtn_top[9] = lbtnPage10_top;


            int i = 0;
            for (i = 0; i < 10; i++)
            {
                lbtn[i].Visible = false;
                lbtn_top[i].Visible = false;
            }
            if (pageDivision == 1)
            {
                for (i = 0; i < pageCount; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn_top[i].Visible = true;
                    lbtn_top[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(i + 1) + "</font>";
                    lbtn_top[i].Text = "<font face=Arial>" + System.Convert.ToString(i + 1) + "</font>";
                    if ((i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                        lbtn_top[i].Enabled = false;
                    }
                }
            }
            else if (pageDivision == currentDivision)
            {
                for (i = 0; i < pageCount - (currentDivision - 1) * 10; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn_top[i].Visible = true;
                    lbtn_top[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    lbtn_top[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    if ((((currentDivision - 1) * 10) + i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                        lbtn_top[i].Enabled = false;
                    }
                }
            }
            else
            {
                for (i = 0; i < 10; i++)
                {
                    lbtn[i].Visible = true;
                    lbtn[i].Enabled = true;

                    lbtn_top[i].Visible = true;
                    lbtn_top[i].Enabled = true;

                    lbtn[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    lbtn_top[i].Text = "<font face=Arial>" + System.Convert.ToString(((currentDivision - 1) * 10) + i + 1) + "</font>";
                    if ((((currentDivision - 1) * 10) + i + 1) == pageIndex + 1)
                    {
                        lbtn[i].Enabled = false;
                        lbtn_top[i].Enabled = false;
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