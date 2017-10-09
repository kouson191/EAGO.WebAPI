using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using EAGO.DLL;
using System.Data;

namespace EAGO.BLL
{
    public partial class PaymentDetails
    {

        private readonly EAGO.DLL.FIFM_SAP FIFM = new EAGO.DLL.FIFM_SAP();

        #region  Method

        /// <summary>
        /// 获取付款明细列表
        /// </summary>
        public DataTable GetList(string ZKUNNR, string IBEGIN, string IEND)
        {
            return FIFM.GetFIFM(ZKUNNR,IBEGIN, IEND);
        }

        #endregion  Method
    }
}
