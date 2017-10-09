using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EAGO.WebApi.Log
{
    public interface Log
    {   /// <summary>
        /// 参数设置
        /// </summary>
        object Parameter { get; set; }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="_value">日志内容</param>
        /// <param name="addDateTime">是否自动添加时间</param>
        void WriteLog(string _value, bool addDateTime);
    }
}

/*例子:2013-03-14 
Code.Log.Interface.Log Log = new BY.Data.Code.Log.Txt.TxtLog();
Log.Parameter = new BY.Data.Code.Log.Txt.TxtParameter { FileLeft = "Error", FullPath = System.AppDomain.CurrentDomain.BaseDirectory };
Log.WriteLog("TEST", true);
 * */
