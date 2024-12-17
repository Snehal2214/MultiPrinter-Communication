using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Models
{
    public class PrinterSetting
    {
        public string PName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; } // For UI feedback
    }

}
