using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BatchClient
{
    public class Configuration
    {
        public Configuration()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            string configPath = Directory.GetCurrentDirectory();
            //path = new DirectoryInfo(@"..\..\..\").FullName;
            configPath = Path.Combine(configPath, "config.txt");

            if (!File.Exists(configPath)) return;

            using (StreamReader sr = new StreamReader(configPath, Encoding.UTF8))
            {
                string line = string.Empty;
                List<string> pathList = new List<string>();
                List<int> intervalTimeList = new List<int>();
                List<string> fileNameList = new List<string>();

                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    if (line.Contains("="))
                    {
                        string[] hostArray = line.Split('=');
                        
                        if(hostArray[0].Trim() == "IP")
                        {
                            string ip = hostArray[1].Trim();
                            DataProxy.SetData(ip);
                        }
                        else if(hostArray[0].Trim() == "Port")
                        {
                            int port = int.Parse(hostArray[1].Trim());
                            DataProxy.SetData(port);
                        }
                    }
                    else if (line.Contains("|"))
                    {
                        string[] splitLine = line.Split('|');
                        int intervalTime = int.Parse(splitLine[0].Trim());
                        string path = splitLine[1].Trim();
                        string fileName = Path.GetFileNameWithoutExtension(path);

                        pathList.Add(path);
                        intervalTimeList.Add(intervalTime);

                        if(!fileNameList.Contains(fileName))
                        {
                            fileNameList.Add(fileName);
                        }
                    }
                }
                DataProxy.SetData(pathList, intervalTimeList, fileNameList);
            }
        }
    }
}
