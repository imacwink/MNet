using Lidgren.Network;
using UnityEngine;
using System.Collections.Generic;

namespace Server
{
    public class STServer
    {
        private NetServer mServer;

        public STServer(int iMaxConn, int iPort, string strIP, string strServerName)
        {
            NetPeerConfiguration config = new NetPeerConfiguration(strServerName);
            config.MaximumConnections = iMaxConn;
            config.LocalAddress = NetUtility.Resolve(strIP);
            config.Port = iPort;
            mServer = new NetServer(config);
        }

        public void StartServer()
        {
            if (mServer != null)
            {
                mServer.Start();
            }
            else
            {
                Debug.Log("Server is not instance!");
            }
        }

        public void ProcessServerListen()
        {
            NetIncomingMessage msg;
            while ((msg = mServer.ReadMessage()) != null)
            {
                List<NetConnection> all = mServer.Connections;
                Debug.Log("Recevied msg! mServer.Connections cnt ：" + all.Count);
            }
        }

        public void Shutdown(string strServerName)
        {
            Debug.Log(strServerName + " Server Shutdown!");

            mServer.Shutdown(strServerName);
        }
    }
}
