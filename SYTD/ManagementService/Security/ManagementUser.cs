using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Security
{
    [RemotingService]
    public class ManagementUser
    {
        [DataTableType("ManagementUser.GetUserList")]
        public DataTable GetUserList(FluorineFx.AMF3.ArrayCollection funcCodes)
        {
            try
            {
            DataTable funcDt = new DataTable();
            DataColumn dc1 = new DataColumn("FUNCCODE");
            DataColumn dc2 = new DataColumn("KIND");
            funcDt.Columns.Add(dc1);
            funcDt.Columns.Add(dc2);
            for(int i=0;i<funcCodes.Count;i++)
            {
                System.Collections.Hashtable uFunc = (System.Collections.Hashtable)funcCodes[i];
                DataRow dr = funcDt.NewRow();
                dr["FUNCCODE"] = uFunc["FUNCCODE"].ToString();
                dr["KIND"] = uFunc["KIND"].ToString();
                funcDt.Rows.Add(dr);
            }
            string tempStr = "";
            for (int i = 0; i < funcDt.Rows.Count;i++ )
            {
                if (i!=0){ tempStr += " or ";}
                tempStr += " T_SystemUserAndRolePopedom.funcCode = '"+funcDt.Rows[i]["FUNCCODE"].ToString()+"' ";
            }
            if (tempStr != "")
            {
                tempStr = " and (" + tempStr + ")";
            }
            string strSql = "SELECT  T_SystemUser.ID,T_SystemUser.USERNAME, T_SystemUser.TRUENAME, T_SystemUser.SUBCODE, T_SystemUser.ADDMAN, DATEADD(Hour, - 8, T_SystemUser.addTime)   AS ADDTIME, ";
            strSql += " CASE T_SystemUser.isBindIp when 1 then '是' else '否' end as ISBINDIP,case T_SystemUser.isBindIp when 1 then T_SystemUser.BINDIP else '' end as BINDIP,";
            strSql += "               T_SubSection.SUBNAME,'' as USERROLECODE,'' as USERROLENAME,'' as OTHERPOPEDOM,'' as OTHERPOPEDOMNAME";
            strSql += "       FROM    T_SystemUser ";
            strSql += "               INNER JOIN (SELECT userName ";
            strSql += "                 FROM  ";
            strSql += "                      (SELECT  T_SystemUser_1.userName";
            strSql += "                        FROM   T_SystemUser AS T_SystemUser_1";
            strSql += "                               INNER JOIN T_SystemUserAndRolePopedom ON T_SystemUserAndRolePopedom.userName = T_SystemUser_1.userName ";
            strSql += "                                                                        AND  T_SystemUserAndRolePopedom.type = 'user'  and T_SystemUser_1.superior=0 ";
            strSql += "                                                                        " + tempStr + ") AS derivedtbl_1";
            strSql += "                       GROUP BY userName)";
            strSql += "                       AS T_User ";
            strSql += "                 ON T_User.userName = T_SystemUser.userName";
            strSql += "             LEFT OUTER JOIN T_SubSection ON T_SubSection.subCode = T_SystemUser.subCode";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            if (dt!=null && dt.Rows.Count>0)
            {
                 tempStr = "";
                for (int i = 0; i < funcDt.Rows.Count; i++)
                {
                    if (i != 0) { tempStr += " or "; }
                    tempStr += " funcCode = '" + funcDt.Rows[i]["FUNCCODE"].ToString() + "'";
                }
                if (tempStr != "")
                {
                    tempStr = "(" + tempStr + ") and ";
                }
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    strSql = "select funcCode from T_SystemUserAndRolePopedom where " + tempStr + " userName='" + dt.Rows[i]["USERNAME"].ToString() + "' and type='USER'";
                    string userRoleName = "";
                    DataTable tempDt = Access.execSql(strSql);
                    if (tempDt!=null && tempDt.Rows.Count>0)
                    {
                        for(int j=0;j<tempDt.Rows.Count;j++)
                        {
                            for (int k = 0; k < funcDt.Rows.Count; k++)
                            {
                                if (funcDt.Rows[k]["KIND"].ToString() == "LANMU")
                                {
                                    if (tempDt.Rows[j]["FUNCCODE"].ToString()==funcDt.Rows[k]["FUNCCODE"].ToString())
                                    {
                                        if (userRoleName.IndexOf("栏目管理员") < 0) { userRoleName += " 栏目管理员"; }
                                    }
                                }
                                if (funcDt.Rows[k]["KIND"].ToString() == "SHENHE")
                                {
                                    if (tempDt.Rows[j]["FUNCCODE"].ToString() == funcDt.Rows[k]["FUNCCODE"].ToString())
                                    {
                                        if (userRoleName.IndexOf("审核管理员") < 0) { userRoleName += " 审核管理员"; }
                                    }
                                }
                                if (funcDt.Rows[k]["KIND"].ToString() == "FENZHAN")
                                {
                                    if (tempDt.Rows[j]["FUNCCODE"].ToString() == funcDt.Rows[k]["FUNCCODE"].ToString())
                                    {
                                        if (userRoleName.IndexOf("分站管理员") < 0) { userRoleName += " 分站管理员"; }
                                    }
                                }
                            }  
                        }
                    }
                    dt.Rows[i]["USERROLENAME"] = userRoleName;
                    bool otherPopedom = CheckOtherPopedom(dt.Rows[i]["USERNAME"].ToString(), funcCodes, Access);
                    if (otherPopedom)
                    {
                        dt.Rows[i]["OTHERPOPEDOM"] = "1";
                        dt.Rows[i]["OTHERPOPEDOMNAME"] = "有";
                    }
                    else
                    {
                        dt.Rows[i]["OTHERPOPEDOM"] = "0";
                        dt.Rows[i]["OTHERPOPEDOMNAME"] = "无";
                    }
                }
            }
            Access.Dispose();

            return dt;
            }
            catch (System.Exception e)
            {
                string ss = e.ToString();
            }
            return null;
        }

        public bool CheckOtherPopedom(string userName,FluorineFx.AMF3.ArrayCollection funcCodes)
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            bool result = CheckOtherPopedom(userName, funcCodes, Access);
            Access.Dispose();
            return result;
        }

        public string UpdateUser(string id, string oleUserName, string userName, string password, string trueName, string subCode, string remark, string updateMan,string isBindIp,string bindIp, string[] roles, FluorineFx.AMF3.ArrayCollection popedomList,FluorineFx.AMF3.ArrayCollection funcCodes,string pptr)
        {
            string result = "系统错误，保存失败。";
            /////////////////////////////////////////////////
            DataTable funcDt = new DataTable();
            DataColumn dc1 = new DataColumn("FUNCCODE");
            DataColumn dc2 = new DataColumn("KIND");
            funcDt.Columns.Add(dc1);
            funcDt.Columns.Add(dc2);
            for (int i = 0; i < funcCodes.Count; i++)
            {
                System.Collections.Hashtable uFunc = (System.Collections.Hashtable)funcCodes[i];
                DataRow dr = funcDt.NewRow();
                dr["FUNCCODE"] = uFunc["FUNCCODE"].ToString();
                dr["KIND"] = uFunc["KIND"].ToString();
                funcDt.Rows.Add(dr);
            }
            ////////////////////////////////////////////////
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            password = Com.Com.checkSql(password);
            remark = Com.Com.checkSql(remark);
            subCode = Com.Com.checkSql(subCode);
            updateMan = Com.Com.checkSql(updateMan);
            isBindIp = Com.Com.checkSql(isBindIp);
            bindIp = Com.Com.checkSql(bindIp);
            if (!new Com.IP().IpCheck(bindIp))
            {
                return "绑定IP地址不是一个有效IP";
            }
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select userName from T_SystemUser where userName='" + userName + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "用户名重复，保存失败。";
            }
            else
            {
                strSql = "update T_SystemUser set ";
                strSql += "userName='" + userName + "',";
                if (password != "")
                {
                    strSql += "password='" + new Common.EncrypeMD5().MD5(password) + "',";
                }
                strSql += "trueName='" + trueName + "',";
                strSql += "subCode='" + subCode + "',";
                strSql += "remark='" + remark + "',";
                strSql += "addMan='" + updateMan + "',";
                strSql += "addTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "isBindIp=" + isBindIp + ",";
                strSql += "BindIp = '" + bindIp + "' ";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + id;
                    if (CheckOtherPopedom(oleUserName, funcCodes, Access))
                    {
                        string tempStr = "";
                        for (int i = 0; i < funcDt.Rows.Count; i++)
                        {
                            if (i != 0) { tempStr += " or "; }
                            tempStr += " funcCode like '" + funcDt.Rows[i]["FUNCCODE"].ToString() + "%'";
                        }
                        if (tempStr != "")
                        {
                            tempStr = "(" + tempStr + ") and ";
                        }
                        Access.execSqlNoQuery1("Delete T_SystemUserAndRolePopedom where " + tempStr + "userName='" + oleUserName + "' and type='USER'");
                        Access.execSqlNoQuery1("update T_SystemUserAndRolePopedom set userName='" + userName + "' where userName='" + oleUserName + "'");             
                    }
                    else
                    {
                        Access.execSqlNoQuery1("delete T_SystemUserRole where userName='" + oleUserName + "'");
                        Access.execSqlNoQuery1("delete T_SystemUserAndRolePopedom where userName='" + oleUserName + "'");  
                    }
                    for (int i = 0; i < roles.Length; i++)
                    {
                        Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode) values('" + userName + "','" + roles[i] + "')");
                    }
                    for (int i = 0; i < popedomList.Count; i++)
                    {
                        System.Collections.Hashtable uPopdeom = (System.Collections.Hashtable)popedomList[i];
                        strSql = "if not exists(select * from T_SystemUserAndRolePopedom where type='USER' and userName='" + userName + "' and funcCode='" + uPopdeom["FUNCCODE"].ToString() + "')  ";
                        strSql += "insert into T_SystemUserAndRolePopedom(type,userName,funcCode,subCode) values('USER','" + userName + "','" + uPopdeom["FUNCCODE"].ToString() + "','" + uPopdeom["SUBCODE"].ToString() + "')";
                        Access.execSqlNoQuery1(strSql);
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("ManagementUser.DeleteUser")]
        public DataTable DeleteUser(FluorineFx.AMF3.ArrayCollection users,FluorineFx.AMF3.ArrayCollection funcCodes,string pptr)
        {
            /////////////////////////////////////////////////
            DataTable funcDt = new DataTable();
            DataColumn dc11 = new DataColumn("FUNCCODE");
            DataColumn dc12 = new DataColumn("KIND");
            funcDt.Columns.Add(dc11);
            funcDt.Columns.Add(dc12);
            for (int i = 0; i < funcCodes.Count; i++)
            {
                System.Collections.Hashtable uFunc = (System.Collections.Hashtable)funcCodes[i];
                DataRow dr = funcDt.NewRow();
                dr["FUNCCODE"] = uFunc["FUNCCODE"].ToString();
                dr["KIND"] = uFunc["KIND"].ToString();
                funcDt.Rows.Add(dr);
            }
            ////////////////////////////////////////////////
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("USERNAME");
            DataColumn dc3 = new DataColumn("TRUENAME");
            DataColumn dc4 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            if (users != null)
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int j = 0; j < users.Count; j++)
                {
                    System.Collections.Hashtable uInfo = (System.Collections.Hashtable)users[j];
                    if (CheckOtherPopedom(uInfo["USERNAME"].ToString(), funcCodes,Access))
                    {
                        string tempStr = "";
                        for (int i = 0; i < funcDt.Rows.Count; i++)
                        {
                            if (i != 0) { tempStr += " or "; }
                            tempStr += " funcCode like '" + funcDt.Rows[i]["FUNCCODE"].ToString() + "%'";
                        }
                        if (tempStr != "")
                        {
                            tempStr = "(" + tempStr + ") and ";
                        }
                        string strSql = "Delete T_SystemUserAndRolePopedom where " + tempStr + " userName='" + uInfo["USERNAME"] + "' and type='USER'";
                        if (Access.execSqlNoQuery1(strSql))
                        {
                            DataTable tempDt = Access.execSql("select * from T_SystemUserAndRolePopedom where userName='" + uInfo["USERNAME"] + "' and type='USER' and funcCode like '" + pptr + "%' and funcCode<>'" + pptr + "'");
                            if (tempDt != null && tempDt.Rows.Count > 0)
                            {
                            }
                            else
                            {
                                Access.execSqlNoQuery1("Delete T_SystemUserAndRolePopedom where funcCode='" + pptr + "' and userName='" + uInfo["USERNAME"] + "' and type='USER'");
                                Access.execSql("Delete T_SystemUser where userName='"+uInfo["USERNAME"]+"'");
                            }
                            DataRow dr = dt.NewRow();
                            dr["ID"] = uInfo["ID"].ToString();
                            dr["USERNAME"] = uInfo["USERNAME"].ToString();
                            dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                            dr["result"] = "0";
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["ID"] = uInfo["ID"].ToString();
                            dr["USERNAME"] = uInfo["USERNAME"].ToString();
                            dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                            dr["result"] = "失败。";
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        if (Access.execSqlNoQuery1("delete T_SystemUser where id=" + uInfo["ID"].ToString() + ""))
                        {
                            Access.execSqlNoQuery1("Delete T_SystemUserAndRolePopedom where userName='" + uInfo["USERNAME"] + "' and type='USER'");
                            Access.execSqlNoQuery1("delete T_SystemUserRole where userName='" + uInfo["USERNAME"] + "'");

                            DataRow dr = dt.NewRow();
                            dr["ID"] = uInfo["ID"].ToString();
                            dr["USERNAME"] = uInfo["USERNAME"].ToString();
                            dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                            dr["result"] = "0";
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["ID"] = uInfo["ID"].ToString();
                            dr["USERNAME"] = uInfo["USERNAME"].ToString();
                            dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                            dr["result"] = "失败。";
                            dt.Rows.Add(dr);
                        }
                    }
                }
                Access.Dispose();
            }
            return dt;
        }

        private bool CheckOtherPopedom(string userName, FluorineFx.AMF3.ArrayCollection funcCodes, DataAccess.DataAccess Access)
        {
            DataTable funcDt = new DataTable();
            DataColumn dc1 = new DataColumn("FUNCCODE");
            DataColumn dc2 = new DataColumn("KIND");
            funcDt.Columns.Add(dc1);
            funcDt.Columns.Add(dc2);
            for (int i = 0; i < funcCodes.Count; i++)
            {
                System.Collections.Hashtable uFunc = (System.Collections.Hashtable)funcCodes[i];
                DataRow dr = funcDt.NewRow();
                dr["FUNCCODE"] = uFunc["FUNCCODE"].ToString();
                dr["KIND"] = uFunc["KIND"].ToString();
                funcDt.Rows.Add(dr);
            }
            bool result = false;
            string tempStr = "";
            for(int i=0;i<funcDt.Rows.Count;i++)
            {
                if (i != 0) { tempStr += " and "; }
                tempStr += " funcCode not like '" + funcDt.Rows[i]["FUNCCODE"].ToString() + "%'";
            }
            if (tempStr!="")
            {
                tempStr = "(" + tempStr + ") and ";
            }
            string strSql = "select funcCode from T_SystemUserAndRolePopedom where " + tempStr + " userName='" + userName + "' and type='USER'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = true;
            }
            return result;
        }

        public string GetUserPopedomXML(string userName,string pptr,string moduleCode)
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>";
            DataTable subList = new Sys.SubSection().GetAllSubList();
            DataColumn dc = new DataColumn("checked");
            subList.Columns.Add(dc);
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                subList.Rows[i]["checked"] = "";
            }
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "";
            if (userName != "")
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.text,T_SystemMenu.listOrder,T_SystemUserAndRolePopedom.funcCode,T_SystemUserAndRolePopedom.subCode ";
                strSql += ",T_SystemMenu.popedomDifference,T_SystemMenu.popedomFlag ";
                strSql += " from T_SystemMenu left join T_SystemUserAndRolePopedom on T_SystemMenu.code=T_SystemUserAndRolePopedom.funcCode and T_SystemUserAndRolePopedom.type='USER'";
                strSql += " and T_SystemUserAndRolePopedom.userName='" + userName + "'";
                strSql += " where T_SystemMenu.isShow=1 and popedomMode='system'";
            }
            else
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.text,T_SystemMenu.listOrder,'' as funcCode,'' as subCode ";
                strSql += ",T_SystemMenu.popedomDifference ,T_SystemMenu.popedomFlag ";
                strSql += " from T_SystemMenu ";
                strSql += " where T_SystemMenu.isShow=1 and popedomMode='system'";
            }
            switch(pptr.ToUpper())
            {
                case "001":
                    strSql += " and (T_SystemMenu.code like '001002%' or T_SystemMenu.code like '001003%' or T_SystemMenu.Code='001')";
                    break;

                case "002":
                    strSql += " and (T_SystemMenu.code like '002002%' or T_SystemMenu.code like '002003%' or T_SystemMenu.Code='002')";
                    break;

                case "003":
                    strSql += " and (T_SystemMenu.code like '003002%' or T_SystemMenu.code like '003003%' or T_SystemMenu.code like '003004%' or T_SystemMenu.Code='003')";
                    break;

                case "004":
                    strSql += " and (T_SystemMenu.code like '004002%' or T_SystemMenu.code like '004003%' or T_SystemMenu.code like '004004%' or T_SystemMenu.Code='004')";
                    break;

                case "005":
                    if (moduleCode == "005001")
                    {
                        strSql += " and (T_SystemMenu.code like '005002%' or T_SystemMenu.code like '005003%' or T_SystemMenu.code like '005004%' or T_SystemMenu.Code='005')";
                    }
                    else if (moduleCode == "005002")
                    {
                        strSql += " and (T_SystemMenu.code like '005003%' or T_SystemMenu.code like '005004%' or T_SystemMenu.Code='005')";
                    }
                    break;

                case "006":
                    strSql += " and (T_SystemMenu.code like '006002%' or T_SystemMenu.code like '006003%' or T_SystemMenu.code like '006004%' or T_SystemMenu.Code='006')";
                    break;

                case "007":
                    if (moduleCode == "007001")
                    {
                        strSql += " and (T_SystemMenu.code like '007002%' or T_SystemMenu.Code='007')";
                    }
                    else if (moduleCode == "007002")
                    {
                        strSql += " and (T_SystemMenu.code like '007003%' or T_SystemMenu.code like '007004%' or T_SystemMenu.code like '007005%' or T_SystemMenu.Code='007')";
                    }
                    break;

                case "009":
                    strSql += " and (T_SystemMenu.code like '009002%' or T_SystemMenu.code like '009003%' or T_SystemMenu.code like '009004%' or T_SystemMenu.Code='009')";
                    break;

                case "010":
                    strSql += " and (T_SystemMenu.code like '010002%' or T_SystemMenu.code like '010003%' or T_SystemMenu.Code='010')";
                    break;

                default:
                    //strSql += " and ((T_SystemMenu.pptr like '" + pptr + "%' and (T_SystemMenu.popedomFlag='LANMU' OR T_SystemMenu.popedomFlag='SHENHE' or T_SystemMenu.popedomFlag='FENZHAN')) or T_SystemMenu.Code='" + pptr + "')";
                    break;
            }
            //////////////////////////////////////////////////////////////////////////
            /*strSql += " and (";
            for (int i = 0; i < funcCodes.Length; i++)
            {
                if (i != 0) { strSql += " or "; }
                strSql += " T_SystemMenu.code like '" + funcCodes[i] + "%'";
            }
            strSql += ")";
             */
            /////////////////////////////////////////////////////////////////////////
            DataTable dt = Access.execSql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("code='"+pptr+"'", "listOrder Asc");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {

                        bool isAllChecked = true;
                        string tempResult = GetUserPopedomXML(dt, drs[i]["code"].ToString(), subList, out isAllChecked, moduleCode);
                        result += "<item type='func' code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' popedomDifference='" + drs[i]["popedomDifference"].ToString() + "' popedomFlag='"+drs[i]["popedomFlag"].ToString()+"' ";
                        if (drs[i]["funcCode"].ToString() != "")
                        {
                            if (isAllChecked)
                            {
                                result += "state='checked'";
                            }
                            else
                            {
                                result += "state='schrodinger'";
                            }
                        }
                        else
                        {
                            result += "state='unchecked'";
                        }
                        result += " >";
                        result += tempResult;
                        result += "</item>";
                    }
                }
            }
            Access.Dispose();
            result += "</root>";
            return result;
        }
        private string GetUserPopedomXML(DataTable dt, string pptr, DataTable subList, out bool isAllChecked,string moduleCode)
        {
            string result = "";
            isAllChecked = true;
            DataRow[] pptrDrs = dt.Select("code='" + pptr + "'");
            DataRow[] drs = dt.Select("pptr='" + pptr + "'", "listOrder Asc");
            if (pptrDrs != null && pptrDrs.Length > 0 && drs != null && drs.Length > 0)
            {
                isAllChecked = true;
                if (pptrDrs[0]["popedomDifference"].ToString().ToUpper() == "GLOBALANDSUBSITE")
                {
                    DataTable tempDt = subList.Copy();
                    DataRow dr = tempDt.NewRow();
                    dr["SUBCODE"] = "global";
                    dr["SUBNAME"] = "全局";
                    tempDt.Rows.InsertAt(dr, 0);
                    /////////////////////////////////////////////////////////////////////////////////////
                    string divisions = "";
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (drs[i]["subCode"].ToString() != "")
                        {
                            divisions = drs[i]["subCode"].ToString();
                            break;
                        }
                    }
                    if (divisions != "")
                    {
                        string[] divs = divisions.Split('|');
                        for (int i = 0; i < divs.Length; i++)
                        {
                            for (int j = 0; j < tempDt.Rows.Count; j++)
                            {
                                if (divs[i].ToString().ToUpper() == tempDt.Rows[j]["subCode"].ToString().ToUpper())
                                {
                                    tempDt.Rows[j]["checked"] = "checked";
                                    break;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////
                    result += AddDivision(tempDt, drs[0]["pptr"].ToString(), pptrDrs[0]["popedomDifference"].ToString(), out isAllChecked, moduleCode);
                }
                else if (pptrDrs[0]["popedomDifference"].ToString().ToUpper() == "SUBSITE")
                {
                    DataTable tempDt = subList.Copy();
                    /////////////////////////////////////////////////////////////////////////////////////
                    string divisions = "";
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (drs[i]["subCode"].ToString() != "")
                        {
                            divisions = drs[i]["subCode"].ToString();
                            break;
                        }
                    }
                    if (divisions != "")
                    {
                        string[] divs = divisions.Split('|');
                        for (int i = 0; i < divs.Length; i++)
                        {
                            for (int j = 0; j < tempDt.Rows.Count; j++)
                            {
                                if (divs[i].ToString().ToUpper() == tempDt.Rows[j]["subCode"].ToString().ToUpper())
                                {
                                    tempDt.Rows[j]["checked"] = "checked";
                                    break;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////
                    result += AddDivision(tempDt, drs[0]["pptr"].ToString(), pptrDrs[0]["popedomDifference"].ToString(), out isAllChecked, moduleCode);
                }

                for (int i = 0; i < drs.Length; i++)
                {
                    result += "<item type='func' code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' popedomDifference='" + drs[i]["popedomDifference"].ToString() + "' popedomFlag='" + drs[i]["popedomFlag"].ToString() + "' ";

                    //isAllChecked = true;
                    string tempResult = GetUserPopedomXML(dt, drs[i]["code"].ToString(), subList, out isAllChecked, moduleCode);
                    if (drs[i]["funcCode"].ToString() != "")
                    {
                        if (isAllChecked)
                        {
                            result += "state='checked'";
                        }
                        else
                        {
                            result += "state='schrodinger'";
                            isAllChecked = false;
                        }
                    }
                    else
                    {
                        result += "state='unchecked'";
                        isAllChecked = false;
                    }

                    result += " >";
                    result += tempResult;
                    result += "</item>";
                }
            }
            return result;
        }

        private string AddDivision(DataTable subList, string code, string popedomDifference, out bool isAllChecked, string moduleCode)
        {
            string result = "";
            bool isChecked = false;
            isAllChecked = true;
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                if (subList.Rows[i]["checked"].ToString() != "")
                {
                    if (!isChecked) { isChecked = true; }
                }
                else
                {
                    if (isAllChecked) { isAllChecked = false; }
                }
            }
            if (isChecked)
            {
                if (isAllChecked)
                {
                    result = "<item type='Division' code='" + code + "' text='管理的站点' state='checked' popedomDifference='" + popedomDifference + "' popedomFlag='' >";
                }
                else
                {
                    result = "<item type='Division' code='" + code + "' text='管理的站点' state='schrodinger' popedomDifference='" + popedomDifference + "' popedomFlag='' >";
                }
            }
            else
            {
                result = "<item type='Division' code='" + code + "' text='管理的站点' state='unchecked' popedomDifference='" + popedomDifference + "' popedomFlag='' >";
            }
            switch(moduleCode)
            {
                case "005001":
                    if (code!="005002")
                    {   //只显示全局的信息维护和审核
                        result += "<item type='division' code='" + subList.Rows[0]["SUBCODE"].ToString() + "' text='" + subList.Rows[0]["SUBNAME"].ToString() + "' popedomDifference='" + popedomDifference + "' popedomFlag='' ";
                        if (subList.Rows[0]["checked"].ToString() != "")
                        {
                            result += "state='checked' ";
                        }
                        else
                        {
                            result += "state='unchecked' ";

                        }
                        result += "></item>";
                    }
                    else
                    {
                        for (int i = 1; i < subList.Rows.Count; i++) //去掉全局信息发布和审核
                        {
                            result += "<item type='division' code='" + subList.Rows[i]["SUBCODE"].ToString() + "' text='" + subList.Rows[i]["SUBNAME"].ToString() + "' popedomDifference='" + popedomDifference + "' popedomFlag='' ";
                            if (subList.Rows[i]["checked"].ToString() != "")
                            {
                                result += "state='checked' ";
                            }
                            else
                            {
                                result += "state='unchecked' ";

                            }
                            result += "></item>";
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < subList.Rows.Count; i++)
                    {
                        result += "<item type='division' code='" + subList.Rows[i]["SUBCODE"].ToString() + "' text='" + subList.Rows[i]["SUBNAME"].ToString() + "' popedomDifference='" + popedomDifference + "' popedomFlag='' ";
                        if (subList.Rows[i]["checked"].ToString() != "")
                        {
                            result += "state='checked' ";
                        }
                        else
                        {
                            result += "state='unchecked' ";

                        }
                        result += "></item>";
                    }
                    break;
            }
            
            result += "</item>";
            return result;
        }

        [DataTableType("ManagementUser.GetUserSelectList")]
        public DataTable GetUserSelectList(FluorineFx.AMF3.ArrayCollection funcCodes)
        {
            DataTable funcDt = new DataTable();
            DataColumn dc1 = new DataColumn("FUNCCODE");
            DataColumn dc2 = new DataColumn("KIND");
            funcDt.Columns.Add(dc1);
            funcDt.Columns.Add(dc2);
            for (int i = 0; i < funcCodes.Count; i++)
            {
                System.Collections.Hashtable uFunc = (System.Collections.Hashtable)funcCodes[i];
                DataRow dr = funcDt.NewRow();
                dr["FUNCCODE"] = uFunc["FUNCCODE"].ToString();
                dr["KIND"] = uFunc["KIND"].ToString();
                funcDt.Rows.Add(dr);
            }
            string tempStr = "";
            for (int i = 0; i < funcDt.Rows.Count; i++)
            {
                if (i != 0) { tempStr += " or "; }
                tempStr += " T_SystemUserAndRolePopedom.funcCode like '" + funcDt.Rows[i]["FUNCCODE"].ToString() + "%' ";
            }
            if (tempStr != "")
            {
                tempStr = " and (" + tempStr + ")";
            }
            string strSql = "SELECT  T_SystemUser.ID,T_SystemUser.USERNAME, T_SystemUser.TRUENAME, T_SystemUser.SUBCODE,T_SystemUser.REMARK, T_SystemUser.ADDMAN, ";
            strSql += "              DATEADD(Hour, - 8, T_SystemUser.addTime)   AS ADDTIME, T_SubSection.SUBNAME,T_User.userName as joinUser";
            strSql += "      FROM   T_SystemUser ";
            strSql += "      left join T_SubSection on T_SubSection.subCode=T_SystemUser.subCode ";
            strSql += "      left join  ";
            strSql += "            (select userName from  (SELECT  T_SystemUser_1.userName";
            strSql += "                        FROM   T_SystemUser AS T_SystemUser_1";
            strSql += "                               INNER JOIN T_SystemUserAndRolePopedom ON T_SystemUserAndRolePopedom.userName = T_SystemUser_1.userName ";
            strSql += "                                                                        AND  T_SystemUserAndRolePopedom.type = 'user' and T_SystemUser_1.superior=0 ";
            strSql += "                                                                        " + tempStr + " ) AS derivedtbl_1";
            strSql += "                       GROUP BY userName)";
            strSql += "                       AS T_User ";
            strSql += "                 on T_User.userName=T_SystemUser.userName ";
            strSql += "      where T_User.userName is null";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("ManagementUser.GetLANMUandSHENHECode")]
        public DataTable GetLANMUandSHENHECode(string pptr,string moduleCode)
        {
            string strSql = "";
            switch(moduleCode)
            {
                case "001001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='001002' or code='001003'";
                    break;

                case "002001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='001002' or code='001003'";
                    break;

                case "003001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='003002' or code='003003' or code='003004'";
                    break;

                case "004001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='004002' or code='004003' or code='004004'";
                    break;

                case "005001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='005002' or code='005003' or code = '005004'";
                    break;

                case "005002":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='005003' or code='005004'";
                    break;

                case "006001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='006002' or code='006003' or code='006004'";
                    break;

                case "007001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='007002'";
                    break;

                case "007002":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='007002' or code='007003' or code='007004' or code='007005'";
                    break;

                case "009001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='009002' or code='009003' or code='009004'";
                    break;

                case "010001":
                    strSql += "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where code='010002' or code='010003'";
                    break;
            }
            //string strSql = "select CODE AS FUNCCODE,popedomFlag as KIND from T_SystemMenu where pptr='" + pptr + "' and (popedomFlag='LANMU' or popedomFlag='SHENHE' or  popedomFlag='FENZHAN')";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
    }
}
