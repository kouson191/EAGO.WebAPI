using System;
using System.Collections.Generic; 
using System.Web;

namespace EAGO.Models
{
//Z_BUDAT	DATS	日期
//Z_NAME1	CHAR	本月付款单位明细
//Z_ZUONR	CHAR	银行回单号
//Z_ZZRTYPET	CHAR	收款类型
//Z_WRBTR	CURR	金额
    public class PaymentDetail
    {

        public string Z_BUDAT { get; set; }
        public string Z_NAME1 { get; set; }
        public string Z_ZUONR { get; set; }
        public string Z_ZZRTYPET { get; set; }
        public string Z_WRBTR { get; set; }
    }
}
