using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using EAGO.WebApi.Common;
using EAGO.WebApi.Log;
using EAGO.Models;
using System.Data;
using EAGO.BLL;
using Newtonsoft.Json;

namespace EAGO.WebApi.EAGO_Controllers
{
    public class OrderDtlController : ApiController
    {
        private EAGO.BLL.Orders orders = new Orders();
        private string logClass = "OrderDtl API：";
        private EAGO.WebApi.Log.Log Log; 
        private static IList<LIPS> lips = new List<LIPS> { };
        //
        // GET: /Order/


        public OrderDtlController()
        { 
            Log = new Log.Txt.TxtLog();
        } 



        [HttpGet]
        public void GetOrderdtl(string IVBELN) //ReturnBaseObject<IEnumerable<LIPS>> 
        {
            ReturnBaseObject<IEnumerable<LIPS>> returnObj = new ReturnBaseObject<IEnumerable<LIPS>>() { ReturnObject = new List<LIPS>() };
            try
            {
                //"0000105960", "", "", ""
                DataTable dt = orders.GetLIPS(IVBELN);

                lips = DataTableToList.ConvertTo<LIPS>(dt);
                var list = lips;
                returnObj.IsError = false;
                returnObj.ReturnObject = list;
                //return returnObj;
            }
            catch (Exception ex)
            {
                Log.WriteLog(logClass + ex.Message.ToString() + "\r\n", true);
                returnObj.IsError = true;
                returnObj.Error.ErrorMsg = ex.Message.ToString();
                returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
                //return returnObj;
            }
            finally
            {
                JsonpCallback(JsonConvert.SerializeObject(returnObj));

            }
        }


        protected virtual void JsonpCallback(string json)
        {
            HttpResponse Response = HttpContext.Current.Response;
            string callback = HttpContext.Current.Request["callback"];

            //如果callback是空, 就是普通的json, 否则就是jsonp
            Response.Write(callback == null ? json : string.Format("{0}({1})", callback, json));
            Response.End();
        }



        /// <summary>
        /// 获得数据列表
        /// </summary> 
        public List<LIPS> DataTableToListLIKP(DataTable dt)
        {
            List<LIPS> modelList = new List<LIPS>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LIPS model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LIPS();

                    //ZZVBELN	CHAR
                    //VBELN	CHAR
                    //ZVBELN	CHAR
                    //ZERDAT	DATS
                    //LFART	CHAR
                    //VSTEL	CHAR
                    //VKORG	CHAR
                    //WADAT	DATS
                    //KUNNR	CHAR
                    //ZZCHHAO	CHAR
                    //SQR	CHAR
                    //ERDAT	DATS
                    //TOTAL	CURR
                    //SHFLAG	CHAR
                    //SHDATE	DATS
                    //ZLFART	CHAR
                    //ADDRESS	CHAR
                    //REMARK	CHAR 

                    if (dt.Rows[n]["ZZVBELN"] != null && dt.Rows[n]["ZZVBELN"].ToString() != "")
                    {
                        model.ZZVBELN = dt.Rows[n]["ZZVBELN"].ToString();
                    }

                    modelList.Add(model);
                }
            }
            return modelList;
        }


    }
}
