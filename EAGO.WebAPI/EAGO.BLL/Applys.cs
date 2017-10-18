using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using EAGO.DLL;
using System.Data;

namespace EAGO.BLL
{

    public partial class Applys
    {
        private readonly EAGO.DLL.LIKP_SQL LIKP = new EAGO.DLL.LIKP_SQL();
        private readonly EAGO.DLL.LIPS_SQL LIPS = new EAGO.DLL.LIPS_SQL();


        #region  Method

        /// <summary>
        /// 获取申请列表
        /// </summary>
        public List<EAGO.Models.LIKP> GetLIKP(string KUNNR, string ZZVBELN, string VBELN, string BEGDATE, string ENDDATE, string SENDFLAG, string LFART, string ZVBELN)
        {
            return LIKP.GetLIKP(KUNNR, ZZVBELN, VBELN, BEGDATE, ENDDATE, SENDFLAG, LFART, ZVBELN);
        }

         //<summary>
         //发送申请
         //</summary>
        public string send(string ZZVBELN)
        {
            return LIKP.send(ZZVBELN);
        }

        //<summary>
        //保存申请
        //</summary>
        public EAGO.Models.LIKP save(EAGO.Models.LIKP likp)
        {
            string ZZVBELN = LIKP.save(likp);
            likp.ZZVBELN = ZZVBELN;
            return likp;
        }


        //<summary>
        //保存申请明细
        //</summary>
        public string savedtl(List< EAGO.Models.LIPS> lips)
        {
            return LIKP.savedtl(lips);
        }

        //<summary>
        //获取申请明细
        //</summary>
        public List<EAGO.Models.LIPS> getlips(string fguid)
        {
            return LIPS.getlips(fguid);
        }

        //<summary>
        //删除申请
        //</summary>
        public string delete(string fguid)
        {
            return LIKP.delete(fguid);
        }
        #endregion  Method
    }
}
