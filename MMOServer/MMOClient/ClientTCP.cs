using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace MMOClient
{
    class ClientTCP
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] _asyncBuffer = new byte[1024];

        public static void ConnectToServer()
        {
            _clientSocket.BeginConnect("127.0.0.1", 5555, new AsyncCallback(ConnectCallback), _clientSocket);

        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            _clientSocket.EndConnect(ar);
            //Once connected, keep listening to the data from the server
            while (true)
            {
                OnReceive();
            }
            
        }

        private static void OnReceive()
        {
            byte[] _sizeInfo = new byte[4];
            byte[] _receivedBuffer = new byte[1024];

            int totalRead = 0, currentRead = 0;

            try
            {
                currentRead = totalRead = _clientSocket.Receive(_sizeInfo);
                if(totalRead <= 0)
                {
                    Console.WriteLine("You are not connected to the server.");
                }
                else
                {
                    
                    while(totalRead < _sizeInfo.Length && currentRead > 0)
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

                    while(totalRead < messageSize && currentRead > 0)
                    {
                        currentRead = _clientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                        totalRead += currentRead;
                    }

                    ClientHandleNetworkPackets.HandleNetworkInformation(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("You are not connected to the server.");
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
}
