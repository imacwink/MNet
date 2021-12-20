using Lidgren.Network;
using System.Threading;
using Entity;
using UnityEngine;
using Protocol;
using Manager;
using System.Collections.Generic;

namespace Client
{
	public class STClient
	{
		public NetClient mClient { get; set; }

		private string mServerName;

		public STClient(string serverName)
		{
			mServerName = serverName;

			var config = new NetPeerConfiguration(serverName)
			{
				AutoFlushSendQueue = false
			};

			mClient = new NetClient(config);
			mClient.RegisterReceivedCallback(new SendOrPostCallback(ProcessIncomingMessage));

		}

		public void StartClient(int port, string server)
		{
			mClient.Start();
			mClient.Connect(server, port);
		}

		public void ProcessIncomingMessage(object peer)
		{
			NetIncomingMessage msg;
			while ((msg = mClient.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.StatusChanged:
						ProcessStatusChanged(msg);
						break;
					case NetIncomingMessageType.Data:
						ProcessData(msg);
						break;
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.ErrorMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
						Debug.Log(msg.ReadString());
						break;
					default:
						Debug.Log("Unhandled type: " + msg.MessageType + " " + msg.LengthBytes + " bytes");
						break;
				}
				mClient.Recycle(msg);
			}
		}

		#region NetIncomingMessageType.StatusChanged
		private void ProcessStatusChanged(NetIncomingMessage msg)
		{
			NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
			Debug.Log(status.ToString() + ": " + msg.ReadString());
		}
		#endregion

		#region NetIncomingMessageType.Data
		private void ProcessData(NetIncomingMessage msg)
		{
			byte bPacketType = msg.ReadByte();

			Debug.Log("Message type: " + bPacketType);

			STPacket stPacket;

			switch (bPacketType)
			{
				case (byte)STPacketType.STLocalEntityPacket:
					stPacket = new STLocalEntityPacket();
					stPacket.NetIncomingMessage2Packet(msg);
					ProcessLocalEntity((STLocalEntityPacket)stPacket);
					break;
				case (byte)STPacketType.STEntityDisconnectsPacket:
					stPacket = new STEntityDisconnectsPacket();
					stPacket.NetIncomingMessage2Packet(msg);
					ProcessDisconnectEntity((STEntityDisconnectsPacket)stPacket);
					break;
				case (byte)STPacketType.STEntityPositionPacket:
					stPacket = new STEntityPositionPacket();
					stPacket.NetIncomingMessage2Packet(msg);
					ProcessEntityPosition((STEntityPositionPacket)stPacket);
					break;
				case (byte)STPacketType.STSpawnEntityPacket:
					stPacket = new STSpawnEntityPacket();
					stPacket.NetIncomingMessage2Packet(msg);
					ProcessSpawnEntity((STSpawnEntityPacket)stPacket);
					break;
			}
		}

		public void ProcessLocalEntity(STLocalEntityPacket packet)
		{
			Debug.Log("ProcessLocalEntity Enity ID : " + packet.ID);

			STEntityManager.GetInstance().mLocalEntityID = packet.ID;
		}

		public void ProcessDisconnectEntity(STEntityDisconnectsPacket packet)
		{
			Debug.Log("ProcessDisconnectEntity Enity ID : " + packet.ID);

			Dictionary<string, GameObject> entityDic = STEntityManager.GetInstance().mEntityDic;
			if (entityDic.ContainsKey(packet.ID))
			{
				GameObject obj = STEntityManager.GetInstance().mEntityDic[packet.ID];
				if (null != obj)
					MonoBehaviour.Destroy(obj);

				STEntityManager.GetInstance().mEntityDic.Remove(packet.ID);
			}
        }

		public void ProcessEntityPosition(STEntityPositionPacket packet)
		{
			Debug.Log("ProcessEntityPosition Enity ID : " + packet.ID);

			Dictionary<string, GameObject> entityDic = STEntityManager.GetInstance().mEntityDic;
			if (entityDic.ContainsKey(packet.ID))
			{
				GameObject obj = STEntityManager.GetInstance().mEntityDic[packet.ID];
				if (null != obj)
				{
					STMovement stMovement = obj.GetComponent<STMovement>();
					if (null != stMovement)
					{
						stMovement.SetMovePosition(new Vector3(packet.X, packet.Y, packet.Z));
					}
				}
			}
		}

		public void ProcessSpawnEntity(STSpawnEntityPacket packet)
		{
			STEntityManager.GetInstance().SpawnEntity(packet);
		}
		#endregion

		public void ShutDown()
		{
			mClient.UnregisterReceivedCallback(new SendOrPostCallback(ProcessIncomingMessage));
			mClient.Shutdown(mServerName);
		}
	}
}
