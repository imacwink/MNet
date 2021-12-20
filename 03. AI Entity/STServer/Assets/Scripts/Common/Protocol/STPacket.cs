using Lidgren.Network;

namespace Protocol
{
    public abstract class STPacket : ISTPacket
    {
        public abstract void Packet2NetOutgoingMessage(NetOutgoingMessage msg);
        public abstract void NetIncomingMessage2Packet(NetIncomingMessage msg);
    }
}
