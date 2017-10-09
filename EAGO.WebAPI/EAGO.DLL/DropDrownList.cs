using EAGO.DBUtility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using EAGO.Models;
using SAP.Middleware.Connector;

namespace EAGO.DLL
{
    public class DropDrownList
    {
        public List<string> getValueList(string type)
        {
            List<string> rslt = new List<string>();
            string sqlstr = "SELECT  value  FROM tbl_dropdown_list where type = '" + type + "' order by  value";
            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rslt.Add(dr[0].ToString());
                }
            }

            return rslt;
        }

        public List<EAGO.Models.DropDrownList> getDropDrownList(string type)
        {
            List<EAGO.Models.DropDrownList> rslt = new List<EAGO.Models.DropDrownList>();

            ////插入空行
            //EAGO.Models.DropDrownList ddl0 = new EAGO.Models.DropDrownList();
            //ddl0.TEXT = "";
            //ddl0.VALUE = "";
            //rslt.Add(ddl0);

            string sqlstr = "SELECT  *  FROM tbl_dropdown_list where type = '" + type + "' order by  value";
            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    EAGO.Models.DropDrownList ddl = new EAGO.Models.DropDrownList();

                    ddl.TEXT = dr["TEXT"].ToString();
                    ddl.VALUE = dr["VALUE"].ToString();
                    rslt.Add(ddl);
                }
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
