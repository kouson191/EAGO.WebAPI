using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EAGO.DLL;
using System.Data;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //EAGO.DLL.LIKP_SAP sap = new LIKP_SAP();
            //DataTable dt = sap.GetLIKP("0000105960", "", "", "");
            //dt.WriteXml("d://1.xml");


            //EAGO.DLL.LIKP_SAP order = new EAGO.DLL.LIKP_SAP();
            //order.


            //EAGO.BLL.Orders order = new EAGO.BLL.Orders();
            //DataTable dt = order.GetLIKP("0000105960", "0110878855", "", "");
             

            EAGO.DLL.LIKP_SQL sap = new EAGO.DLL.LIKP_SQL();
           string rslt =  sap.send("000010596020170908002");  

            //EAGO.DLL.FIFM_SAP sap = new FIFM_SAP();
            //DataTable dt = sap.GetFIFM("0000102594", "2010-01-01", "2017-12-31");

            //dt.WriteXml("d://3.xml");
        }
    }
}
