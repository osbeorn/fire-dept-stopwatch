using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDeptStopwatch.Classes
{
    public class CameraInfo
    {
        private string _url;
        public string Url {
            get {
                return _url;
            }
            set
            {
                if (value.StartsWith("http://"))
                {
                    _url = value.Replace("http://", "");
                }
                else
                {
                    _url = value;
                }
            }
        }

        public string StreamUrl { get; set; }

        // credentials
        public string User { get; set; }
        public string Password { get; set; }

        // other properties
        public bool HdRecording { get; set; }

        public override string ToString()
        {
            if (User != null && Password != null)
            {
                return "http://" + User + ":" + Password + "@" + Url;
            }
            else
            {
                return "http://" + Url;
            }
        }
    }
}
