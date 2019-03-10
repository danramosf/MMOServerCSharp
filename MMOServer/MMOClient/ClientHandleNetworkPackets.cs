using System;
using System.Collections.Generic;
using Bindings;

namespace MMOClient
{
    class ClientHandleNetworkPackets
    {
        private delegate void Packet_(byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages");


            Packets = new Dictionary<int, Packet_>
            {
                {(int)ServerPackets.SConnectionOK, HandleConnectionOK }
            };
        }

        public static void HandleNetworkInformation(byte[] data)
        {
            int packetNum; PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            if(Packets.TryGetValue(packetNum, out Packet_ Packet))
            {
                Packet.Invoke(data);
            }
        }

        private static void HandleConnectionOK(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here...
            Console.WriteLine(msg);

            ClientTCP.ThankYouServer();
        }
    }
}
