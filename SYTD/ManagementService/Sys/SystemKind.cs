using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using FluorineFx;

namespace ManagementService.Sys
{
    [RemotingService]
    public class SystemKind
    {
        [DataTableType("SystemKind.GetKind")]
        public DataTable GetKindList(string subCode,string kind,string pptr)
        {
            subCode = Com.Com.checkSql(subCode);
            kind = Com.Com.checkSql(kind);
            pptr = Com.Com.checkSql(pptr);

            string strSql = "select T_SystemKind.ID,T_SystemKind.SUBCODE,T_SystemKind.KIND,T_SystemKind.PPTR,T_SystemKind.CODE,T_SystemKind.TEXT,T_SystemKind.SEQUENCE";
            strSql += ",T_SystemKind.DEFAULTPIC,T_SystemKind.LISTORDER,T_SubSection.SUBNAME ";
            strSql += ",T_SystemKind.LINKORKIND,T_SystemKind.LINKURL,case T_SystemKind.LINKORKIND when 0 then '' else '链接' end as LINKORKIND_DESCRIPTION ";
            strSql += " from T_SystemKind ";
            strSql += " left join T_SubSection on T_SubSection.SubCode = T_SystemKind.SubCode ";
            strSql += " where T_SystemKind.kind='" + kind + "' and T_SystemKind.subCode='" + subCode + "' and T_SystemKind.pptr='" + pptr + "' order by T_SystemKind.subCode Asc,T_SystemKind.listOrder Asc";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("SystemKind.GetAllKind")]
        public DataTable GetAllKind(string sub,string kind,string pptr)
        {
            sub = Com.Com.checkSql(sub);
            kind = Com.Com.checkSql(kind);
            string strSql = "select TEXT,CODE from T_SystemKind where subCode='" + sub + "' and kind='" + kind + "' and pptr='" + pptr + "' order by listOrder Asc";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }


