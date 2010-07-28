using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Sys
{
    [RemotingService]
    public class PublishType
    {
        [DataTableType("PublishType")]
        public DataTable GetKindList(string category)
        {
            string strSql = "select ID,NAME from PublishType WHERE Category=" + category;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddKind(string name,string category)
        {
            string result = "系统错误，保存失败。";
            name = Com.Com.checkSql(name);
            category = Com.Com.checkSql(category);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select * from PublishType where name='" + name + "' and category=" + category;
            DataTable dt = Access.execSql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                result = "类别名称重复。";
            }
            else
            {
                strSql = "insert into publishType(name,category) values('" + name + "'," + category + ")";
                if (Access.execSqlNoQuery1(strSql))
                {
                    dt = Access.execSql("select @@IDENTITY");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = "OK" + dt.Rows[0][0].ToString();
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateKind(string id, string name, string category)
        {
            string result = "系统错误，保存失败。";
            id = Com.Com.checkSql(id);
            name = Com.Com.checkSql(name);
            category = Com.Com.checkSql(category);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select * from PublishType where name='" + name + "' and category=" + category + " and id<>" + id;
            DataTable dt = Access.execSql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                result = "类别名称重复。";
            }
            else
            {
                strSql = "update publishType set name='" + name + "' where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + id;
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("PublishType.DeleteKind")]
        public DataTable DeleteKind(FluorineFx.AMF3.ArrayCollection kinds)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("NAME");
            DataColumn dc3 = new DataColumn("result");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);

            for (int i = 0; i < kinds.Count; i++)
            {
                System.Collections.Hashtable uKind = (System.Collections.Hashtable)kinds[i];
                DataRow dr = dt.NewRow();
                dr["ID"] = uKind["ID"].ToString();
                dr["NAME"] = uKind["NAME"].ToString();
                dr["result"] = "";
                dt.Rows.Add(dr);
            }

            string strSql = "";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strSql = "delete PublishType where id=" + dt.Rows[i]["ID"].ToString();
                if (Access.execSqlNoQuery1(strSql))
                {
                    dt.Rows[i]["result"] = "0";
                }
                else
                {
                    dt.Rows[i]["result"] = "删除失败。";
                }
            }
            Access.Dispose();
            return dt;
        }
    }
}
