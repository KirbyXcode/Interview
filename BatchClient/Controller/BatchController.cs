using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BatchClient
{
    public class BatchController
    {
        public void BatchOpen()
        {
            Console.WriteLine("Batch Opening...");

            List<string> pathList = DataProxy.GetPathList();
            List<int> intervalTimeList = DataProxy.GetIntervalTimeList();

            if (pathList == null || pathList.Count <= 0) return;

            Process pr = new Process();

            ProcessStartInfo info = new ProcessStartInfo();

            for (int i = 0; i < pathList.Count; i++)
            {
                if (!File.Exists(pathList[i])) continue;

                Thread.Sleep(intervalTimeList[i]);

                info.FileName = Path.GetFileName(pathList[i]);

                info.WorkingDirectory = Path.GetDirectoryName(pathList[i]);

                //info.CreateNoWindow = false;
                //info.UseShellExecute = true;

                info.WindowStyle = ProcessWindowStyle.Normal;

                pr.StartInfo = info;

                try
                {
                    pr.Start();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public void BatchClose()
        {
            Console.WriteLine("Batch Closing...");

            List<string> fileNameList = DataProxy.GetFileNameList();

            for (int i = 0; i < fileNameList.Count; i++)
            {
                Process[] prs = Process.GetProcessesByName(fileNameList[i]);
                for (int j = 0; j < prs.Length; j++)
                {
                    if (prs[j].HasExited) continue;
                    prs[j].Kill();
                }
            }
        }
    }
}
