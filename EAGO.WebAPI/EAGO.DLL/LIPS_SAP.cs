using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using EAGO.DBUtility;
using SAP.Middleware.Connector;

namespace EAGO.DLL
{

    /// <summary>
    /// 销售订单明细表
    /// </summary>
    public class LIPS_SAP
    {

        LIPS_SQL lips = new LIPS_SQL();

        /// <summary>
        /// IVBELN 销售订单号
        /// </summary>
        public DataTable GetLIPS( string IVBELN)
        {

            SAP_FUN _sapdb = new SAP_FUN();
            SAP.Middleware.Connector.IDestinationConfiguration ID = _sapdb;
            SAP.Middleware.Connector.RfcDestinationManager.RegisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcDestination prd = SAP.Middleware.Connector.RfcDestinationManager.GetDestination("PRD_000");
            SAP.Middleware.Connector.RfcDestination _cc = prd;
            SAP.Middleware.Connector.RfcDestinationManager.UnregisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcRepository repo = prd.Repository;
            SAP.Middleware.Connector.IRfcFunction companyBapi = repo.CreateFunction("ZSDFMYGORDER");
            DataTable ItemTable = new DataTable("ZLIPS");

            ItemTable.Columns.Add(new DataColumn("ZZVBELN", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("POSNR", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("MATNR", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("MAKTX", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("MEINS", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("NEED", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("CONFIRM_NUM", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("LFIMG", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("PRICE", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("MONEY", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("APPLY_NUM", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("ORDER_NUM", typeof(decimal)));

             

            //设置参数 && 连接sap
            companyBapi.SetValue("FTYPE", "2");//
            companyBapi.SetValue("IVBELN", IVBELN); //
             

            //执行函数
            companyBapi.Invoke(prd);
            SAP.Middleware.Connector.IRfcTable reTB1 = (SAP.Middleware.Connector.IRfcTable)companyBapi.GetValue("ZLIPS");
            for (int MI = 0; MI < reTB1.Count; MI++)
            {
                reTB1.CurrentIndex = MI;
                DataRow newRow = ItemTable.NewRow();
                newRow["ZZVBELN"] = reTB1.GetString("ZZVBELN").ToString();
                newRow["POSNR"] = reTB1.GetString("POSNR").ToString();
                newRow["MATNR"] = reTB1.GetString("MATNR").ToString();
                newRow["MAKTX"] = reTB1.GetString("MAKTX").ToString();
                newRow["MEINS"] = reTB1.GetString("MEINS").ToString();
                //newRow["NEED"] = reTB1.GetString("NEED").ToString();
                //newRow["CONFIRM_NUM"] = reTB1.GetString("CONFIRM_NUM").ToString();

                newRow["ORDER_NUM"] = reTB1.GetString("NEED").ToString(); //销售订单数量
                newRow["APPLY_NUM"] = lips.getApplyNum(IVBELN, newRow["MATNR"].ToString()).ToString();    ///获取已申请数量 
                newRow["LFIMG"] = reTB1.GetString("LFIMG").ToString(); //已发货数量

                newRow["PRICE"] = reTB1.GetString("PRICE").ToString();
                newRow["MONEY"] = reTB1.GetString("MONEY").ToString();  

                ItemTable.Rows.Add(newRow);
            }

            return ItemTable;
        }


    }
}