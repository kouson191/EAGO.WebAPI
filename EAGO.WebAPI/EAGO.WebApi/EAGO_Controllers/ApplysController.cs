﻿using System.Text;
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

namespace EAGO.WebApi.EAGO_Controllers
{
    public class ApplysController : ApiController
    {

        private EAGO.BLL.Applys applys = new EAGO.BLL.Applys();
        private string logClass = "Applys API：";
        private EAGO.WebApi.Log.Log Log;
        private static IList<LIKP> likp = new List<LIKP> { };
        private static IList<LIPS> lips = new List<LIPS> { };


        public ApplysController()
        {
            Log = new Log.Txt.TxtLog();
        }


        /// <summary>
        /// 获取交货申请列表
        /// </summary>
        /// <param name="KUNNR">客户编号</param>
        /// <param name="ZZVBELN">交货申请单号</param>
        /// <param name="VBELN">销售订单号</param>
        /// <param name="BEGDATE">开始日期</param>
        /// <param name="ENDDATE">结束日期</param>
        /// <param name="SENDFLAG">是否已发送</param>
        /// <param name="LFART">交货类型</param>
        /// <param name="ZVBELN">交货单号</param>
        [HttpGet]
        public void GetAllApplys(string KUNNR, string ZZVBELN, string VBELN, string BEGDATE, string ENDDATE, string SENDFLAG, string LFART, string ZVBELN)
        {
            ReturnBaseObject<IEnumerable<LIKP>> returnObj = new ReturnBaseObject<IEnumerable<LIKP>>() { ReturnObject = new List<LIKP>() };
            try
            {
                //"010120170908001", "", "", "", "", "", ""
                likp = applys.GetLIKP(KUNNR, ZZVBELN, VBELN, BEGDATE, ENDDATE, SENDFLAG, LFART, ZVBELN);

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
                // return returnObj;
            }
            finally
            {
                JsonpCallback(JsonConvert.SerializeObject(returnObj));

            }
        }


        /// <summary>
        /// 获取交货申请明细
        /// </summary>
        /// <param name="ZZVBELN">交货申请单号</param>
        [HttpGet]
        public void GetApplysDtl(string ZZVBELN)
        {
            ReturnBaseObject<IEnumerable<LIPS>> returnObj = new ReturnBaseObject<IEnumerable<LIPS>>() { ReturnObject = new List<LIPS>() };
            try
            { 
                lips = applys.getlips(ZZVBELN);

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
            finally
            {
                JsonpCallback(JsonConvert.SerializeObject(returnObj));

            }
        }


        /// <summary>
        /// 保存交货申请
        /// </summary>
        /// <param name="slikp">交货申请Jason对象</param>
        [HttpPost]
        public void PostApplys(dynamic slikp)
        {
            EAGO.Models.LIKP likp = new EAGO.Models.LIKP();
            likp.LIPS = new List<LIPS>();

            likp.VBELN = slikp["VBELN"] ?? "";
            likp.LFART = slikp["LFART"] ?? "";
            likp.WADAT = slikp["WADAT"] ?? "";
            likp.KUNNR = slikp["KUNNR"] ?? "";
            likp.ZZCHHAO = slikp["ZZCHHAO"] ?? "";
            likp.SQR = slikp["SQR"] ?? "";
            likp.ADDRESS = slikp["ADDRESS"] ?? "";
            likp.REMARK = slikp["REMARK"] ?? "";


            var record = slikp["LIPS"];
            foreach (var jp in record)
            {
                EAGO.Models.LIPS lip = new EAGO.Models.LIPS();
                lip.POSNR = jp.POSNR ?? "1";
                lip.MATNR = jp.MATNR ?? "";
                lip.MAKTX = jp.MAKTX ?? "";
                lip.MEINS = jp.MEINS ?? "";
                lip.NEED = jp.NEED ?? 0;
                lip.CONFIRM_NUM = jp.CONFIRM_NUM ?? 0;
                lip.LFIMG = jp.LFIMG ?? 0; 
                likp.LIPS.Add(lip); 
            }  

            ReturnBaseObject<EAGO.Models.LIKP> returnObj = new ReturnBaseObject<EAGO.Models.LIKP>() { ReturnObject = new EAGO.Models.LIKP() };
            try
            {
                returnObj.ReturnObject = applys.save(likp);

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

        /// <summary>
        /// 将交货申请发送SAP
        /// </summary>
        /// <param name="ZZVBELN">交货申请单号</param>
        [HttpGet]
        public void SendSAP(string ZZVBELN)
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