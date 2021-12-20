using Lidgren.Network;

namespace Protocol
{
    public class STSpawnEntityPacket : STPacket
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string ID { get; set; }
        public override void Packet2NetOutgoingMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)STPacketType.STSpawnEntityPacket);
            msg.Write(X);
            msg.Write(Y);
            msg.Write(Z);
            msg.Write(ID);
        }

        public override void NetIncomingMessage2Packet(NetIncomingMessage msg)
        {
            X = msg.ReadFloat();
            Y = msg.ReadFloat();
            Z = msg.ReadFloat();
            ID = msg.ReadString();
        }
    }
}