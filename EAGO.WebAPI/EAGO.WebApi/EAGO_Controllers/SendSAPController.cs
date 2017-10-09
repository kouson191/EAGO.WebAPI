using System.Text;
using System.Web.Http;
using EAGO.WebApi.Common;
using EAGO.WebApi.Log;
using EAGO.Models;
using System.Data;
using EAGO.BLL;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Web;

namespace EAGO.WebApi.EAGO_Controllers
{
    public class SendSAPController : ApiController
    {

        private EAGO.BLL.Applys applys = new EAGO.BLL.Applys();
        private string logClass = "SendSAP API：";
        private EAGO.WebApi.Log.Log Log;


        public SendSAPController()
        { 
            Log = new Log.Txt.TxtLog();
        } 


        [HttpGet]
        public void GetSend(string ZZVBELN)
        {
            ReturnBaseObject<string> returnObj = new ReturnBaseObject<string>() { ReturnObject = "" };
            try
            {
                string result = applys.send(ZZVBELN);
                returnObj.ReturnObject = result;
                returnObj.IsError = false;
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


    }
}
