using System;
using System.Collections.Generic;
using Bindings;
using MMOServer.Controllers;
using Newtonsoft.Json.Linq;

namespace MMOServer
{
    class ServerHandleNetworkPackets
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages");


            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CThankYou, HandleThankYou },
                {(int)ClientPackets.CAccountCreate, HandleAccountCreate }
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            int packetNum; PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadInteger();
            if (Packets.TryGetValue(packetNum, out Packet_ Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleThankYou(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here...
            Console.WriteLine(msg);
        }

        private static void HandleAccountCreate(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //Add your code you want to execute here...
            Console.WriteLine(msg);
            AccountController accController = new AccountController();

            try
            {
                var jsonMsg = JObject.Parse(msg);
                string email = (string)jsonMsg["email"];
                string password = (string)jsonMsg["password"];

                accController.CreateAccount(email, password);
            }
            catch(Exception ex)
            {
                Console.WriteLine("The message received from the client is in the wrong format. The account was not created.");
            }            
        }
    }
}
