using Common;
using Client;
using UnityEngine;
using Lidgren.Network;
using Protocol;

namespace Manager
{
    public class STClientNetworkManager : STMonoSingleton<STClientNetworkManager>
    {
        public STClient mSTClient { get; set; }

        public void Init(int iPort, string strServerIP, string strServerName)
        {
            mSTClient = new STClient(strServerName);
            mSTClient.StartClient(iPort, strServerIP);
        }

        public void SendPosition(float x, float y, float z)
        {
            Debug.Log("SendPosition : [ " + x + "、" + y + "、" + z + "]");

            NetOutgoingMessage msg = mSTClient.mClient.CreateMessage();
            string strLocalEntityID = STEntityManager.GetInstance().mLocalEntityID;
            new STEntityPositionPacket() { ID = strLocalEntityID, X = x, Y = y, Z = z }.Packet2NetOutgoingMessage(msg);
            mSTClient.mClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            mSTClient.mClient.FlushSendQueue();
        }

        public void SendDisconnect()
        {
            if (null != mSTClient)
            {
                NetOutgoingMessage msg = mSTClient.mClient.CreateMessage();
                string strLocalEntityID = STEntityManager.GetInstance().mLocalEntityID;
                new STEntityDisconnectsPacket() { ID = strLocalEntityID }.Packet2NetOutgoingMessage(msg);
                mSTClient.mClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                mSTClient.mClient.FlushSendQueue();
                mSTClient.mClient.Disconnect("Bye bye!");

                mSTClient.ShutDown();
                mSTClient = null;
            }
        }
    }
}
