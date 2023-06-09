﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EngravingMachineApp
{
    public class Root
    {
        public string name { get; set; }
        public string DevId { get; set; }

        public string ParName { get; set; }

        public string data { get; set; }

        public string DeviceID { get; set; }

        public List<string> RootData()
        {
            List<string> listData = new List<string>();
            listData.Add(data);
            listData.Add(DevId);
            listData.Add(ParName);
            listData.Add(DeviceID);

            return listData;
        }
    }

    public class DataInfo
    {
        public string DevId { get; set; }

        public string ParName { get; set; }

    }
}
