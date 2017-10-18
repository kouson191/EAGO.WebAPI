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
    /// <summary>
    /// 销售订单
    /// </summary>
    [RequestAuthorize] //票据验证特征
    public class OrderController : ApiController
    {
        private EAGO.BLL.Orders orders = new Orders();
        private string logClass = "Order API：";
        private EAGO.WebApi.Log.Log Log;
        private static IList<LIKP> likp = new List<LIKP> { };
        private static IList<LIPS> lips = new List<LIPS> { };

        /// <summary>
        /// 
        /// </summary>
        public OrderController()
        { 
            Log = new Log.Txt.TxtLog();
        }



        /// <summary>
        /// 获取销售订单列表
        /// </summary>
        /// <param name="IKUNNR">客户编号</param>
        /// <param name="IVBELN">订单号</param>
        /// <param name="IBEGIN">开始日期</param>
        /// <param name="IEND">结束日期</param>
        [HttpGet]
        public ReturnBaseObject<IEnumerable<LIKP>> GetAllOrderHead(string IKUNNR, string IVBELN, string IBEGIN, string IEND)
        {
            ReturnBaseObject<IEnumerable<LIKP>> returnObj = new ReturnBaseObject<IEnumerable<LIKP>>() { ReturnObject = new List<LIKP>() };
            try
            {
                DataTable dt = orders.GetLIKP(IKUNNR, IVBELN, IBEGIN, IEND);

                likp = DataTableToList.ConvertTo<LIKP>(dt);
                var list = likp;
                returnObj.IsError = false;
                returnObj.ReturnObject = list;
            }
            catch (Exception ex)
            {
                Log.WriteLog(logClass + ex.Message.ToString() + "\r\n", true);
                returnObj.IsError = true;
                returnObj.Error.ErrorMsg = ex.Message.ToString();
                returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
            }
            finally
            {

            }

            return returnObj;
        }

        ///// <summary>
        ///// 获取销售订单列表
        ///// </summary>
        ///// <param name="IKUNNR">客户编号</param>
        ///// <param name="IVBELN">订单号</param>
        ///// <param name="IBEGIN">开始日期</param>
        ///// <param name="IEND">结束日期</param>
        //[HttpGet]
        //public void GetAllOrderHead(string IKUNNR, string IVBELN, string IBEGIN, string IEND)
        //{
        //    ReturnBaseObject<IEnumerable<LIKP>> returnObj = new ReturnBaseObject<IEnumerable<LIKP>>() { ReturnObject = new List<LIKP>() };
        //    try
        //    {
        //        DataTable dt = orders.GetLIKP(IKUNNR, IVBELN, IBEGIN, IEND);

        //        likp = DataTableToList.ConvertTo<LIKP>(dt);
        //        var list = likp;
        //        returnObj.IsError = false;
        //        returnObj.ReturnObject = list;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteLog(logClass + ex.Message.ToString() + "\r\n", true);
        //        returnObj.IsError = true;
        //        returnObj.Error.ErrorMsg = ex.Message.ToString();
        //        returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
        //    }
        //    finally
        //    {
        //        JsonpCallback(JsonConvert.SerializeObject(returnObj));

        //    }
        //}

        /// <summary>
        /// 获取销售订单明细
        /// </summary>
        /// <param name="IVBELN">订单号</param>
        [HttpGet]
        public ReturnBaseObject<IEnumerable<LIPS>> GetOrderDtl(string IVBELN)
        {
            ReturnBaseObject<IEnumerable<LIPS>> returnObj = new ReturnBaseObject<IEnumerable<LIPS>>() { ReturnObject = new List<LIPS>() };
            try
            { 
                DataTable dt = orders.GetLIPS(IVBELN); 
                lips = DataTableToList.ConvertTo<LIPS>(dt);
                var list = lips;
                returnObj.IsError = false;
                returnObj.ReturnObject = list; 
            }
            catch (Exception ex)
            {
                Log.WriteLog(logClass + ex.Message.ToString() + "\r\n", true);
                returnObj.IsError = true;
                returnObj.Error.ErrorMsg = ex.Message.ToString();
                returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误; 
            }

            return returnObj;
        }
         
        //protected virtual void JsonpCallback(string json)
        //{
        //    HttpResponse Response = HttpContext.Current.Response;
        //    string callback = HttpContext.Current.Request["callback"];

        //    //如果callback是空, 就是普通的json, 否则就是jsonp
        //    Response.Write(callback == null ? json : string.Format("{0}({1})", callback, json));
        //    Response.End();
        //}
         
    }
}
