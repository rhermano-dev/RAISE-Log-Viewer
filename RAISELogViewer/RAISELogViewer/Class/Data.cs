using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagramTestApp.Class
{
    internal class Data
    {
        public string Time { get; set; }
        public string LogType { get; set; }
        public string MessageType { get; set; }
        public string JsonString { get; set; }
    }

    static class MainData
    {
        public static string Time;
        public static string FilePath;
    }

}
