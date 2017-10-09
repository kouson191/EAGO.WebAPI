using EAGO.DBUtility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using EAGO.Models;

namespace EAGO.DLL
{
    public class LIPS_SQL
    {
        /// <summary>
        /// 获取交货申请明细
        /// </summary>
        /// <param name="ZZVBELN">交货申请单号</param>
        /// <returns></returns>
        public List<EAGO.Models.LIPS> getlips(string ZZVBELN)
        {
            List<LIPS> rslt = new List<LIPS>();
            string sqlstr = "select * from tbl_lips where ZZVBELN = '" + ZZVBELN + "' ";
            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];

            if (dt.Rows.Count > 0)
            {
                rslt = ConvertTo<LIPS>(dt); 
              
            }

            return rslt;
        }

        /// <summary>
        /// 获取已申请数量
        /// </summary>
        /// <param name="VBELN">销售订单号</param>
        /// <param name="MATNR">物料编码</param>
        /// <returns></returns>
        public decimal getApplyNum(string VBELN, string MATNR)
        {
            decimal rslt = 0;
            string sqlstr = @" SELECT ISNULL( SUM(tbl_lips.CONFIRM_NUM) ,0)
                              FROM  tbl_likp  inner join tbl_lips on 
                              tbl_likp.ZZVBELN = tbl_lips.ZZVBELN 
                              WHERE tbl_likp.VBELN = '" + VBELN + "' AND tbl_lips.MATNR = '" + MATNR + @"'  ";
            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                rslt = decimal.Parse(dt.Rows[0][0].ToString());
            }

            return rslt;
        }



        public static List<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static List<T> ConvertTo<T>(List<DataRow> rows)
        {
            List<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }

    }
}