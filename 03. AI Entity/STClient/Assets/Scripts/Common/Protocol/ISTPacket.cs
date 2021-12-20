using Lidgren.Network;

namespace Protocol
{
    public interface ISTPacket
    {
        void Packet2NetOutgoingMessage(NetOutgoingMessage msg);
        void NetIncomingMessage2Packet(NetIncomingMessage msg);
    }
}
