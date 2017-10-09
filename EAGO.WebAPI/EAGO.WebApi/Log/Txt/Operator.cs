using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAGO.WebApi.Log;

namespace EAGO.WebApi.Log.Txt
{
    /// <summary>
    /// Txt日志操作类
    /// </summary>
    public class TxtLog : Log
    {
        MTWFile MTWFile = new MTWFile();
        TxtParameter txtParameter = new TxtParameter();
        /// <summary>
        /// 参数设置
        /// </summary>
        public object Parameter
        {
            get
            {
                return txtParameter;
            }
            set
            {
                txtParameter = (TxtParameter)value;
            }
        }


        #region Log 成员



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="_value">日志内容</param>
        /// <param name="addDateTime">是否自动添加时间</param>
        public void WriteLog(string _value, bool addDateTime)
        {
            WriteLog(txtParameter.FullPath, txtParameter.FileLeft, _value, addDateTime);
        }

        #endregion



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="_path">写入路径</param>
        /// <param name="_file">文件前辍</param>
        /// <param name="_value">写入内容</param>
        /// <param name="addDateTime">是否自动添加日期</param>
        public void WriteLog(string _fullPath, string _file, string _value, bool addDateTime)
        {
            try
            {
                string checkPath = _fullPath;
                string strFilePath = checkPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + _file + "log.txt";
                if (System.IO.Directory.Exists(checkPath) == false)
                {
                    System.IO.Directory.CreateDirectory(checkPath);
                }
                if (addDateTime)
                {
                    MTWFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "->" + _value, strFilePath);
                }
                else
                {
                    MTWFile.WriteLine(_value, strFilePath);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = System.IO.File.AppendText(_fullPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogError" + "log.txt"))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "->" + ex.Message.ToString());
                    writer.Close();
                }
            }
        }
    }

    /// <summary>
    /// 多线程写文件
    /// </summary>
    public class MTWFile
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public MTWFile()
        {

        }


        private string _fileName;

        private static Dictionary<long, long> lockDic = new Dictionary<long, long>();

        /// <summary>
        /// 获取或设置文件名称
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Create(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="content">文本内容</param>
        private void Write(string content, string newLine)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                throw new Exception("FileName不能为空！");
            }
            Create(_fileName);
            using (System.IO.FileStream fs = new System.IO.FileStream(_fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, 8, System.IO.FileOptions.Asynchronous))
            {
                //Byte[] dataArray = System.Text.Encoding.ASCII.GetBytes(System.DateTime.Now.ToString() + content + "/r/n");
                Byte[] dataArray = System.Text.Encoding.UTF8.GetBytes(content + newLine);

                bool flag = true;
                long slen = dataArray.Length;
                long len = 0;
                while (flag)
                {
                    try
                    {
                        if (len >= fs.Length)
                        {
                            fs.Lock(len, slen);
                            lockDic[len] = slen;
                            flag = false;
                        }
                        else
                        {
                            len = fs.Length;
                        }
                    }
                    catch
                    {
                        while (!lockDic.ContainsKey(len))
                        {
                            len += lockDic[len];
                        }
                    }
                }
                fs.Seek(len, System.IO.SeekOrigin.Begin);
                fs.Write(dataArray, 0, dataArray.Length);
                fs.Close();
            }
        }



        /// <summary>
        /// 写入文件内容
        /// </summary>
        /// <param name="content"></param>
        public void WriteLine(string content, string filePath)
        {
            _fileName = filePath;
            this.Write(content, System.Environment.NewLine);
        }
    }
}
