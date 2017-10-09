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
    public class ApplysDtlController : ApiController
    {

        private EAGO.BLL.Applys applys = new EAGO.BLL.Applys();
        private string logClass = "ApplysDtl API：";
        private EAGO.WebApi.Log.Log Log; 
        private static IList<LIPS> lips = new List<LIPS> { };


        public ApplysDtlController()
        { 
            Log = new Log.Txt.TxtLog();
        } 


        [HttpGet]
        public void GetApplysDtl(string ZZVBELN)
        {
            ReturnBaseObject<IEnumerable<LIPS>> returnObj = new ReturnBaseObject<IEnumerable<LIPS>>() { ReturnObject = new List<LIPS>() };
            try
            {
                //"010120170908001", "", "", "", "", "", ""
                lips = applys.getlips(ZZVBELN);

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
                // return returnObj;
            }
            finally
            {
                JsonpCallback(JsonConvert.SerializeObject(returnObj));

            }
        }


        [ HttpPost]
        public void PostApplysDtl(List< EAGO.Models.LIPS> lips)
        {
            ReturnBaseObject<string> returnObj = new ReturnBaseObject<string>() { ReturnObject = "" };
            try
            { 
                string rslt = applys.savedtl(lips);
                returnObj.ReturnObject = rslt;  

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
