using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using EAGO.DLL;
using System.Data;


namespace EAGO.BLL
{
    public partial class Orders
    {

        private readonly EAGO.DLL.LIKP_SAP LIKP = new EAGO.DLL.LIKP_SAP();
        private readonly EAGO.DLL.LIPS_SAP LIPS = new EAGO.DLL.LIPS_SAP();

        public Orders()
        { }
        #region  Method

        /// <summary>
        /// 获取销售订单列表
        /// </summary>
        public DataTable GetLIKP(string IKUNNR, string IVBELN, string IBEGIN, string IEND)
        {
            return LIKP.GetLIKP(   IKUNNR,   IVBELN,   IBEGIN,   IEND);
        }


        /// <summary>
        /// 获取销售订单明细
        /// </summary>
        public DataTable GetLIPS(string IVBELN)
        {
            return LIPS.GetLIPS(IVBELN);
        }


        #endregion  Method

    }
}
