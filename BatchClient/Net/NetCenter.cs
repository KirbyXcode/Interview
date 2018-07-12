using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BatchClient
{
    public class NetCenter
    {
        private Socket socket;
        private Thread processThread;

        const int BUFFER_SIZE = 1024;

        public byte[] readBuff = new byte[BUFFER_SIZE];

        private string lastMsg;
        private string endChar = "#";
        private bool canIndex = true;
        private bool canProcess = true;

        private Queue<string> msgQueue;
        private BatchController controller;
        private StringBuilder sb;

        private string ip;
        private int port;

        public NetCenter()
        {
            Connetion();

            msgQueue = new Queue<string>();
            controller = new BatchController();
            sb = new StringBuilder();

            processThread = new Thread(ProcessFunc);
            processThread.Start();

            CheckConnection();
        }

        #region Socket

        //连接
        public void Connetion()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ip = DataProxy.GetIP();
            port = DataProxy.GetPort();

            try
            {
                socket.Connect(ip, port);
                socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);

                Console.WriteLine("连接成功！");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close("连接失败！");
            }
        }

        //接收回调
        private void ReceiveCb(IAsyncResult ar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;

                //count是接收数据的大小
                int count = socket.EndReceive(ar);

                if (count == 0)
                {
                    Close("连接断开！");
                }

                //数据处理
                string str = Encoding.UTF8.GetString(readBuff, 0, count);

                msgQueue.Enqueue(str);
                //继续接收	
                socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Close("连接断开！");
            }
        }

        private void Close(string msg)
        {
            if (processThread != null)
            {
                //canProcess = false;
                processThread.Abort();
                processThread = null;
            }

            //if (msgQueue != null)
            //    msgQueue = null;

            //if (controller != null)
            //    controller = null;

            if (socket != null)
            {
                socket.Close();
                socket = null;
            }

            Console.WriteLine(msg);
        }

        void ProcessFunc()
        {
            while (canProcess)
            {
                if (msgQueue.Count > 0)
                {
                    string dataTotal = (string)msgQueue.Dequeue();
                    dataTotal = lastMsg + dataTotal;
                    lastMsg = null;
                    if (string.IsNullOrEmpty(dataTotal)) continue;
                    int lastIndex = 0;
                    int index = 0;
                    int count = 0;
                    string fullMsg;
                    int hasCutLength = 0;
                    int subLength = 0;

                    while ((index = dataTotal.IndexOf(endChar, index)) != -1 && canIndex)
                    {
                        subLength = index - lastIndex;
                        fullMsg = dataTotal.Substring(lastIndex, subLength);

                        OperationResponse(fullMsg);

                        hasCutLength += fullMsg.Length;
                        count++;
                        index = index + endChar.Length;
                        lastIndex = index;
                    }

                    lastMsg = dataTotal.Substring(lastIndex, dataTotal.Length - lastIndex);
                }
            }
        }

        #endregion

        #region 接收消息封装

        private void OperationResponse(string code)
        {
            if (code == NetCode.BATCH_OPEN_SRES)
            {
                controller.BatchOpen();
            }
            else if (code == NetCode.BATCH_CLOSE_SRES)
            {
                controller.BatchClose();
            }
        }

        #endregion

        #region Reconnection

        private bool IsSocketConnected()
        {
            if (socket == null) return false;

            bool connectState = true;

            sb.AppendFormat("{0}{1}", NetCode.CHECK_CONN_CREQ, "#");
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

            try
            {
                socket.Send(bytes);

                connectState = true;
            }
            catch (SocketException e)
            {
                //10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    connectState = true;
                }
                else
                {
                    connectState = false;
                    Close("连接断开！");
                }
            }

            sb.Remove(0, sb.Length);

            return connectState;
        }

        private void Reconnection()
        {
            if (socket == null)
            {
                Console.WriteLine("正在重连！");

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socket.Connect(ip, port);

                    processThread = new Thread(ProcessFunc);
                    processThread.Start();

                    socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);

                    Console.WriteLine("重连成功！");
                }
                catch (Exception)
                {
                    Close("重连失败！");
                }
            }
        }

        private void CheckConnection()
        {
            while (true)
            {
                Thread.Sleep(10000);

                if (!IsSocketConnected())
                {
                    Reconnection();
                }
            }
        }

        #endregion
    }
}