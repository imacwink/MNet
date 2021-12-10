using Lidgren.Network;
using UnityEngine;

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
                Debug.Log("Recevied msg!");
            }
        }

        public void Shutdown(string strServerName)
        {
            Debug.Log(strServerName + " Server Shutdown!");

            mServer.Shutdown(strServerName);
        }
    }
}
