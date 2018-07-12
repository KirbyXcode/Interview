using System.Collections.Generic;

namespace BatchClient
{
    public static class DataProxy
    {
        private static Data data;

        static DataProxy()
        {
            data = new Data();
        }

        public static void SetData(string ip)
        {
            data.ip = ip;
        }

        public static void SetData(int port)
        {
            data.port = port;
        }

        public static void SetData(string ip, int port)
        {
            data.ip = ip;
            data.port = port;
        }

        public static void SetData(List<string> pathList, List<int> intervalTimeList, List<string> fileNameList)
        {
            data.pathList = pathList;
            data.intervalTimeList = intervalTimeList;
            data.fileNameList = fileNameList;
        }

        public static void SetData(string ip, int port, List<string> pathList, List<int> intervalTimeList, List<string> fileNameList)
        {
            data.ip = ip;
            data.port = port;
            data.pathList = pathList;
            data.intervalTimeList = intervalTimeList;
            data.fileNameList = fileNameList;
        }

        public static string GetIP()
        {
            return data.ip;
        }

        public static int GetPort()
        {
            return data.port;
        }

        public static List<string> GetPathList()
        {
            return data.pathList;
        }

        public static List<int> GetIntervalTimeList()
        {
            return data.intervalTimeList;
        }

        public static List<string> GetFileNameList()
        {
            return data.fileNameList;
        }
    }
}
