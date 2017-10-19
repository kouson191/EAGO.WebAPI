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
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using System.Net.Http;

namespace EAGO.WebApi.EAGO_Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : ApiController
    {
        private string logClass = "User API：";
        private EAGO.WebApi.Log.Log Log;

        public UserController()
        {
            Log = new Log.Txt.TxtLog();
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        //[HttpGet]
        //public void Login(string strUser, string strPwd)
        //{
        //    ReturnBaseObject<UserInfo> returnObj = new ReturnBaseObject<UserInfo>() { ReturnObject = new UserInfo() };
        //    if (!ValidateUser(strUser, strPwd))
        //    {
        //        Log.WriteLog(logClass + "登录失败" + "\r\n", true);
        //        returnObj.IsError = true;
        //        returnObj.Error.ErrorMsg = "登录失败";
        //        returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
        //    }
        //    else
        //    {
        //        strPwd = GetRandomString(8);
        //        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, strUser, DateTime.Now,
        //                        DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", strUser, strPwd),
        //                        FormsAuthentication.FormsCookiePath);
        //        //返回登录结果、用户信息、用户验证票据信息
        //        var oUser = new UserInfo { bRes = true, UserName = strUser, Password = strPwd, Ticket = FormsAuthentication.Encrypt(ticket) };

        //        CacheHelper.SetCache(strUser, strPwd, 7200);

        //        returnObj.IsError = false;
        //        returnObj.ReturnObject = oUser;
        //    }

        //    JsonpCallback(JsonConvert.SerializeObject(returnObj));
        //}


        [HttpGet]
        public HttpResponseMessage Login(string strUser, string strPwd)
        {
            ReturnBaseObject<UserInfo> returnObj = new ReturnBaseObject<UserInfo>() { ReturnObject = new UserInfo() };
            if (!ValidateUser(strUser, strPwd))
            {
                Log.WriteLog(logClass + "登录失败" + "\r\n", true);
                returnObj.IsError = true;
                returnObj.Error.ErrorMsg = "登录失败";
                returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
            }
            else
            {
                strPwd = strUser; // GetRandomString(8);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, strUser, DateTime.Now,
                                DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", strUser, strPwd),
                                FormsAuthentication.FormsCookiePath);
                //返回登录结果、用户信息、用户验证票据信息
                var oUser = new UserInfo { bRes = true, UserName = strUser, Password = "", Ticket = FormsAuthentication.Encrypt(ticket) };

                CacheHelper.SetCache(strUser, strPwd);

                returnObj.IsError = false;
                returnObj.ReturnObject = oUser;
            }

            string str = Jil.JSON.Serialize<ReturnBaseObject<UserInfo>>(returnObj);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }



        //protected virtual void JsonpCallback(string json)
        //{
        //    HttpResponse Response = HttpContext.Current.Response;
        //    string callback = HttpContext.Current.Request["callback"];

        //    //如果callback是空, 就是普通的json, 否则就是jsonp
        //    Response.Write(callback == null ? json : string.Format("{0}({1})", callback, json));
        //    Response.End();
        //}

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateUser(string strUser, string strPwd)
        {
            return true;

            //if (strUser == "admin" && strPwd == "123456")
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public static string GetRandomString(int iLength)
        {
            string buffer = "0123456789*/.#~";// 随机字符中也可以为汉字（任何）  
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }
    }

    public class UserInfo
    {
        public bool bRes { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Ticket { get; set; }
    }



}