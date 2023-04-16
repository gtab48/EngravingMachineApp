using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngravingMachineApp
{
    internal class HttpMonitor
    {
        HttpListener httpListener = new HttpListener();
        static HttpServer httpServer = new HttpServer();   

        public HttpMonitor(string url)
        {
            //定义url及端口号，通常设置为配置文件
            httpListener.Prefixes.Add(url);
            //启动监听
            httpListener.Start();
            //异步监听客户端请求，当客户端的网络请求到来时会自动执行Result委托
            //该委托没有返回值，有一个IAsyncResult接口的参数，可通过该参数获取context对象
            httpListener.BeginGetContext(Result, null);
        }

        private void Result(IAsyncResult async)
        {
            //继续异步监听
            httpListener.BeginGetContext(Result, async);
            var guid = Guid.NewGuid().ToString();

            //获得context对象
            var context = httpListener.EndGetContext(async);
            var request = context.Request;
            var response = context.Response;
            //告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
            context.Response.ContentType = "text/plain;charset=UTF-8";
            //添加响应头信息
            context.Response.AddHeader("Content-type", "text/plain");
            context.Response.ContentEncoding = Encoding.UTF8;
            string returnObj = null;
            if (request.HttpMethod == "POST" && request.InputStream != null)
            {
                //处理客户端发送的请求并返回处理信息
                returnObj = HandleRequest(request, response);
            }
            else
            {
                returnObj = $"不是post请求或者传过来的数据为空";
            }
            //设置客户端返回信息的编码
            var returnByteArr = Encoding.UTF8.GetBytes(returnObj);
            try
            {
                using (var stream = response.OutputStream)
                {
                    //把处理信息返回到客户端
                    stream.Write(returnByteArr, 0, returnByteArr.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"网络蹦了：{ex.ToString()}");
            }
        }

        /// <summary>
        /// 返回数据给客户端
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string data = null;
            try
            {
                var byteList = new List<byte>();
                var byteArr = new byte[2048];
                int readLen = 0;
                int len = 0;
                //接收客户端传过来的数据并转成字符串类型
                do
                {
                    readLen = request.InputStream.Read(byteArr, 0, byteArr.Length);
                    len += readLen;
                    byteList.AddRange(byteArr);
                } while (readLen != 0);
                data = Encoding.UTF8.GetString(byteList.ToArray(), 0, len);

                //获取得到数据data可以进行其他操作
            }
            catch (Exception ex)
            {
                response.StatusDescription = "404";
                response.StatusCode = 404;
                return $"在接收数据时发生错误:{ex.ToString()}";//把服务端错误信息直接返回可能会导致信息不安全，此处仅供参考
            }
            response.StatusDescription = "200";//获取或设置返回给客户端的 HTTP 状态代码的文本说明。
            response.StatusCode = 200;// 获取或设置返回给客户端的 HTTP 状态代码。
            return HttpData(data).ToString();

            //return $"接收数据完成";
            //return data;
        }

        private static object HttpData(string data)
        {

            Root jobject = JsonConvert.DeserializeObject<Root>(data);

            //接受方法返回消息
            object message = new object();
            //反射执行HttpSever中的方法
            Type type = typeof(HttpServer);
            object obj = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(jobject.name);
            string[] parameters = jobject.RootData().ToArray();
            parameters = parameters.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            if (method != null)
            {
                try
                {
                    message = method.Invoke(httpServer, parameters);
                }
                catch (Exception ex)
                {
                    return "参数错误";
                }
            }
            else
            {
                return "方法调用失败";
            }
            //jobject.Property("X") == null
            return message;
        }
    }
}
