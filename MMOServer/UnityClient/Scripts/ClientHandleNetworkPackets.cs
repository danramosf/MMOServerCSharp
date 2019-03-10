using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClientHandleNetworkPackets : MonoBehaviour
{
    private delegate void Packet_(byte[] data);
    private static Dictionary<int, Packet_> packets;

    public static void InitializeNetworkPackages()
    {
        Debug.Log("Initialize Network Packages");
        packets = new Dictionary<int, Packet_>
            {
                {(int)ServerPackets.SConnectionOK, HandleConnectionOK }
            };

        
    }

    void Awake()
    {
        Debug.Log("In awake..");
        InitializeNetworkPackages();
    }

    public static void HandleNetworkInformation(byte[] data)
    {
        Debug.Log("In HandleNetworkInformation");

        int packetNum;
        PacketBuffer buffer = new PacketBuffer();

        
        buffer.WriteBytes(data);
        packetNum = (int)buffer.ReadInteger();
        buffer.Dispose();
        //if(Packets.TryGetValue(packetNum, out Packet))
        //{

        //}
        Debug.Log("Before if statement...");
        if (packets.TryGetValue(packetNum, out Packet_ Packet))
        {
            Debug.Log("In if statement...");
            Packet.Invoke(data);
        }
        Debug.Log("Out of if statement..");
    }

    private static void HandleConnectionOK(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();

        //Add your code you want to execute here...
        Debug.Log(msg);

        ClientTCP.ThankYouServer();
    }
}
