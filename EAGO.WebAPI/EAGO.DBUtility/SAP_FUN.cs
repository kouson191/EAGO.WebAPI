using System;
using System.Collections.Generic; 
using System.Text; 
using System.Data;
using SAP.Middleware.Connector;
using System.Configuration;

namespace EAGO.DBUtility
{
    public class SAP_FUN : IDestinationConfiguration
    {
        public RfcConfigParameters GetParameters(String destinationName)
        {
            if ("PRD_000".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();

                string AppServerHost = ConfigurationManager.AppSettings["AppServerHost"];
                string User = ConfigurationManager.AppSettings["User"];
                string Password = ConfigurationManager.AppSettings["Password"];
                string Client = ConfigurationManager.AppSettings["Client"];


                parms.Add(RfcConfigParameters.AppServerHost, AppServerHost);   //SAP主机IP 
                parms.Add(RfcConfigParameters.SystemNumber, "00");  //SAP实例

                //parms.Add(RfcConfigParameters.MessageServerHost, AppServerHost); //SAP主机IP
                //parms.Add(RfcConfigParameters.MessageServerService, "3601");

                parms.Add(RfcConfigParameters.LogonGroup, "eccprd");  
                parms.Add(RfcConfigParameters.User, User);  //用户名 
                parms.Add(RfcConfigParameters.Password, Password);  //密码 
                parms.Add(RfcConfigParameters.Client, Client);  // Client

                parms.Add(RfcConfigParameters.Language, "ZH");  //登陆语言
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "100");
                parms.Add(RfcConfigParameters.IdleTimeout, "60");
                return parms;
            }
            else return null;
        }


        public bool ChangeEventsSupported()
        {

            return false;

        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

    }
}