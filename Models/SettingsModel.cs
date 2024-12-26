using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfHelpers;

namespace BaseApp.Models
{
    public class SettingsModel : ViewModelBase
    {
        public int Id { get; set; }


        private string _ipAddress;
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        private int _port;
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string _ExcelPath;
        public string ExcelPath
        {
            get
            {
                return _ExcelPath;
            }
            set
            {
                _ExcelPath = value;
                OnPropertyChanged("ExcelPath");
            }
        }


        private string pName;

        public string PName
        {
            get { return pName; }
            set { pName = value; OnPropertyChanged(nameof(PName)); }
        }
    }
}
