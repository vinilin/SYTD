using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class DataAccess
    {
        SqlConnection conn = new SqlConnection(new Common.SysConfig().GetConnString());	//取得连接
        /// <summary>
        /// 打开数据连接
        /// </summary>
        public DataAccess()
        {
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), "Access打开数据库连接");
            }
        }
        /// <summary>
        /// 关闭数据连接并释放内存
        /// </summary>
        public void Dispose()
        {
            try
            {
                conn.Close();
                conn.Dispose();
            }
            catch { }
        }
        #region procedure

        public DataTable ExecuteQueryByPage(ClsProcedureParameter objPara)
        {
            SqlCommand command = conn.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable tmpTable = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "commonPage";
                if (objPara != null)
                {
                    objPara.FillParameter(command);
                }
                tmpTable = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(tmpTable);

            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), "commonMyPage" + ",ExecuteQueryByPage");
            }
            finally
            {
                command.Dispose();
            }
            return tmpTable;

        }

        /// <summary>
        /// 执行有参存储过程返回结果集
        /// </summary>
        /// <param name="strProcedureName">存储过程名字</param>
        /// <param name="objPar">存储过程参数对象</param>
        /// <returns>DATATable</returns>
        public DataTable ExecQuery(string strProcedureName, ClsProcedureParameter objPar)
        {
            SqlCommand command = conn.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable tmpTable = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                if (objPar != null)
                {
                    objPar.FillParameter(command);	//填充command
                }
                tmpTable = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(tmpTable);
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行带参数查询");
            }
            finally
            {
                command.Dispose();
                adapter.Dispose();
            }
            return tmpTable;
        }
        /// <summary>
        /// 执行无参存储过程返回结果集
        /// </summary>
        /// <param name="strProcedureName">存储过程名字</param>
        /// <returns>DataTable</returns>
        public DataTable ExecQuery(string strProcedureName)
        {
            SqlCommand command = conn.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable tmpTable = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                tmpTable = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(tmpTable);
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行不带参数查询");
            }
            finally
            {
                command.Dispose();
                adapter.Dispose();
            }
            return tmpTable;
        }

        /// <summary>
        /// 执行insert等操作的存储过程
        /// </summary>
        /// <param name="strProcedureName">存储过程名字</param>
        /// <param name="objPar">存储过程参数对象</param>
        /// <returns>bool</returns>
        public bool ExecNoQuery(string strProcedureName, ClsProcedureParameter objPar)
        {
            SqlCommand command = conn.CreateCommand();
            bool Result = false;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                if (objPar != null)
                {
                    objPar.FillParameter(command);
                }
                command.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行带参数非查询");
            }
            finally
            {
                command.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 执行insert等操作的存储过程
        /// </summary>
        /// <param name="strProcedureName">存储过程名字</param>
        /// <returns>bool</returns>
        public bool ExecNoQuery(string strProcedureName)
        {
            SqlCommand command = conn.CreateCommand();
            bool Result = false;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                command.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行不带参数非查询");
            }
            finally
            {
                command.Dispose();
            }
            return Result;
        }

        public object[] ExecCollect(string strProcedureName, ClsProcedureParameter objPar)
        {
            SqlCommand command = conn.CreateCommand();
            SqlDataReader reader = null;
            object[] objrtn = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                if (objPar != null)
                {
                    objPar.FillParameter(command);
                }
                reader = command.ExecuteReader();
                if (reader == null)
                {
                    objrtn = null;
                }
                if (reader.Read())
                {
                    if (reader.FieldCount < 1)
                    {
                        objrtn = null;
                    }
                    else
                    {
                        objrtn = new object[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            objrtn[i] = reader.GetValue(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行带参数非查询返回对象");
            }
            finally
            {
                reader.Close();
                command.Dispose();
            }
            return objrtn;
        }
        public object[] ExecCollect(string strProcedureName)
        {
            SqlCommand command = conn.CreateCommand();
            SqlDataReader reader = null;
            object[] objrtn = null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = strProcedureName;
                reader = command.ExecuteReader();
                if (reader == null)
                {
                    objrtn = null;
                }
                if (reader.Read())
                {
                    if (reader.FieldCount < 1)
                    {
                        objrtn = null;
                    }
                    else
                    {
                        objrtn = new object[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            objrtn[i] = reader.GetValue(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strProcedureName + ",Access执行不带参数非查询返回对象");
            }
            finally
            {
                reader.Close();
                command.Dispose();
            }
            return objrtn;
        }
        #endregion

        #region ExecSql
        /// <summary>
        /// 执行SQL语句 并返回结果集
        /// </summary>
        /// <param name="strSql">SQL语句：SELECT为主</param>
        /// <returns>DataTable</returns>
        public DataTable execSql(string strSql)
        {
            SqlCommand command;
            DataTable tempData = null;
            SqlDataAdapter adapter;
            command = conn.CreateCommand();
            try
            {

                //command.CommandTimeout = COMMAND_TIMEOUT;
                command.CommandType = CommandType.Text;
                command.CommandText = strSql;
                //command.Transaction = Transaction;
                adapter = new SqlDataAdapter();
                tempData = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(tempData);
                //return tempData;
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strSql + ",Access执行SQL查询返回DataTable");
            }
            finally
            {
                command.Dispose();
            }
            return tempData;
        }
        /// <summary>
        /// 执行SQL语句 不返回结果集
        /// </summary>
        /// <param name="strSql">SQL语句：INSERT等</param>				
        public void execSqlNoQuery(string strSql)
        {
            SqlCommand command;
            command = conn.CreateCommand();
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = strSql;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strSql + ",Access执行SQL查询");
            }
            finally
            {
                command.Dispose();
            }
        }
        /// <summary>
        /// 执行SQL语句 返回操作结果
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>bool</returns>
        public bool execSqlNoQuery1(string strSql)
        {
            bool result = false;
            SqlCommand command;
            command = conn.CreateCommand();
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = strSql;
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), strSql + ",Access执行SQL查询");
            }
            finally
            {
                command.Dispose();
            }
            return result;
        }
        #endregion


    }
}