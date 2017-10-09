using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
         List<EAGO.Models.LIKP> likps;
         EAGO.Models.LIKP likp;
         List<EAGO.Models.LIPS> lips;
         EAGO.Models.LIPS lipdtl; 



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //EAGO.DLL.LIKP_SQL sap = new EAGO.DLL.LIKP_SQL();
            //string rslt = sap.send("010120170908001");
            //MessageBox.Show(rslt);


            //EAGO.BLL.Orders order = new EAGO.BLL.Orders();
            //DataTable dt =  order.GetLIKP("0000105960", "0110878855", "", "");

            EAGO.BLL.DropDrownList ddl = new EAGO.BLL.DropDrownList();

            List<string> ls = ddl.getValueList("LFART");


            //EAGO.BLL.Applys applys = new EAGO.BLL.Applys();
            //likp = new EAGO.Models.LIKP(); 
            //likp.VBELN  = "AAA";
            //likp.ZVBELN  = "";
            //likp.ZERDAT = "2017-01-01";
            //likp.LFART = "ZA6A";
            //likp.VSTEL = "顺德";
            //likp.VKORG = "A";
            //likp.WADAT = "2017-09-09";
            //likp.KUNNR = "0000105960";
            //likp.TOTAL = 1000;
            //likp.ZZCHHAO = "XS888";
            //likp.SQR = "张"; 
            //likp.ADDRESS = "222";
            //likp.REMARK = "111";


            //lipdtl = new EAGO.Models.LIPS();
             


            //lipdtl.ZZVBELN = "" ; //交货申请单号
            //lipdtl.POSNR = "1" ; //行项目号
            //lipdtl.MATNR = "001" ; //物料号
            //lipdtl.MAKTX = "物料"; //物料描述
            //lipdtl.MEINS = "个" ; //销售单位
            //lipdtl.NEED = 100 ; //需求数量 订单数量
            //lipdtl.CONFIRM_NUM = 10 ; //确认数量(需求数量)
            //lipdtl.LFIMG = 50 ; //交货数量 已发数量
            //lipdtl.PRICE = 100; //单价
            //lipdtl.MONEY = lipdtl.CONFIRM_NUM * lipdtl.PRICE; //金额

            //likp.LIPS = new List<EAGO.Models.LIPS>();
            //likp.LIPS.Add(lipdtl);


            ////保存
            //applys.save(likp);

            //发送  
            //applys.send("000010596020170908002");

            ////明细查询
            //likp.GUID = "67ed24f9-b433-4a3d-950d-33b9718cc0a5"; 
            //lips = applys.getmode(likp.GUID);


             //likps = applys.GetLIKP("0000105960","010120170908001", "", "", "", "", "", "");

            //string url = "http://192.168.12.3:8321/api/Applys";
            //string data = "{ZZVBELN=000010596020170908002}";
            //string result = HttpPost(url, data);
            //MessageBox.Show(result); 


        }


        public static string HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "text";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
         
    }
}
