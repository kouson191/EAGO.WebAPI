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
    public class LIKP_SQL
    {


        public string delete(string ZZVBELN)
        {
             
            string rslt = "0";
            rslt = DbHelperSQL.ExecuteSql("Update tbl_likp set DELFLAG = '1',DELDATE = getdate() where ZZVBELN = '" + ZZVBELN + "' and DELFLAG  is null  ").ToString();

            return rslt;
        }

        /// <summary>
        /// 发送SAP
        /// </summary>
        /// <param name="ZZVBELN">申请交货单号</param>
        /// <returns></returns>
        public string send(string ZZVBELN)
        {
            string rslt = "0";
            rslt = DbHelperSQL.ExecuteSql("Update tbl_likp set SENDFLAG = '1',SENDDATE = getdate() where ZZVBELN = '" + ZZVBELN + "' and SENDFLAG <> '1' ").ToString();

            if (rslt == "1")
            {
                List<EAGO.Models.LIKP> likp = new List<EAGO.Models.LIKP>();
                List<EAGO.Models.LIPS> lips = new List<EAGO.Models.LIPS>();
                string sqlstr = getsql() + " and ZZVBELN = '" + ZZVBELN + "' ";
                DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    likp = ConvertTo(dt);
                }

                LIPS_SQL objlips = new LIPS_SQL();
                lips = objlips.getlips(ZZVBELN);

                LIKP_SAP likpsap = new LIKP_SAP();
                rslt = likpsap.sendSAP(likp, lips);
            }

            return rslt.ToString();
        }


        public string getsql()
        {
            string sqlstr = "";
            sqlstr = sqlstr + "SELECT GUID ";
            sqlstr = sqlstr + "      ,ZZVBELN ";
            sqlstr = sqlstr + "      ,VBELN ";
            sqlstr = sqlstr + "      ,ZVBELN ";
            sqlstr = sqlstr + "      ,CONVERT(varchar(100),ZERDAT, 23) ZERDAT";
            sqlstr = sqlstr + "      ,CONVERT(varchar(100),LFART, 23) LFART";
            sqlstr = sqlstr + "      ,VSTEL ";
            sqlstr = sqlstr + "      ,VKORG ";
            sqlstr = sqlstr + "      ,CONVERT(varchar(100),WADAT, 23) WADAT";
            sqlstr = sqlstr + "      ,KUNNR ";
            sqlstr = sqlstr + "      ,TOTAL ";
            sqlstr = sqlstr + "      ,ZZCHHAO ";
            sqlstr = sqlstr + "      ,SQR ";
            sqlstr = sqlstr + "      ,SENDFLAG ";
            sqlstr = sqlstr + "      ,CONVERT(varchar(100),SENDDATE, 23) SENDDATE";
            sqlstr = sqlstr + "      ,SHFLAG ";
            sqlstr = sqlstr + "      ,CONVERT(varchar(100),SHDATE, 23) SHDATE";
            sqlstr = sqlstr + "      ,ADDRESS ";
            sqlstr = sqlstr + "      ,REMARK ";
            sqlstr = sqlstr + "  FROM  tbl_likp  where 1 = 1   and DELFLAG is null  ";
            return sqlstr;

        }

        /// <summary>
        /// 保存发货申请明细
        /// </summary>
        /// <param name="LIPS"></param>
        /// <returns></returns>
        public string savedtl(List<EAGO.Models.LIPS> LIPS)
        {

            string lipsguid = System.Guid.NewGuid().ToString();
            string FGUID = "";
            string ZZVBELN = "";
            string sqlstr = "";
            List<string> sqllist = new List<string>();

            foreach (EAGO.Models.LIPS lips in LIPS)
            {
                if (string.IsNullOrEmpty(FGUID))
                {
                    sqlstr = " select GUID from tbl_likp where ZZVBELN = '" + lips.ZZVBELN + "'   ";
                    ZZVBELN = lips.ZZVBELN;
                    DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];
                    if (dt.Rows.Count > 0) FGUID = dt.Rows[0][0].ToString();
                }

                lips.MONEY = lips.CONFIRM_NUM * lips.PRICE;

                sqlstr = " INSERT INTO tbl_lips ";
                sqlstr = sqlstr + "(GUID";
                sqlstr = sqlstr + ",FGUID";
                sqlstr = sqlstr + ",ZZVBELN";
                sqlstr = sqlstr + ",POSNR";
                sqlstr = sqlstr + ",MATNR";
                sqlstr = sqlstr + ",MAKTX";
                sqlstr = sqlstr + ",MEINS";
                sqlstr = sqlstr + ",NEED";
                //sqlstr = sqlstr + ",CONFIRM_NUM";
                //sqlstr = sqlstr + ",LFIMG";
                sqlstr = sqlstr + ",PRICE";
                sqlstr = sqlstr + ",MONEY)";

                sqlstr = sqlstr + " values( ";
                sqlstr = sqlstr + "'" + lipsguid + "',";
                sqlstr = sqlstr + "'" + FGUID + "',";
                sqlstr = sqlstr + "'" + lips.ZZVBELN + "',";
                sqlstr = sqlstr + "'" + lips.POSNR + "',";
                sqlstr = sqlstr + "'" + lips.MATNR + "',";
                sqlstr = sqlstr + "'" + lips.MAKTX + "',";
                sqlstr = sqlstr + "'" + lips.MEINS + "',";
                sqlstr = sqlstr + "'" + lips.NEED + "',";
                //sqlstr = sqlstr + "'" + lips.CONFIRM_NUM + "',";
                //sqlstr = sqlstr + "'" + lips.LFIMG + "',";
                sqlstr = sqlstr + "'" + lips.PRICE + "',";
                sqlstr = sqlstr + "'" + lips.MONEY + "')";

                sqllist.Add(sqlstr);
            }

            //执行事务保存
            if (DbHelperSQL.ExecuteSqlTran(sqllist) == 0)
            {
                return "";
            }
            else
            {
                return ZZVBELN;
            }
        }


        public string save(EAGO.Models.LIKP likp)
        {
            string sapcode = likp.KUNNR;
            string likpguid = System.Guid.NewGuid().ToString();
            string ZZVBELN = sapcode + DateTime.Today.ToString("yyyyMMdd");
            string sqlstr = "select ZZVBELN from tbl_likp where ZZVBELN like '" + ZZVBELN + "%' order by ZZVBELN desc ";
            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];
            List<string> sqllist = new List<string>();

            if (dt.Rows.Count > 0)
            {
                int no = int.Parse(dt.Rows[0][0].ToString().Substring(sapcode.Length + 8, 3));
                ZZVBELN = ZZVBELN + (no + 1).ToString("000");
            }
            else
            {
                ZZVBELN = ZZVBELN + "001";
            }

            sqlstr = "";
            sqlstr = sqlstr + "INSERT INTO  tbl_likp ";
            sqlstr = sqlstr + "           ( GUID ";
            sqlstr = sqlstr + "           ,ZZVBELN ";
            sqlstr = sqlstr + "           ,VBELN ";
            sqlstr = sqlstr + "           ,ZVBELN ";
            sqlstr = sqlstr + "           ,ZERDAT ";
            sqlstr = sqlstr + "           ,LFART ";
            sqlstr = sqlstr + "           ,VSTEL ";
            sqlstr = sqlstr + "           ,VKORG ";
            sqlstr = sqlstr + "           ,WADAT ";
            sqlstr = sqlstr + "           ,KUNNR ";
            sqlstr = sqlstr + "           ,ZZCHHAO ";
            sqlstr = sqlstr + "           ,SQR ";
            sqlstr = sqlstr + "           ,ADDRESS ";
            sqlstr = sqlstr + "           ,REMARK ";
            sqlstr = sqlstr + "           ,SENDFLAG) ";
            sqlstr = sqlstr + "     VALUES (";
            sqlstr = sqlstr + "'" + likpguid + "',";
            sqlstr = sqlstr + "'" + ZZVBELN + "',";
            sqlstr = sqlstr + "'" + likp.VBELN + "',";
            sqlstr = sqlstr + "'" + likp.ZVBELN + "',";
            sqlstr = sqlstr + "     getdate(),";
            sqlstr = sqlstr + "'" + likp.LFART + "',";
            sqlstr = sqlstr + "'" + likp.VSTEL + "',";
            sqlstr = sqlstr + "'" + likp.VKORG + "',";
            sqlstr = sqlstr + "'" + likp.WADAT + "',";
            sqlstr = sqlstr + "'" + likp.KUNNR + "',";
            sqlstr = sqlstr + "'" + likp.ZZCHHAO + "',";
            sqlstr = sqlstr + "'" + likp.SQR + "',";
            sqlstr = sqlstr + "'" + likp.ADDRESS + "',";
            sqlstr = sqlstr + "'" + likp.REMARK + "','0' )";
            sqllist.Add(sqlstr);


            foreach (EAGO.Models.LIPS lips in likp.LIPS)
            {
                string lipsguid = System.Guid.NewGuid().ToString();

                lips.MONEY = lips.CONFIRM_NUM * lips.PRICE;

                sqlstr = " INSERT INTO tbl_lips ";
                sqlstr = sqlstr + "(GUID";
                sqlstr = sqlstr + ",FGUID";
                sqlstr = sqlstr + ",ZZVBELN";
                sqlstr = sqlstr + ",POSNR";
                sqlstr = sqlstr + ",MATNR";
                sqlstr = sqlstr + ",MAKTX";
                sqlstr = sqlstr + ",MEINS";
                sqlstr = sqlstr + ",NEED";
                //sqlstr = sqlstr + ",CONFIRM_NUM";
                //sqlstr = sqlstr + ",LFIMG";
                sqlstr = sqlstr + ",PRICE";
                sqlstr = sqlstr + ",MONEY)";

                sqlstr = sqlstr + " values( ";
                sqlstr = sqlstr + "'" + lipsguid + "',";
                sqlstr = sqlstr + "'" + likpguid + "',";
                sqlstr = sqlstr + "'" + ZZVBELN + "',";
                sqlstr = sqlstr + "'" + lips.POSNR + "',";
                sqlstr = sqlstr + "'" + lips.MATNR + "',";
                sqlstr = sqlstr + "'" + lips.MAKTX + "',";
                sqlstr = sqlstr + "'" + lips.MEINS + "',";
                sqlstr = sqlstr + "'" + lips.NEED + "',";
                //sqlstr = sqlstr + "'" + lips.CONFIRM_NUM + "',";
                //sqlstr = sqlstr + "'" + lips.LFIMG + "',";
                sqlstr = sqlstr + "'" + lips.PRICE + "',";
                sqlstr = sqlstr + "'" + lips.MONEY + "')";
                sqllist.Add(sqlstr);

            }

            //执行事务保存
            if (DbHelperSQL.ExecuteSqlTran(sqllist) == 0)
            {
                return "";
            }
            else
            {
                return ZZVBELN;
            }

        }

        //ZZVBELN	 	交货申请号
        //VBELN	 	销售单号
        //BEGDATE      下单日期  ERDAT
        //ENDDATE       下单日期  ERDAT
        //SENDFLAG    发送标记
        //LFART  交货类型
        //ZVBELN  交货单号

        public List<EAGO.Models.LIKP> GetLIKP(string KUNNR, string ZZVBELN, string VBELN, string BEGDATE, string ENDDATE, string SENDFLAG, string LFART, string ZVBELN)
        {
            List<EAGO.Models.LIKP> rslt = new List<EAGO.Models.LIKP>();
            string sqlstr = getsql();


            sqlstr = sqlstr + " and KUNNR = '" + KUNNR + "' ";

            if (!string.IsNullOrEmpty(ZZVBELN))
            {
                sqlstr = sqlstr + " and ZZVBELN = '" + ZZVBELN + "' ";
            }

            if (!string.IsNullOrEmpty(VBELN))
            {
                sqlstr = sqlstr + " and VBELN = '" + VBELN + "' ";
            }

            if (!string.IsNullOrEmpty(BEGDATE))
            {
                sqlstr = sqlstr + " and CONVERT(varchar(100),ZERDAT, 23)  >= '" + BEGDATE + "' ";
            }

            if (!string.IsNullOrEmpty(ENDDATE))
            {
                sqlstr = sqlstr + " and CONVERT(varchar(100),ZERDAT, 23)  <= '" + ENDDATE + "' ";
            }

            if (!string.IsNullOrEmpty(SENDFLAG))
            {
                sqlstr = sqlstr + " and SENDFLAG = '" + SENDFLAG + "' ";
            }

            if (!string.IsNullOrEmpty(LFART))
            {
                sqlstr = sqlstr + " and LFART = '" + LFART + "' ";
            }

            if (!string.IsNullOrEmpty(ZVBELN))
            {
                sqlstr = sqlstr + " and ZVBELN = '" + ZVBELN + "' ";
            }

            sqlstr = sqlstr + " Order by  WADAT Desc, ZZVBELN desc";

            DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];

            if (dt.Rows.Count > 0)
            {
                //rslt = ConvertTo<LIKP>(dt);

                rslt = TableToList<LIKP>(dt,true);

            }

            return rslt;
        }


        public static List<EAGO.Models.LIKP> ConvertTo(DataTable table)
        {
            List<LIKP> likps = new List<LIKP>();

            if (table != null)
            { 
                foreach (DataRow row in table.Rows)
                {
                    EAGO.Models.LIKP likp = new EAGO.Models.LIKP();
                    //likp.GUID = row["GUID"].ToString();
                    likp.ZZVBELN = row["ZZVBELN"].ToString();
                    likp.VBELN = row["VBELN"].ToString();
                    likp.ZVBELN = row["ZVBELN"].ToString();
                    likp.ZERDAT = row["ZERDAT"].ToString();
                    likp.LFART = row["LFART"].ToString();
                    likp.VSTEL = row["VSTEL"].ToString();
                    likp.VKORG = row["VKORG"].ToString();
                    likp.WADAT = row["WADAT"].ToString();
                    //likp.KUNNR = row["KUNNR"].ToString();
                    likp.TOTAL = decimal.Parse( row["TOTAL"].ToString());
                    likp.ZZCHHAO = row["ZZCHHAO"].ToString();
                    likp.SQR = row["SQR"].ToString();
                    likp.SENDFLAG = row["SENDFLAG"].ToString();
                    likp.SENDDATE = row["SENDDATE"].ToString();
                    //likp.SHFLAG = row["SHFLAG"].ToString();
                    //likp.SHDATE = row["SHDATE"].ToString();
                    likp.ADDRESS = row["ADDRESS"].ToString();
                    likp.REMARK = row["REMARK"].ToString();
                    likps.Add(likp);
                }
            
            }

            return likps;
        }



        //public static List<T> ConvertTo<T>(DataTable table)
        //{
        //    if (table == null)
        //    {
        //        return null;
        //    }

        //    List<DataRow> rows = new List<DataRow>();

        //    foreach (DataRow row in table.Rows)
        //    {
        //        rows.Add(row);
        //    }

        //    return ConvertTo<T>(rows);
        //}



        /// <summary>  
        /// DataTable转化为List集合  
        /// </summary>  
        /// <typeparam name="T">实体对象</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <param name="isStoreDB">是否存入数据库datetime字段，date字段没事，取出不用判断</param>  
        /// <returns>返回list集合</returns>  
        public static List<T> TableToList<T>(DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            List<string> listColums = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties(); //集合属性数组  
                T entity = Activator.CreateInstance<T>(); //新建对象实例  
                foreach (PropertyInfo p in pArray)
                {
                    if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;  //DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环  
                    }
                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    try
                    {
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型  
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {
                        // throw;  
                    }  
                }
                list.Add(entity);
            }
            return list;
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