using Lidgren.Network;

namespace Protocol
{
    public class STLocalEntityPacket : STPacket
    {
        public string ID { get; set; }

        public override void Packet2NetOutgoingMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)STPacketType.STLocalEntityPacket);
            msg.Write(ID);
        }

        public override void NetIncomingMessage2Packet(NetIncomingMessage msg)
        {
            ID = msg.ReadString();
        }
    }
}