        [DataTableType("SystemKind.GetAllKindNotLink")]
        public DataTable GetAllKindNotLink(string sub, string kind, string pptr)
        {
            sub = Com.Com.checkSql(sub);
            kind = Com.Com.checkSql(kind);
            string strSql = "select TEXT,CODE from T_SystemKind where subCode='" + sub + "' and kind='" + kind + "' and pptr='" + pptr + "' and linkOrKind = 0 order by listOrder Asc";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("SystemKind.GetKindInfo")]
        public DataTable GetKindInfo(string id)
        {
            id = Com.Com.checkSql(id);
            string strSql = "select T_SystemKind.ID,T_SystemKind.SUBCODE,T_SystemKind.KIND,T_SystemKind.PPTR,T_SystemKind.CODE,T_SystemKind.TEXT,T_SystemKind.DEFAULTPIC,T_SystemKind.SEQUENCE,T_SystemKind.LISTORDER,T_SubSection.SUBNAME ";
            strSql += ",T_SystemKind.LINKORKIND,T_SystemKind.LINKURL,case T_SystemKind.LINKORKIND when 0 then '' else '链接' end as LINKORKIND_DESCRIPTION ";
            strSql += " from T_SystemKind ";
            strSql += " left join T_SubSection on T_SubSection.SubCode=T_SystemKind.SubCode ";
            strSql += " where T_SystemKind.id='" + id + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("SystemKind.AddKind")]
        public DataTable AddKind(string subCode,string kind,string pptr,string text,string kindLevel,string defaultPic,string sequence,string linkOrKind,string linkUrl)
        {
            //string result = "系统错误，保存失败。";
            DataTable resultDt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("PPTR");
            DataColumn dc3 = new DataColumn("CODE");
            DataColumn dc4 = new DataColumn("TEXT");
            DataColumn dc5 = new DataColumn("LISTORDER");
            DataColumn dc6 = new DataColumn("LINKORKIND");
            DataColumn dc7 = new DataColumn("LINKURL");
            DataColumn dc8 = new DataColumn("result");
            resultDt.Columns.Add(dc1);
            resultDt.Columns.Add(dc2);
            resultDt.Columns.Add(dc3);
            resultDt.Columns.Add(dc4);
            resultDt.Columns.Add(dc5);
            resultDt.Columns.Add(dc6);
            resultDt.Columns.Add(dc7);
            resultDt.Columns.Add(dc8);

            subCode = Com.Com.checkSql(subCode);
            kind = Com.Com.checkSql(kind);
            pptr = Com.Com.checkSql(pptr);
            text = Com.Com.checkSql(text);
            kindLevel = Com.Com.checkSql(kindLevel);
            defaultPic = Com.Com.checkSql(defaultPic);
            sequence = Com.Com.checkSql(sequence);
            linkOrKind = Com.Com.checkSql(linkOrKind);
            linkUrl = Com.Com.checkSql(linkUrl);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select text from T_SystemKind where kind='" + kind + "' and text='" + text + "' and subCode='" + subCode + "'"; ;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                //result = "该分类已存在“"+text+"”，保存失败。";
                DataRow dr = resultDt.NewRow();
                dr["ID"] = "";
                dr["PPTR"] = pptr;
                dr["CODE"] = "";
                dr["TEXT"] = text;
                dr["LISTORDER"] = "";
                dr["LINKORKIND"] = linkOrKind;
                dr["LINKURL"] = linkUrl;
                dr["result"] = "该分类已存在“" + text + "”，保存失败。";
                resultDt.Rows.Add(dr);
            }
            else
            {
                string code = SearchNodeCode(Access, kind, pptr, subCode);
                int listOrder = 1;
                tempDt = Access.execSql("select count(*)+1 from T_SystemKind where kind='" + kind + "' and pptr='" + pptr + "' and subCode='" + subCode + "'");
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    try
                    {
                        listOrder = System.Convert.ToInt32(tempDt.Rows[0][0].ToString());
                    }
                    catch
                    {
                    	
                    }
                }
                strSql = "insert into T_SystemKind(subCode,kind,pptr,code,text,listOrder,kindLevel,defaultPic,sequence,linkOrKind,linkUrl) ";
                strSql += " values('" + subCode + "','" + kind + "','" + pptr + "','" + code + "','" + text + "'," + listOrder + ",'" + kindLevel + "','" + defaultPic + "','" + sequence + "'," + linkOrKind + ",'" + linkUrl  + "')";
                if (Access.execSqlNoQuery1(strSql))
                {
                    string id = "";
                    tempDt = Access.execSql("select @@IDENTITY");
                    if (tempDt!=null && tempDt.Rows.Count>0)
                    {
                        id = tempDt.Rows[0][0].ToString();
                    }
                    //result = "OK" + id;
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = id;
                    dr["PPTR"] = pptr;
                    dr["CODE"] = code;
                    dr["TEXT"] = text;
                    dr["LISTORDER"] = listOrder;
                    dr["LINKORKIND"] = linkOrKind;
                    dr["LINKURL"] = linkUrl;
                    dr["result"] = "OK";
                    resultDt.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = "";
                    dr["PPTR"] = pptr;
                    dr["CODE"] = "";
                    dr["TEXT"] = text;
                    dr["LISTORDER"] = "";
                    dr["LINKORKIND"] = linkOrKind;
                    dr["LINKURL"] = linkUrl;
                    dr["result"] = "系统错误，保存失败。";
                    resultDt.Rows.Add(dr);
                }
            }
            Access.Dispose();
            return resultDt;
        }

        [DataTableType("SystemKind.UpdateKind")]
        public DataTable UpdateKind(string id,string subCode,string kind,string pptr,string code,string text,string listOrder,string kindLevel,string linkOrKind,string linkUrl)
        {
            DataTable resultDt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("PPTR");
            DataColumn dc3 = new DataColumn("CODE");
            DataColumn dc4 = new DataColumn("TEXT");
            DataColumn dc5 = new DataColumn("LISTORDER");
            DataColumn dc6 = new DataColumn("LINKORKIND");
            DataColumn dc7 = new DataColumn("LINKURL");
            DataColumn dc8 = new DataColumn("result");
            resultDt.Columns.Add(dc1);
            resultDt.Columns.Add(dc2);
            resultDt.Columns.Add(dc3);
            resultDt.Columns.Add(dc4);
            resultDt.Columns.Add(dc5);
            resultDt.Columns.Add(dc6);
            resultDt.Columns.Add(dc7);
            resultDt.Columns.Add(dc8);

            id = Com.Com.checkSql(id);
            subCode = Com.Com.checkSql(subCode);
            kind = Com.Com.checkSql(kind);
            pptr = Com.Com.checkSql(pptr);
            text = Com.Com.checkSql(text);
            kindLevel = Com.Com.checkSql(kindLevel);
            linkOrKind = Com.Com.checkSql(linkOrKind);
            linkUrl = Com.Com.checkSql(linkUrl);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select text from T_SystemKind where kind='" + kind + "' and subCode='" + subCode + "' and text='" + text + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                //result = "该分类已存在“" + text + "”，保存失败。";
                DataRow dr = resultDt.NewRow();
                dr["ID"] = "";
                dr["PPTR"] = pptr;
                dr["CODE"] = "";
                dr["TEXT"] = text;
                dr["LISTORDER"] = listOrder;
                dr["LINKORKIND"] = linkOrKind;
                dr["LINKURL"] = linkUrl;
                dr["result"] = "该分类已存在“" + text + "”，保存失败。";
                resultDt.Rows.Add(dr);
            }
            else
            {
                strSql = "update T_SystemKind set ";
                strSql += "subCode='" + subCode + "',";
                strSql += "text='" + text + "',";
                strSql += "linkOrKind = " + linkOrKind + ",";
                strSql += "linkUrl = '" + linkUrl + "'";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {   
                    //result = "OK" + id;
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = id;
                    dr["PPTR"] = pptr;
                    dr["CODE"] = code;
                    dr["TEXT"] = text;
                    dr["LISTORDER"] = listOrder;
                    dr["LINKORKIND"] = linkOrKind;
                    dr["LINKURL"] = linkUrl;
                    dr["result"] = "OK";
                    resultDt.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = "";
                    dr["PPTR"] = pptr;
                    dr["CODE"] = "";
                    dr["TEXT"] = text;
                    dr["LISTORDER"] = listOrder;
                    dr["LINKORKIND"] = linkOrKind;
                    dr["LINKURL"] = linkUrl;
                    dr["result"] = "系统错误，保存失败。";
                    resultDt.Rows.Add(dr);
                }
            }
            Access.Dispose();
            return resultDt;
        }

        public string PicSave(string kindId, string defaultPic)
        {
            kindId = Com.Com.checkSql(kindId);
            defaultPic = Com.Com.checkSql(defaultPic);
            string result = "系统错误，保存图片至数据库失败。";
            string strSql = "update T_SystemKind set defaultPic='" + defaultPic + "' where id=" + kindId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }


        [DataTableType("SystemKind.DeleteKind")]
        public DataTable DeleteKind(FluorineFx.AMF3.ArrayCollection kinds,String type)
        {
            type = Com.Com.checkSql(type);
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("SUBCODE");
            DataColumn dc4 = new DataColumn("KIND");
            DataColumn dc5 = new DataColumn("PPTR");
            DataColumn dc6 = new DataColumn("CODE");
            DataColumn dc7 = new DataColumn("TEXT");
            DataColumn dc8 = new DataColumn("DEFAULTPIC");
            DataColumn dc9 = new DataColumn("SEQUENCE");
            DataColumn dc10 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            dt.Columns.Add(dc7);
            dt.Columns.Add(dc8);
            dt.Columns.Add(dc9);
            dt.Columns.Add(dc10);
            try
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < kinds.Count; i++)
                {
                    System.Collections.Hashtable uKind = (System.Collections.Hashtable)kinds[i];

                    DataRow dr = dt.NewRow();
                    Boolean existSecondKind = false;
                    Boolean existArticle = false;
                    string strSql = "select * from T_ArticleList where subCode='" + uKind["SUBCODE"].ToString() + "' and articleType='"+type+"' and articleKind='" + uKind["CODE"].ToString() + "'";
                    DataTable tempDt1 = Access.execSql(strSql);
                    if (tempDt1!=null && tempDt1.Rows.Count>0)
                    {
                        existArticle = true;
                    }
                    if (existArticle)
                    {
                        dr["ID"] = uKind["ID"].ToString();
                        dr["SUBCODE"] = uKind["SUBCODE"].ToString();
                        dr["KIND"] = uKind["KIND"].ToString();
                        dr["PPTR"] = uKind["PPTR"].ToString();
                        dr["CODE"] = uKind["CODE"].ToString();
                        dr["TEXT"] = uKind["TEXT"].ToString();
                        dr["DEFAULTPIC"] = uKind["DEFAULTPIC"].ToString();
                        dr["SEQUENCE"] = uKind["SEQUENCE"].ToString();
                        dr["result"] = "该分类下有文章，不能删除。";
                    }
                    else
                    {
                        strSql = "select * from T_SystemKind where kind='" + uKind["KIND"].ToString() + "' and pptr='" + uKind["CODE"].ToString() + "'";
                        DataTable tempDt = Access.execSql(strSql);
                        if (tempDt != null && tempDt.Rows.Count > 0)
                        {
                            existSecondKind = true;
                        }
                        if (existSecondKind)
                        {
                            dr["ID"] = uKind["ID"].ToString();
                            dr["SUBCODE"] = uKind["SUBCODE"].ToString();
                            dr["KIND"] = uKind["KIND"].ToString();
                            dr["PPTR"] = uKind["PPTR"].ToString();
                            dr["CODE"] = uKind["CODE"].ToString();
                            dr["TEXT"] = uKind["TEXT"].ToString();
                            dr["DEFAULTPIC"] = uKind["DEFAULTPIC"].ToString();
                            dr["SEQUENCE"] = uKind["SEQUENCE"].ToString();
                            dr["result"] = "存在下级分类，请先删除下级分类";
                        }
                        else
                        {
                            strSql = "delete T_SystemKind where id=" + uKind["ID"].ToString();
                            if (Access.execSqlNoQuery1(strSql))
                            {
                                if (uKind["SEQUENCE"] != null || uKind["SEQUENCE"].ToString() != "")
                                {
                                    ////////删除图片
                                    string path = System.AppDomain.CurrentDomain.BaseDirectory + "UploadFile\\" + type + "\\" + uKind["SEQUENCE"].ToString();
                                    if (Directory.Exists(path))
                                    {
                                        Directory.Delete(path, true);
                                    }
                                }
                                dr["ID"] = uKind["ID"].ToString();
                                dr["SUBCODE"] = uKind["SUBCODE"].ToString();
                                dr["KIND"] = uKind["KIND"].ToString();
                                dr["PPTR"] = uKind["PPTR"].ToString();
                                dr["CODE"] = uKind["CODE"].ToString();
                                dr["TEXT"] = uKind["TEXT"].ToString();
                                dr["DEFAULTPIC"] = uKind["DEFAULTPIC"].ToString();
                                dr["SEQUENCE"] = uKind["SEQUENCE"].ToString();
                                dr["result"] = "0";

                            }
                            else
                            {
                                dr["ID"] = uKind["ID"].ToString();
                                dr["SUBCODE"] = uKind["SUBCODE"].ToString();
                                dr["KIND"] = uKind["KIND"].ToString();
                                dr["PPTR"] = uKind["PPTR"].ToString();
                                dr["CODE"] = uKind["CODE"].ToString();
                                dr["TEXT"] = uKind["TEXT"].ToString();
                                dr["DEFAULTPIC"] = uKind["DEFAULTPIC"].ToString();
                                dr["SEQUENCE"] = uKind["SEQUENCE"].ToString();
                                dr["result"] = "失败";
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
                Access.Dispose();

            }
            catch (System.Exception e)
            {
                Common.Log.LogError(e.ToString(), "");
            }
            return dt;
        }

        //递归找出未用的CODE
        private string SearchNodeCode(DataAccess.DataAccess Access,string kind, string pptr,string subCode)
        {
            return SearchNodeCode(Access,kind, pptr,1,subCode);
        }
        private string SearchNodeCode(DataAccess.DataAccess Access,string kind, string pptr, int code,string subCode)
        {
            string strCode = code.ToString();
            for (int i = code.ToString().Length; i < 3; i++)
            {
                strCode = "0" + strCode;
            }
            strCode = pptr + strCode;
            string strSql = "select code from T_SystemKind where kind='" + kind + "' and pptr='" + pptr + "' and code='" + strCode + "' and subCode='" + subCode + "'";
            //Common.Log.LogError(strSql, "");
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                strCode = SearchNodeCode(Access, kind, pptr, code + 1,subCode);
            }
            return strCode;
        }

        public bool KindSort(FluorineFx.AMF3.ArrayCollection kinds,string subCode)
        {
            if (kinds == null || kinds.Count==0)
            {
                return true;
            }
            bool result = false;
            try
            {
                DataTable tempDt = new DataTable();
                DataColumn dc1 = new DataColumn("ID");
                DataColumn dc2 = new DataColumn("SUBCODE");
                DataColumn dc3 = new DataColumn("KIND");
                DataColumn dc4 = new DataColumn("PPTR");
                DataColumn dc5 = new DataColumn("CODE");
                DataColumn dc6 = new DataColumn("TEXT");
                tempDt.Columns.Add(dc1);
                tempDt.Columns.Add(dc2);
                tempDt.Columns.Add(dc3);
                tempDt.Columns.Add(dc4);
                tempDt.Columns.Add(dc5);
                tempDt.Columns.Add(dc6);
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < kinds.Count; i++)
                {
                    System.Collections.Hashtable uKind = (System.Collections.Hashtable)kinds[i];
                    DataRow dr = tempDt.NewRow();
                    dr["ID"] = uKind["ID"].ToString();
                    dr["SUBCODE"] = uKind["SUBCODE"].ToString();
                    dr["KIND"] = uKind["KIND"].ToString();
                    dr["PPTR"] = uKind["PPTR"].ToString();
                    dr["CODE"] = uKind["CODE"].ToString();
                    dr["TEXT"] = uKind["TEXT"].ToString();
                    tempDt.Rows.Add(dr);
                }
                
                DataRow[] drs = tempDt.Select("subCode='" + subCode + "'");
                if (drs != null && drs.Length > 0)
                {
                    for (int j = 0; j < drs.Length; j++)
                    {
                        Access.execSqlNoQuery1("update T_SystemKind set listOrder = " + (j + 1).ToString() + " where id=" + drs[j]["ID"].ToString());
                    }
                }
                
                Access.Dispose();
                result = true;
            }
            catch(Exception e)
            {
                Common.Log.LogError(e.ToString(),"");
                //string tt = e.ToString();
            }
            return result;
        }
    }
}
