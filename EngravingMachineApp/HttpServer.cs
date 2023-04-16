using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EngravingMachineApp.Form1;
using static EngravingMachineApp.MarkAPI;

namespace EngravingMachineApp
{
    internal class HttpServer
    {

        private static MarkAPI MarkAPI = new MarkAPI("MarkSDK.dll"); //加载MarkSDK.dll动态库

        private static MarkAPI Calib = new MarkAPI("Calib.dall");//加载Caib.dll动态库

        public bool stop = true;

        public Dictionary<string,object> deviceID = new Dictionary<string,object>();


        

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        public string DeviceID()
        {
            string DevID = JsonConvert.SerializeObject(map);
            return DevID;
        }

        

       
    }
}
