using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    /// <summary>
    ///ClsProcedureParameter 的摘要说明
    /// </summary>
    public class ClsProcedureParameter
    {
        #region Member Variables
        private System.Collections.ArrayList objList = new ArrayList();
        #endregion

        //public string a;
        #region Medthod
        /// <summary>
        /// fill procedure access Parameter
        /// </summary>
        /// <param name="command"> command </param>		
        internal void FillParameter(SqlCommand command)
        {
            int i;
            for (i = 0; i < objList.Count; ++i)
            {
                //a=objList[i].ToString();
                command.Parameters.Add((SqlParameter)objList[i]);
            }
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, System.Guid ObjValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.UniqueIdentifier);
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, bool ObjValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Bit);
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, DateTime ObjValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.DateTime);
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="String"> value  </param>
        public void AddValue(string strName, string strValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.VarChar);
            objPara.Value = strValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="int"> value  </param>
        public void AddValue(string strName, int iValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Int);
            objPara.Value = iValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, decimal dclValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Decimal);
            objPara.Value = dclValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, double dblValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Float);
            objPara.Value = dblValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, float fltValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Float);
            objPara.Value = fltValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Int64"> value  </param>
        public void AddValue(string strName, Int64 i64Value)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.BigInt);
            objPara.Value = i64Value;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="char"> value  </param>
        public void AddValue(string strName, char cvalue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Char);
            objPara.Value = cvalue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="byte[]"> value  </param>
        public void AddValue(string strName, byte[] aryValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Binary);
            objPara.Value = aryValue;
            objList.Add(objPara);
        }

        #region  addvalue output
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, System.Guid ObjValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.UniqueIdentifier);
            objPara.Direction = direct;
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, bool ObjValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Bit);
            objPara.Direction = direct;
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="datetime"> value  </param>
        public void AddValue(string strName, DateTime ObjValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.DateTime);
            objPara.Direction = direct;
            objPara.Value = ObjValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="String"> value  </param>
        public void AddValue(string strName, string strValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.VarChar);
            objPara.Direction = direct;
            objPara.Value = strValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="int"> value  </param>
        public void AddValue(string strName, int iValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Int);
            objPara.Direction = direct;
            objPara.Value = iValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, decimal dclValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Decimal);
            objPara.Direction = direct;
            objPara.Value = dclValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, double dblValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Float);
            objPara.Direction = direct;
            objPara.Value = dblValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Decimal"> value  </param>
        public void AddValue(string strName, float fltValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Float);
            objPara.Direction = direct;
            objPara.Value = fltValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="Int64"> value  </param>
        public void AddValue(string strName, Int64 i64Value, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.BigInt);
            objPara.Direction = direct;
            objPara.Value = i64Value;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="char"> value  </param>
        public void AddValue(string strName, char cvalue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Char);
            objPara.Direction = direct;
            objPara.Value = cvalue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="byte[]"> value  </param>
        public void AddValue(string strName, byte[] aryValue, ParameterDirection direct)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Binary);
            objPara.Direction = direct;
            objPara.Value = aryValue;
            objList.Add(objPara);
        }
        #endregion



        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="byte[]"> value  </param>
        public void AddImg(string strName, byte[] aryValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Image);
            objPara.Value = aryValue;
            objList.Add(objPara);
        }
        /// <summary>
        /// Add Procedure Parameter value
        /// </summary>
        /// <param name="String"> parameter name  </param>
        /// <param name="byte[]"> value  </param>
        public void AddText(string strName, byte[] aryValue)
        {
            SqlParameter objPara = new SqlParameter(strName, SqlDbType.Text);
            objPara.Value = aryValue;
            objList.Add(objPara);
        }
        #endregion
    }
}
