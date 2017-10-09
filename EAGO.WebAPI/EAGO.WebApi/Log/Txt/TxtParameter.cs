using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAGO.WebApi.Log.Txt
{
    /// <summary>
    /// Txt日志文件参数
    /// </summary>
    public class TxtParameter
    {
        /// <summary>
        /// 写入前辍
        /// </summary>
        public string FileLeft = "Error";

        /// <summary>
        /// 写入路径
        /// </summary>
        public string FullPath
        {
            get
            {
                if (fullPath == "")
                {
                    return System.AppDomain.CurrentDomain.BaseDirectory;
                }
                if (!System.IO.Directory.Exists(fullPath + "\\Log"))
                {
                    System.IO.Directory.CreateDirectory(fullPath + "\\Log");
                }
                return fullPath + "\\Log";
            }
            set
            {
                fullPath = value;
            }
        }
        private string fullPath = "";
    }
}
