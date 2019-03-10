using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;

public class ClientTCP : MonoBehaviour
{
    public string IP_ADDRESS;
    public int PORT;

    private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _asyncBuffer = new byte[1024];


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to server...");

        Debug.Log("In Start..");
        ClientHandleNetworkPackets.InitializeNetworkPackages();
        _clientSocket.BeginConnect(IP_ADDRESS, PORT, new AsyncCallback(ConnectCallback), _clientSocket);

        
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        _clientSocket.EndConnect(ar);
        //Once connected, keep listening to the data from the server
        while (true)
        {
            OnReceive();
        }

    }

    private void OnReceive()
    {
        byte[] _sizeInfo = new byte[4];
        byte[] _receivedBuffer = new byte[1024];

        int totalRead = 0;
        int currentRead = 0;

        try
        {
            currentRead = totalRead = _clientSocket.Receive(_sizeInfo);
            if (totalRead <= 0)
            {
                Debug.Log("You are not connected to the server.");
            }
            else
            {

                while (totalRead < _sizeInfo.Length && currentRead > 0)
                {
                    currentRead = _clientSocket.Receive(_sizeInfo, totalRead, _sizeInfo.Length - totalRead, SocketFlags.None);
                    totalRead += currentRead;
                }

                int messageSize = 0;
                messageSize |= _sizeInfo[0];
                messageSize |= (_sizeInfo[1] << 8);
                messageSize |= (_sizeInfo[2] << 16);
                messageSize |= (_sizeInfo[3] << 24);

                byte[] data = new byte[messageSize];

                totalRead = 0;
                currentRead = totalRead = _clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);

                while (totalRead < messageSize && currentRead > 0)
                {
                    currentRead = _clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                    totalRead += currentRead;
                }

                ClientHandleNetworkPackets.HandleNetworkInformation(data);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("You are not connected to the server.");
            Debug.Log(ex);
        }
    }

    public static void SendData(byte[] data)
    {
        _clientSocket.Send(data);
    }

    public static void ThankYouServer()
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CThankYou);
        buffer.WriteString("Thanks for letting me connect to your server.");
        SendData(buffer.ToArray());
        buffer.Dispose();
    }
}
