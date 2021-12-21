using Lidgren.Network;
using UnityEngine;
using System.Collections.Generic;
using Entity;
using Protocol;
using Manager;

namespace Server
{
    public class STServer
    {
        private NetServer mServer;
        private List<string> mEntityIDList;
        private Dictionary<string, STEntityPostion> mEntityPostionDic;

        public STServer(int iMaxConn, int iPort, string strIP, string strServerName)
        {
            mEntityIDList = new List<string>();
            mEntityPostionDic = new Dictionary<string, STEntityPostion>();

            NetPeerConfiguration config = new NetPeerConfiguration(strServerName);
            config.MaximumConnections = iMaxConn;
            config.LocalAddress = NetUtility.Resolve(strIP);
            config.Port = iPort;
            mServer = new NetServer(config);
        }

        public void StartServer()
        {
            if (null != mServer)
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

                List<NetConnection> all = mServer.Connections;

                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        ProcessStatusChanged(all, msg);
                        break;
                    case NetIncomingMessageType.Data:
                        ProcessData(all, msg);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Debug.Log(msg.ReadString());
                        break;
                    default:
                        Debug.LogError("Unhandled type: " + msg.MessageType + " " + msg.LengthBytes + " bytes " + msg.DeliveryMethod + "|" + msg.SequenceChannel);
                        break;
                }

                mServer.Recycle(msg);
            }
        }

        #region NetIncomingMessageType.StatusChanged
        private void ProcessStatusChanged(List<NetConnection> all, NetIncomingMessage msg)
        {
            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

            string reason = msg.ReadString();

            Debug.LogWarning(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

            string entityID;

            switch (status)
            {
                case NetConnectionStatus.Connected:
                    {
                        entityID = NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier);
                        mEntityIDList.Add(entityID);

                        SendLocalEntityPacket(msg.SenderConnection, entityID);
                        SendSpawnEntitis(all, msg.SenderConnection, entityID);
                    }
                    break;
                case NetConnectionStatus.Disconnected:
                    {
                        // Remove Entity (临时处理)
                        entityID = NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier);
                        STEntityManager.GetInstance().RemoveEntity(entityID);
                    }
                    break;
                default:
                    break;
            }
        }

        public void SendLocalEntityPacket(NetConnection localConnect, string entityID)
        {
            Debug.Log("SendLocalEntityPacket Entity ID: " + entityID);

            NetOutgoingMessage outgoingMessage = mServer.CreateMessage();
            STLocalEntityPacket packet = new STLocalEntityPacket();
            packet.ID = entityID;
            packet.Packet2NetOutgoingMessage(outgoingMessage);
            mServer.SendMessage(outgoingMessage, localConnect, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void SendSpawnEntitis(List<NetConnection> all, NetConnection localConnect, string entityID)
        {
            all.ForEach(p => {
                string pEntityID = NetUtility.ToHexString(p.RemoteUniqueIdentifier);
                if (entityID != pEntityID)
                    SendAllSpawnEntitiesToLocal(localConnect,
                        pEntityID,
                        mEntityPostionDic[pEntityID].X,
                        mEntityPostionDic[pEntityID].Y,
                        mEntityPostionDic[pEntityID].Z);
            });

            System.Random random = new System.Random();
            SendLocalSpawnEntityToAll(all, entityID, random.Next(-3, 3), 0, random.Next(-3, 3));
        }

        public void SendAllSpawnEntitiesToLocal(NetConnection localConnect, string entityID, float X, float Y, float Z)
        {
            Debug.Log("SendAllSpawnEntitiesToLocal Entity ID: " + entityID);

            mEntityPostionDic[entityID] = new STEntityPostion() { X = X, Y = Y, Z = Z };

            NetOutgoingMessage outgoingMessage = mServer.CreateMessage();
            STSpawnEntityPacket packet = new STSpawnEntityPacket();
            packet.ID = entityID;
            packet.X = X;
            packet.Y = Y;
            packet.Z = Z;
            packet.Packet2NetOutgoingMessage(outgoingMessage);

            mServer.SendMessage(outgoingMessage, localConnect, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void SendLocalSpawnEntityToAll(List<NetConnection> all, string entityID, float X, float Y, float Z)
        {
            Debug.Log("SendLocalSpawnEntityToAll Entity ID: " + entityID);

            mEntityPostionDic[entityID] = new STEntityPostion() { X = X, Y = Y, Z = Z };

            NetOutgoingMessage outgoingMessage = mServer.CreateMessage();
            STSpawnEntityPacket packet = new STSpawnEntityPacket();
            packet.ID = entityID;
            packet.X = X;
            packet.Y = Y;
            packet.Z = Z;
            packet.Packet2NetOutgoingMessage(outgoingMessage);
            mServer.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);

            // Create Server Entity
            STEntityManager.GetInstance().CreateEntity("STServerSceneRoot", packet);
        }
        #endregion

        #region NetIncomingMessageType.Data
        private void ProcessData(List<NetConnection> all, NetIncomingMessage msg)
        {
            byte type = msg.ReadByte();

            STPacket stPacket;

            switch (type)
            {
                case (byte)STPacketType.STEntityPositionPacket:
                    stPacket = new STEntityPositionPacket();
                    stPacket.NetIncomingMessage2Packet(msg);
                    SendEntityPositionPacket(all, (STEntityPositionPacket)stPacket);
                    break;
                case (byte)STPacketType.STEntityDisconnectsPacket:
                    stPacket = new STEntityDisconnectsPacket();
                    stPacket.NetIncomingMessage2Packet(msg);
                    SendEntityDisconnectPacket(all, (STEntityDisconnectsPacket)stPacket);
                    break;
                default:
                    Debug.LogError("Unhandled data / packet type: " + type);
                    break;
            }
        }

        public void SendEntityPositionPacket(List<NetConnection> all, STEntityPositionPacket packet)
        {
            Debug.Log("SendEntityPositionPacket Entity ID: " + packet.ID);

            mEntityPostionDic[packet.ID] = new STEntityPostion() { X = packet.X, Y = packet.Y, Z = packet.Z };

            NetOutgoingMessage outgoingMessage = mServer.CreateMessage();
            packet.Packet2NetOutgoingMessage(outgoingMessage);
            mServer.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);

            // Update Entity Position
            STEntityManager.GetInstance().UpdateEntity(packet);
        }

        public void SendEntityDisconnectPacket(List<NetConnection> all, STEntityDisconnectsPacket packet)
        {
            Debug.Log("SendEntityDisconnectPacket Entity ID: " + packet.ID);

            mEntityPostionDic.Remove(packet.ID);
            mEntityIDList.Remove(packet.ID);

            NetOutgoingMessage outgoingMessage = mServer.CreateMessage();
            packet.Packet2NetOutgoingMessage(outgoingMessage);
            mServer.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
        }
        #endregion

        public void Shutdown(string strServerName)
        {
            Debug.Log(strServerName + " Server Shutdown!");

            mServer.Shutdown(strServerName);
        }
    }
}
