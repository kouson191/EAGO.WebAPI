using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAGO.WebApi.Common
{
    public class ReturnBaseObject<T>
    {
        public ReturnBaseObject()
        {
            Error = new Error();
            IsError = false;
            Error.ErrorCode = Error.EnumErrorCode.操作成功;
        }
        /// <summary>
        /// 是否操作错误
        /// </summary>
        public bool IsError
        {
            get { return Error.IsError; }
            set { Error.IsError = value; }
        }
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T ReturnObject { get; set; }

        public Error Error { get; set; }
        /// <summary>
        /// 扩展对像
        /// </summary>
        public object ExtendObject { get; set; }

    }
}
