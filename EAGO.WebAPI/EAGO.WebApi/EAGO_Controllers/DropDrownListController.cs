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
    public class DropDrownListController : ApiController
    {
        //
        // GET: /DropDrownList/ 

        private EAGO.BLL.DropDrownList ddl = new EAGO.BLL.DropDrownList();
        private string logClass = "DropDrownList API：";
        private EAGO.WebApi.Log.Log Log;

        private static IList<EAGO.Models.DropDrownList> valuelist = new List<EAGO.Models.DropDrownList> { };


        public DropDrownListController()
        { 
            Log = new Log.Txt.TxtLog();
        } 

        /// <summary>
        /// 获取选项列表
        /// </summary>
        /// <param name="type">类型</param>
        [HttpGet]
        public ReturnBaseObject<IEnumerable<EAGO.Models.DropDrownList>> GetList(string type)
        {
            ReturnBaseObject<IEnumerable<EAGO.Models.DropDrownList>> returnObj = new ReturnBaseObject<IEnumerable<EAGO.Models.DropDrownList>>() { ReturnObject = new List<EAGO.Models.DropDrownList>() };
            try
            {
                valuelist = ddl.getDropDrownList(type);

                var list = valuelist;
                returnObj.IsError = false;
                returnObj.ReturnObject = valuelist;
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
