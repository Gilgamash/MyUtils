using MyUtils.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MyUtils.Communication
{
    /// <summary>
    /// Http请求
    /// </summary>
    public class HttpHelper
    {
        #region HttpPost

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>结果</returns>
        public static string Post(string url, string data, int timeout, string contentType, string encoding = "GB2312")
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            var postBytes = Encoding.UTF8.GetBytes(data);
            request.Method = "Post";
            request.ContentType = contentType;
            request.ContentLength = postBytes.Length;
            request.Timeout = timeout;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
            var response = request.GetResponse() as HttpWebResponse;
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <param name="contentType">请求类型</param>
        /// <returns>结果</returns>
        public static string Post(string url, string data, int timeout, string contentType = "application/x-www-form-urlencoded")
        {
            return Post(url, data, timeout, contentType, "GB2312");
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <returns>结果</returns>
        public static string Post(string url, string data, int timeout = 30000)
        {
            return Post(url, data, timeout, "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <returns>结果</returns>
        public static string Post(string url, string data)
        {
            return Post(url, data, 30000);
        }

        #endregion

        #region HttpGet

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>结果</returns>
        public static string Get(string url, int timeout, string contentType, string encoding = "GB2312")
        {
            var result = string.Empty;
            var mRequest = (HttpWebRequest)WebRequest.Create(url);
            mRequest.Timeout = timeout;
            mRequest.ContentType = contentType;
            var mResponse = (HttpWebResponse)mRequest.GetResponse();
            Stream responseStream = mResponse.GetResponseStream();
            if (responseStream != null)
            {
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding(encoding));
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
            }
            return result;
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <param name="contentType">请求类型</param>
        /// <returns>结果</returns>
        public static string Get(string url, int timeout, string contentType = "application/x-www-form-urlencoded")
        {
            return Get(url, timeout, contentType, "GB2312");
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeout">超时时间，毫秒，默认30s</param>
        /// <returns>结果</returns>
        public static string Get(string url, int timeout = 30000)
        {
            return Get(url, timeout, "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>结果</returns>
        public static string Get(string url)
        {
            return Get(url, 30000);
        }

        #endregion

        #region UploadWithParams

        /// <summary>
        /// 上传执行 方法
        /// </summary>
        /// <param name="parameter">上传文件请求参数</param>
        public static string Execute(UploadParameterType parameter)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // 1.分界线
                string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x")),       // 分界线可以自定义参数
                    beginBoundary = string.Format("--{0}\r\n", boundary),
                    endBoundary = string.Format("\r\n--{0}--\r\n", boundary);
                byte[] beginBoundaryBytes = parameter.Encoding.GetBytes(beginBoundary),
                    endBoundaryBytes = parameter.Encoding.GetBytes(endBoundary);
                // 2.组装开始分界线数据体 到内存流中
                memoryStream.Write(beginBoundaryBytes, 0, beginBoundaryBytes.Length);
                // 3.组装 上传文件附加携带的参数 到内存流中
                if (parameter.PostParameters != null && parameter.PostParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in parameter.PostParameters)
                    {
                        string parameterHeaderTemplate = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n{2}", keyValuePair.Key, keyValuePair.Value, beginBoundary);
                        byte[] parameterHeaderBytes = parameter.Encoding.GetBytes(parameterHeaderTemplate);

                        memoryStream.Write(parameterHeaderBytes, 0, parameterHeaderBytes.Length);
                    }
                }
                // 4.组装文件头数据体 到内存流中
                string fileHeaderTemplate = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", parameter.FileNameKey, parameter.FileNameValue);
                byte[] fileHeaderBytes = parameter.Encoding.GetBytes(fileHeaderTemplate);
                memoryStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
                // 5.组装文件流 到内存流中
                byte[] buffer = new byte[1024 * 1024 * 1];
                int size = parameter.UploadStream.Read(buffer, 0, buffer.Length);
                while (size > 0)
                {
                    memoryStream.Write(buffer, 0, size);
                    size = parameter.UploadStream.Read(buffer, 0, buffer.Length);
                }
                // 6.组装结束分界线数据体 到内存流中
                memoryStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                // 7.获取二进制数据
                byte[] postBytes = memoryStream.ToArray();
                // 8.HttpWebRequest 组装
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(parameter.Url, UriKind.RelativeOrAbsolute));
                webRequest.Method = "POST";
                webRequest.Timeout = int.MaxValue;
                webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                webRequest.ContentLength = postBytes.Length;
                if (Regex.IsMatch(parameter.Url, "^https://"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                }
                // 9.写入上传请求数据
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                }
                // 10.获取响应
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), parameter.Encoding))
                    {
                        string body = reader.ReadToEnd();
                        reader.Close();
                        return body;
                    }
                }
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion
    }
}