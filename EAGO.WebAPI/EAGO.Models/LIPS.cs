using System;
using System.Collections.Generic; 
using System.Web;

namespace EAGO.Models
{
    public class LIPS
    {
        public string GUID { get; set; }
        public string FGUID { get; set; }

        /// <summary>
        /// 交货申请单号
        /// </summary>
        public string ZZVBELN { get; set; }

        /// <summary>
        /// 行项目号
        /// </summary>
        public string POSNR { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string MAKTX { get; set; }

        /// <summary>
        /// 销售单位
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal NEED { get; set; }

        /// <summary>
        /// 确认数量
        /// </summary>
        public decimal CONFIRM_NUM { get; set; }

        /// <summary>
        /// 交货数量
        /// </summary>
        public decimal LFIMG { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal PRICE { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal MONEY { get; set; }


        /// <summary>
        /// 已申请数量
        /// </summary>
        public decimal APPLY_NUM { get; set; }



        /// <summary>
        /// 销售订单数量
        /// </summary>
        public decimal ORDER_NUM { get; set; }

    }
}