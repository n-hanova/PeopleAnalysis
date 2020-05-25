using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCoreLibrary.Startup.Settings
{
    class ConsulSettings
    {
        public string Address { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public List<string> Tags { get; set; }
    }
}
