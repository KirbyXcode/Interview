using System.Collections.Generic;

namespace BatchClient
{
    public class Data
    {
        public string ip;
        public int port;
        public List<string> pathList = new List<string>();
        public List<int> intervalTimeList = new List<int>();
        public List<string> fileNameList = new List<string>();
    }
}
