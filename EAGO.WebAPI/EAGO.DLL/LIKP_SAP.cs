using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using EAGO.DBUtility;
using SAP.Middleware.Connector;
using EAGO.Models;

namespace EAGO.DLL
{

    /// <summary>
    /// 销售订单主表
    /// </summary>
    public class LIKP_SAP
    {

        public string sendSAP(List<EAGO.Models.LIKP> likp, List<EAGO.Models.LIPS> lips)
        {
            try
            {
                SAP_FUN _sapdb = new SAP_FUN();
                SAP.Middleware.Connector.IDestinationConfiguration ID = _sapdb;
                SAP.Middleware.Connector.RfcDestinationManager.RegisterDestinationConfiguration(ID);
                SAP.Middleware.Connector.RfcDestination prd = SAP.Middleware.Connector.RfcDestinationManager.GetDestination("PRD_000");
                SAP.Middleware.Connector.RfcDestination _cc = prd;
                SAP.Middleware.Connector.RfcDestinationManager.UnregisterDestinationConfiguration(ID);
                SAP.Middleware.Connector.RfcRepository repo = prd.Repository;
                SAP.Middleware.Connector.IRfcFunction companyBapi = repo.CreateFunction("ZSDFMYGORDER");
                 

                //设置参数 && 连接sap
                  

                SAP.Middleware.Connector.IRfcTable reTB1 = (SAP.Middleware.Connector.IRfcTable)companyBapi.GetTable("ZLIKP");

                foreach (EAGO.Models.LIKP item in likp)
                {

                    IRfcStructure rowG = repo.GetStructureMetadata("ZTSDYGLIKP").CreateStructure();//函数表参考表结构 
                    rowG.SetValue("ZZVBELN", item.ZZVBELN == null ? "" : item.ZZVBELN);  //交货申请号
                    rowG.SetValue("VBELN", item.VBELN == null ? "" : item.VBELN);  //销售单号
                    rowG.SetValue("ZVBELN", item.ZVBELN == null ? "" : item.ZVBELN);  //交货单号
                    rowG.SetValue("ZERDAT", item.ZERDAT == null ? "" : item.ZERDAT.Replace("-", ""));  //创建日期
                    rowG.SetValue("LFART", item.LFART == null ? "" : item.LFART);  //交货类型
                    rowG.SetValue("VSTEL", item.VSTEL == null ? "" : item.VSTEL);  //交货地点
                    rowG.SetValue("VKORG", item.VKORG == null ? "" : item.VKORG);  //销售机构
                    rowG.SetValue("WADAT", item.WADAT == null ? "" : item.WADAT.Replace("-", ""));  //请求交货日期 Substring(0, 10).
                    rowG.SetValue("KUNNR", item.KUNNR == null ? "" : item.KUNNR);  //客户编号
                    rowG.SetValue("ZZCHHAO", item.ZZCHHAO == null ? "" : item.ZZCHHAO);  //车号
                    rowG.SetValue("SQR", item.SQR == null ? "" : item.SQR);  //申请人
                    rowG.SetValue("ERDAT", item.ERDAT == null ? "" : item.ERDAT.Replace("-", ""));  //下单日期 .Substring(0, 10)
                    rowG.SetValue("TOTAL", item.TOTAL);  //总金额/交货次数   ??? 
                    rowG.SetValue("ADDRESS", item.ADDRESS == null ? "" : item.ADDRESS);  //地址
                    rowG.SetValue("SHFLAG", item.SHFLAG == null ? "" : item.SHFLAG);
                    rowG.SetValue("SHDATE", item.SHDATE == null ? "" : item.SHDATE.Replace("-", ""));
                    rowG.SetValue("ZLFART", item.ZLFART == null ? "" : item.ZLFART);
                    rowG.SetValue("REMARK", item.REMARK == null ? "" : item.REMARK);  //备注 

                    reTB1.Append(rowG);
                    companyBapi.SetValue("ZLIKP", reTB1);//

                }


                SAP.Middleware.Connector.IRfcTable reTB2 = (SAP.Middleware.Connector.IRfcTable)companyBapi.GetTable("ZLIPS");


                int POSNR = 0;
                foreach (EAGO.Models.LIPS item in lips)
                {
                    POSNR = POSNR + 1;
                    IRfcStructure rowG = repo.GetStructureMetadata("ZTSDYGLIPS").CreateStructure();//函数表参考表结构

                    rowG.SetValue("ZZVBELN", item.ZZVBELN == null ? "" : item.ZZVBELN);  //交货申请单号
                    rowG.SetValue("POSNR", POSNR);  //行项目号
                    rowG.SetValue("MATNR", item.MATNR == null ? "" : item.MATNR);  //物料号
                    rowG.SetValue("MAKTX", item.MAKTX == null ? "" : item.MAKTX);  //物料描述
                    rowG.SetValue("MEINS", item.MEINS == null ? "" : item.MEINS);  //销售单位
                    rowG.SetValue("NEED", item.NEED);  //需求数量
                    rowG.SetValue("CONFIRM_NUM", item.CONFIRM_NUM);  //确认数量
                    rowG.SetValue("LFIMG", item.LFIMG);  //交货数量
                    rowG.SetValue("PRICE", item.PRICE);  //单价
                    rowG.SetValue("MONEY", item.MONEY);  //金额   

                    reTB2.Append(rowG); 

                }

                companyBapi.SetValue("ZLIPS", reTB2);// 

                companyBapi.SetValue("FTYPE", "3");//
                //执行函数
                companyBapi.Invoke(prd);

                return (string)companyBapi.GetValue("RESULT"); 
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }


        //IKUNNR	客户编号
        //IVBELN	销售单号 
        //IBEGIN	开始日期
        //IEND	结束日期  
        public DataTable GetLIKP(string IKUNNR, string IVBELN, string IBEGIN, string IEND)
        {

            SAP_FUN _sapdb = new SAP_FUN();
            SAP.Middleware.Connector.IDestinationConfiguration ID = _sapdb;
            SAP.Middleware.Connector.RfcDestinationManager.RegisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcDestination prd = SAP.Middleware.Connector.RfcDestinationManager.GetDestination("PRD_000");
            SAP.Middleware.Connector.RfcDestination _cc = prd;
            SAP.Middleware.Connector.RfcDestinationManager.UnregisterDestinationConfiguration(ID);
            SAP.Middleware.Connector.RfcRepository repo = prd.Repository;
            SAP.Middleware.Connector.IRfcFunction companyBapi = repo.CreateFunction("ZSDFMYGORDER");
            DataTable ItemTable = new DataTable("ZLIKP");

            ItemTable.Columns.Add(new DataColumn("ZZVBELN", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("VBELN", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ZVBELN", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ZERDAT", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("LFART", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("VSTEL", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("VKORG", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("WADAT", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("KUNNR", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ZZCHHAO", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("SQR", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ERDAT", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("TOTAL", typeof(decimal)));
            ItemTable.Columns.Add(new DataColumn("SHFLAG", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("SHDATE", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ZLFART", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("ADDRESS", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("REMARK", typeof(string)));

            //设置参数 && 连接sap
            companyBapi.SetValue("FTYPE", "1");//
            companyBapi.SetValue("IKUNNR", IKUNNR); //IKUNNR	客户编号 测试用 "0000105960"

            if (!string.IsNullOrEmpty(IVBELN))
            {
                companyBapi.SetValue("IVBELN", IVBELN); //IVBELN	销售单号 
            }


            if (!string.IsNullOrEmpty(IBEGIN))
            {
                companyBapi.SetValue("IBEGIN", IBEGIN.Replace("-", ""));//开始日期
            }

            if (!string.IsNullOrEmpty(IEND))
            {
                companyBapi.SetValue("IEND", IEND.Replace("-", ""));//结束日期
            }

            //执行函数
            companyBapi.Invoke(prd);
            SAP.Middleware.Connector.IRfcTable reTB1 = (SAP.Middleware.Connector.IRfcTable)companyBapi.GetValue("ZLIKP");
            for (int MI = 0; MI < reTB1.Count; MI++)
            {
                reTB1.CurrentIndex = MI;
                DataRow newRow = ItemTable.NewRow();
                newRow["ZZVBELN"] = reTB1.GetString("ZZVBELN").ToString();
                newRow["VBELN"] = reTB1.GetString("VBELN").ToString();
                newRow["ZVBELN"] = reTB1.GetString("ZVBELN").ToString();
                newRow["ZERDAT"] = reTB1.GetString("ZERDAT").ToString();
                newRow["LFART"] = reTB1.GetString("LFART").ToString();
                newRow["VSTEL"] = reTB1.GetString("VSTEL").ToString();
                newRow["VKORG"] = reTB1.GetString("VKORG").ToString();
                newRow["WADAT"] = reTB1.GetString("WADAT").ToString();
                newRow["KUNNR"] = reTB1.GetString("KUNNR").ToString();
                newRow["ZZCHHAO"] = reTB1.GetString("ZZCHHAO").ToString();
                newRow["SQR"] = reTB1.GetString("SQR").ToString();
                newRow["ERDAT"] = reTB1.GetString("ERDAT").ToString();
                newRow["TOTAL"] = reTB1.GetString("TOTAL").ToString();
                newRow["SHFLAG"] = reTB1.GetString("SHFLAG").ToString();
                newRow["SHDATE"] = reTB1.GetString("SHDATE").ToString();
                newRow["ZLFART"] = reTB1.GetString("ZLFART").ToString();
                newRow["ADDRESS"] = reTB1.GetString("ADDRESS").ToString();
                newRow["REMARK"] = reTB1.GetString("REMARK").ToString();
                ItemTable.Rows.Add(newRow);
            }

            DataTable dtCopy = ItemTable.Copy();
            DataView dv = ItemTable.DefaultView;
            dv.Sort = "ERDAT DESC ";
            dtCopy = dv.ToTable();


            return dtCopy;
        }

    }
}