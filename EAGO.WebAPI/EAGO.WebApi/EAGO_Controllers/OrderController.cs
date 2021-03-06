﻿using System;
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
        public HttpResponseMessage GetAllOrderHead(string IKUNNR, string IVBELN, string IBEGIN, string IEND)
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

            string str = Jil.JSON.Serialize<ReturnBaseObject<IEnumerable<LIKP>>>(returnObj);
            HttpResponseMessage resultmsg = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return resultmsg;
        } 

        /// <summary>
        /// 获取销售订单明细
        /// </summary>
        /// <param name="IVBELN">订单号</param>
        [HttpGet]
        public HttpResponseMessage GetOrderDtl(string IVBELN)
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

            string str = Jil.JSON.Serialize<ReturnBaseObject<IEnumerable<LIPS>>>(returnObj);
            HttpResponseMessage resultmsg = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return resultmsg;
        } 
    }
}
