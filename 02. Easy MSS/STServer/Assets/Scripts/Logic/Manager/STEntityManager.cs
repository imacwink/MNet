using UnityEngine;
using System.Collections.Generic;
using Common;
using Client;
using Lidgren.Network;
using Protocol;

namespace Manager
{
    public class STEntityManager : STMonoSingleton<STEntityManager>
    {
        public string mLocalEntityID { get; set; }
        public STClient mSTClient { get; set; }
        public Dictionary<string, GameObject> mEntityDic { get; set; }

        public void Init(int iPort, string strServerIP, string strServerName)
        {
            mLocalEntityID = "";
            mSTClient = new STClient(strServerName);
            mSTClient.StartClient(iPort, strServerIP);
            mEntityDic = new Dictionary<string, GameObject>();
        }

        public void SpawnEntity(STSpawnEntityPacket packet)
        {
            Debug.Log("SpawnEntity Enity ID : " + packet.ID);

            GameObject entityObj = (GameObject)Resources.Load("Entity");
            Vector3 position = new Vector3(packet.X, packet.Y, packet.Z);
            Quaternion rotation = new Quaternion();

            GameObject entityInstance = Instantiate(entityObj, position, rotation);

            GameObject entityRoot = GameObject.Find("STClientSceneRoot/GameRoot/EntityRoot");
            if (null != entityRoot)
            {
                entityInstance.transform.parent = entityRoot.transform;
            }

            mEntityDic.Add(packet.ID, entityInstance);

            Transform cameraTrans = entityInstance.transform.Find("Main Camera");

            if (packet.ID == mLocalEntityID)
            {
                entityInstance.AddComponent<STController>();
                entityInstance.transform.name = "LocalEntity";
                if (null != cameraTrans)
                {
                    cameraTrans.gameObject.SetActive(true);
                }
            }
            else
            {
                entityInstance.AddComponent<STMovement>();
                entityInstance.transform.name = packet.ID;
                if (null != cameraTrans)
                {
                    cameraTrans.gameObject.SetActive(false);
                }
            }
        }

        public void SendPosition(float x, float y, float z)
        {
            Debug.Log("SendPosition : [ " + x + "、" + y + "、" + z + "]");

            NetOutgoingMessage msg = mSTClient.mClient.CreateMessage();
            new STEntityPositionPacket() { ID = mLocalEntityID, X = x, Y = y, Z = z }.Packet2NetOutgoingMessage(msg);
            mSTClient.mClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            mSTClient.mClient.FlushSendQueue();
        }

        public void SendDisconnect()
        {
            if (null != mSTClient)
            {
                NetOutgoingMessage msg = mSTClient.mClient.CreateMessage();
                new STEntityDisconnectsPacket() { ID = mLocalEntityID }.Packet2NetOutgoingMessage(msg);
                mSTClient.mClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                mSTClient.mClient.FlushSendQueue();
                mSTClient.mClient.Disconnect("Bye bye!");

                mSTClient.ShutDown();
                mSTClient = null;
            }
        }
    }
}