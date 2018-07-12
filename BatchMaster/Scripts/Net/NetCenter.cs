using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections;

public class NetCenter : MonoBehaviour
{
    public static NetCenter Instance;

    private Socket socket;

    private string ip;
    private int port;

    private StringBuilder sb = new StringBuilder();

    #region 生命周期

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Connection();

        StartCoroutine(CheckConnection());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Close();
        sb = null;
        Instance = null;
    }

    #endregion

    #region Socket

    //连接
    public void Connection()
    {
        //if (socket != null || socket.Connected) return;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        ip = Configuration.GetContent("IP");
        port = int.Parse(Configuration.GetContent("Port"));

        try
        {
            socket.Connect(ip, port);

#if DEBUG_MODEL
            Debug.Log("Socket连接成功!");
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Close();
        }

    }

    private void Close()
    {
        if (socket != null)
        {
            //socket.Shutdown(SocketShutdown.Both);

            socket.Close();

            socket = null;

#if DEBUG_MODEL
            Debug.Log("Socket已断开!");
#endif
        }
    }

    #endregion

    #region 发送消息封装

    public void SendRequest(string netCode)
    {
        sb.AppendFormat("{0}{1}", netCode, "#");
        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

        try
        {
            socket.Send(bytes);
        }
        catch (Exception)
        {
            //Debug.Log(e);
            Close();
        }

        sb.Remove(0, sb.Length);
    }

    #endregion

    #region Reconnection

    private void Reconnection()
    {
        if (socket == null)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ip, port);
            }
            catch (Exception)
            {
                Close();
            }

#if DEBUG_MODEL
            Debug.Log("正在重连中...");
#endif
        }
    }

    private IEnumerator CheckConnection()
    {
        float checkTime = float.Parse(Configuration.GetContent("CheckTime"));

        while (true)
        {
            yield return new WaitForSeconds(checkTime);

            if (!IsSocketConnected())
            {
                Reconnection();
            }
        }
    }

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
                Close();
            }
        }

        sb.Remove(0, sb.Length);

        return connectState;
    }

    #endregion
}