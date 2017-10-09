using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using EAGO.DBUtility;
using SAP.Middleware.Connector;
using EAGO;

namespace EAGO.DLL
{
    public class FIFM_SAP
    {/// <summary>
        /// 付款明细表
        /// ZKUNNR 客户编码
        /// BUDAT1 开始日期
        /// BUDAT2 结束日期
        /// </summary>  
        public DataTable GetFIFM(string ZKUNNR, string BUDAT1, string BUDAT2)
        {  

            SAP_FUN _sapdb = new SAP_FUN();
            SAP.Middleware.Connector.IDestinationConfiguration ID = _sapdb;
            SAP.Middleware.Connector.RfcDestinationManager.RegisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcDestination prd = SAP.Middleware.Connector.RfcDestinationManager.GetDestination("PRD_000");
            SAP.Middleware.Connector.RfcDestination _cc = prd;
            SAP.Middleware.Connector.RfcDestinationManager.UnregisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcRepository repo = prd.Repository;
            SAP.Middleware.Connector.IRfcFunction companyBapi = repo.CreateFunction("ZFIFM012");
            DataTable ItemTable = new DataTable("FIFM");

            ItemTable.Columns.Add(new DataColumn("Z_BUDAT", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("Z_NAME1", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("Z_ZUONR", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("Z_ZZRTYPET", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("Z_WRBTR", typeof(string)));


            //测试 公司代码 1010科技公司
            //正式 1200 益高
            //设置参数 && 连接sap
            companyBapi.SetValue("I_BUKRS", "1010");// 
            companyBapi.SetValue("I_ZKUNNR1", ZKUNNR);
            companyBapi.SetValue("I_ZKUNNR2", ZKUNNR);

            if (!string.IsNullOrEmpty(BUDAT1))
            {
                companyBapi.SetValue("I_BUDAT1", BUDAT1.Replace("-", ""));//开始日期
            }

            if (!string.IsNullOrEmpty(BUDAT2))
            {
                companyBapi.SetValue("I_BUDAT2", BUDAT2.Replace("-", ""));//结束日期
            }

            //执行函数
            companyBapi.Invoke(prd);
            SAP.Middleware.Connector.IRfcTable reTB1 = (SAP.Middleware.Connector.IRfcTable)companyBapi.GetValue("I_ZFIFM012"); // I_ZFIFM012 内表名称
            for (int MI = 0; MI < reTB1.Count; MI++)
            {
                reTB1.CurrentIndex = MI;
                DataRow newRow = ItemTable.NewRow();
                newRow["Z_BUDAT"] = reTB1.GetString("Z_BUDAT").ToString();
                newRow["Z_NAME1"] = reTB1.GetString("Z_NAME1").ToString();
                newRow["Z_ZUONR"] = reTB1.GetString("Z_ZUONR").ToString();
                newRow["Z_ZZRTYPET"] = reTB1.GetString("Z_ZZRTYPET").ToString();
                newRow["Z_WRBTR"] = reTB1.GetString("Z_WRBTR").ToString();

                ItemTable.Rows.Add(newRow);
            }

            DataTable dtCopy = ItemTable.Copy();

            DataView dv = ItemTable.DefaultView;
            dv.Sort = "Z_BUDAT DESC ";
            dtCopy = dv.ToTable();


            return dtCopy;
        }

    }
}
