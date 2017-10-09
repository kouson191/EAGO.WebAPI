using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http; 
using EAGO.WebApi.Common;
using EAGO.WebApi.Log;
using EAGO.Models;

namespace EAGO.WebApi.EAGO_Controllers
{
    public class IPAddressController : ApiController
    {

        private string logClass = "Address API：";
        private EAGO.WebApi.Log.Log Log;


        private static IList<Address> addresses = new List<Address>  
        {   
            new Address(){ IPAddress="1.91.38.31", Province="北京市", City="北京市" },     
            new Address(){ IPAddress = "210.75.225.254", Province = "上海市", City = "上海市"  },  
        };


        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
         
        public ReturnBaseObject<IEnumerable<Address>> GetIPAddresses()
        {
            ReturnBaseObject<IEnumerable<Address>> returnObj = new ReturnBaseObject<IEnumerable<Address>>() { ReturnObject = new List<Address>() };
            try
            {
                var list = addresses;
                returnObj.IsError = false;
                returnObj.ReturnObject = list;
                return returnObj;
            }
            catch (Exception ex)
            {
                Log.WriteLog(logClass + ex.Message.ToString() + "\r\n", true);
                returnObj.IsError = true;
                returnObj.Error.ErrorMsg = ex.Message.ToString();
                returnObj.Error.ErrorCode = Error.EnumErrorCode.未知错误;
                return returnObj;
            }
        } 



        public Address GetIPAddressByIP(string IP)
        {
            return addresses.FirstOrDefault(x => x.IPAddress == IP);
        }

    }  
}
