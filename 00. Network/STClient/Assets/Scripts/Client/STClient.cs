using Lidgren.Network;
using System.Threading;

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
				mClient.Recycle(msg);
			}
		}

		public void ShutDown()
		{
			mClient.UnregisterReceivedCallback(new SendOrPostCallback(ProcessIncomingMessage));
			mClient.Shutdown(mServerName);
		}
	}
}
