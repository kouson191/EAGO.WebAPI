using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EAGO.WebApi.Common
{
    public class Error
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public enum EnumErrorCode
        {
            操作成功,
            未知错误,
            编码重复,
            找不到值
        }

        private bool isError = false;
        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                if (isError)
                {
                    if (CallError != null)
                    {
                        CallError(null, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// 错误编码
        /// </summary>
        public EnumErrorCode ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 错误的触发
        /// </summary>
        public event EventHandler CallError;
    }
}
