using System;
using System.Collections.Generic; 
using System.Web;

namespace EAGO.Models
{
    public class LIKP
    {

        public Int32 ID { get; set; }

        public string GUID { get; set; } 

        /// <summary>
        /// 交货申请号 
        /// </summary>
        ///  
        public string ZZVBELN { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        public string VBELN { get; set; }

        /// <summary>
        /// 交货单号
        /// </summary>
        public string ZVBELN { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string ZERDAT { get; set; }

        /// <summary>
        /// 交货类型
        /// </summary>
        public string LFART { get; set; }

        /// <summary>
        /// 交货地点
        /// </summary>
        public string VSTEL { get; set; }

        /// <summary>
        /// 销售机构
        /// </summary>  	
        public string VKORG { get; set; }

        /// <summary>
        /// 请求交货日期 	 	
        /// </summary>
        public string WADAT { get; set; }

        /// <summary>
        /// 客户编号 	
        /// </summary>
        public string KUNNR { get; set; }

        /// <summary>
        /// 发货数量 	
        /// </summary>
        public decimal TOTAL { get; set; }

        /// <summary>
        /// 车号 
        /// </summary>
        public string ZZCHHAO { get; set; }

        /// <summary>
        /// 申请人 
        /// </summary>
        public string SQR { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public string ERDAT { get; set; }

        /// <summary>
        /// 发送标记	
        /// </summary>
        public string SENDFLAG { get; set; }

        /// <summary>
        /// 发送日期 
        /// </summary>
        public string SENDDATE { get; set; }
         
        /// <summary>
        /// 审核标志 	
        /// </summary>
        public string SHFLAG { get; set; }

        /// <summary>
        /// 审核日期 
        /// </summary>
        public string SHDATE { get; set; }

        
        /// <summary>
        /// 审核交货类型 	
        /// </summary>
        public string ZLFART { get; set; }
        

        /// <summary>
        /// 地址 	
        /// </summary>
        public string ADDRESS { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<LIPS> LIPS { get; set; }
    }
}