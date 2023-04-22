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

        internal class DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public uint dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSL_DEFINE.BSL_BUFFER_SIZE)]
            public byte[] dbcc_name;        ////设备名称
                                            // public IntPtr dbcc_name;
        }

        public static bool  stop = true;

        //public Dictionary<string,object> deviceID = new Dictionary<string,object>();


        

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        public string DeviceID()
        {
            string DevID = JsonConvert.SerializeObject(map);
            return DevID;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static string DeviceStop(string DeviceID)
        {
            string stopType = "操作失败";
            if (MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_StopMark func = (BSL_StopMark)MarkAPI.Invoke("StopMark", typeof(BSL_StopMark));
                if (func != null)
                {
                    BslErrCode iRes = func(DeviceID);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {
                        stop = true;
                        return stopType = "当前设备已停止";
                    }
                }
            }
            return stopType;
        }

        ///<summary>
        ///打印
        /// </summary>
        public static string print() 
        {
            string message = "打印失败";
            BSL_DrawFileInImgEx funcDrawFile = (BSL_DrawFileInImgEx)MarkAPI.Invoke("DrawFileInImgEx", typeof(BSL_DrawFileInImgEx));
            if (funcDrawFile != null)
            {
                string fileName = null;
                IntPtr hbitmap = funcDrawFile(fileName, pictureBoxShow.Width, pictureBoxShow.Height, 1, false, false, 10, false);
                message = "打印成功";
            }
            return message;
        }
        


        ///<summary>
        ///加载文件
        /// </summary>
       public static void OpenFile(string fileName)
        {
            string message = "加载文件失败";
            if(MarkAPI.hLib != IntPtr.Zero)
            {
                BSL_LoadDataFile func = (BSL_LoadDataFile)MarkAPI.Invoke("LoadDataFile", typeof(BSL_LoadDataFile));

                if (func != null)
                {
                    BslErrCode iRes = func(fileName);
                    if (iRes == BslErrCode.BSL_ERR_SUCCESS)
                    {

                    }
                }
            }
        }


        ///<summary>
        ///获取文件中所有图形信息
        /// </summary>
        /// 
        public void ShowShapeList(string strFileName) 
        {

        }
    }
}
