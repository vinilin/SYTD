using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ManagementService.Com
{
    public class CommonPage
    {
        public DataTable CommonQueryPage(string tblName,
                                         string fieldCollections,
                                         string strWhere,
                                         string joinConditions,
                                         string orderField,
                                         int orderType,
                                         int PageSize,
                                         int PageIndex)
        {
            DataAccess.ClsProcedureParameter objPara = new DataAccess.ClsProcedureParameter();
            objPara.AddValue("@tblName", tblName);
            objPara.AddValue("@fieldCollections", fieldCollections);
            objPara.AddValue("@PageSize", PageSize);
            objPara.AddValue("@PageIndex", PageIndex);
            objPara.AddValue("@IsCount", 1);
            objPara.AddValue("@orderField", orderField);
            objPara.AddValue("@OrderType", orderType);
            objPara.AddValue("@strWhere", strWhere);
            objPara.AddValue("@joinConditions", joinConditions);

            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.ExecQuery("CommonMyPage", objPara);
            if (dt != null && dt.Rows.Count > 0)
            {
                string recordCount = dt.Rows[0][0].ToString();
                int pageCount = ((int)(System.Convert.ToInt32(recordCount) / PageSize)) + 1;
                if (pageCount <= PageIndex)
                {
                    PageIndex = pageCount - 1;
                }
                fieldCollections = recordCount + " as recordCount," + fieldCollections;
                DataAccess.ClsProcedureParameter objPara1 = new DataAccess.ClsProcedureParameter();
                objPara1.AddValue("@tblName", tblName);
                objPara1.AddValue("@fieldCollections", fieldCollections);
                objPara1.AddValue("@PageSize", PageSize);
                objPara1.AddValue("@PageIndex", PageIndex);
                objPara1.AddValue("@IsCount", 0);
                objPara1.AddValue("@orderField", orderField);
                objPara1.AddValue("@OrderType", orderType);
                objPara1.AddValue("@strWhere", strWhere);
                objPara1.AddValue("@joinConditions", joinConditions);
                dt = Access.ExecQuery("CommonMyPage", objPara1);

            }
            Access.Dispose();
            return dt;
        }
    }
}