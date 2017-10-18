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
    /// 付款明细
    /// </summary>
    [RequestAuthorize] //票据验证特征
    public class PaymentDetailsController : ApiController
    {

        private EAGO.BLL.PaymentDetails payment = new EAGO.BLL.PaymentDetails();
        private string logClass = "PaymentDetails API：";
        private EAGO.WebApi.Log.Log Log;
        private static IList<PaymentDetail> likp = new List<PaymentDetail> { };


        public PaymentDetailsController()
        { 
            Log = new Log.Txt.TxtLog();
        } 

        
        /// <summary>
        /// 付款明细
        /// </summary>
        /// <param name="ZKUNNR">客户编号</param>
        /// <param name="IBEGIN">开始日期</param>
        /// <param name="IEND">结束日期</param>
        [HttpGet]
        public ReturnBaseObject<IEnumerable<PaymentDetail>> GetAllPayment(string ZKUNNR, string IBEGIN, string IEND) //ReturnBaseObject<IEnumerable<PaymentDetail>>
        {
            ReturnBaseObject<IEnumerable<PaymentDetail>> returnObj = new ReturnBaseObject<IEnumerable<PaymentDetail>>() { ReturnObject = new List<PaymentDetail>() };
            try
            {
                //"0000105960", "", "", ""
                DataTable dt = payment.GetList(ZKUNNR,  IBEGIN, IEND);

                likp = DataTableToList.ConvertTo<PaymentDetail>(dt);
                var list = likp;
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
