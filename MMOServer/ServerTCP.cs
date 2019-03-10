using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace MMOServer
{

    class ServerTCP
    {
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];

        //The number of clients we want to connect to the server goes in the array size.
        public static Client[] _clients = new Client[Constants.MAX_PLAYERS];

        public static void SetupServer()
        {

            for(int i=0; i < Constants.MAX_PLAYERS; i++)
            {
                _clients[i] = new Client();
            }

            //Accept any IP address on port 5555
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5555));
            //The number of pending requests (queue) for connections we can have at the same time goes in the args here \/
            _serverSocket.Listen(10);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }

        //Once we have a pending connection, the AcceptCallback will be called.
        private static void AcceptCallback(IAsyncResult ar)
        {
            //The client is now connecting to the server
            Socket socket = _serverSocket.EndAccept(ar);
            //Once the client is connectec, we are opening to other connections.
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //Placing the player in the first 'slot' available.
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if(_clients[i].socket == null)
                {
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    _clients[i].StartClient();
                    Console.WriteLine("Connection from '{0}' received.", _clients[i].ip);
                    SendConnectionOK(i);
                    return;
                }
            }
        }

        //Send data from the server to the client
        public static void SendDataTo(int index, byte[] data)
        {

            byte[] sizeInfo = new byte[4];
            sizeInfo[0] = (byte)data.Length;
            sizeInfo[1] = (byte)(data.Length >> 8);
            sizeInfo[2] = (byte)(data.Length >> 16);
            sizeInfo[3] = (byte)(data.Length >> 24);

            _clients[index].socket.Send(sizeInfo);
            _clients[index].socket.Send(data);

        }

        public static void SendConnectionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("You have been successfully connected to the server.");
            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }
    }

    class Client
    {
        public int index;
        public string ip;
        public Socket socket;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];

        public void StartClient()
        {
            //Make this client available to receive data.
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            closing = false;
        }

        //Once we receive Data from the server...
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            //Once a client is disconnected from the server, the server is still online for other clients.
            try
            {
                //The byte length of the data we are receiving
                int received = socket.EndReceive(ar);
                if(received <= 0)
                {
                    CloseClient(index);
                }
                else
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(_buffer, dataBuffer, received);
                    ServerHandleNetworkPackets.HandleNetworkInformation(index, dataBuffer);
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
            }
            catch (Exception ex)
            {
                CloseClient(index);
            }
        }

        private void CloseClient(int index)
        {
            closing = true;
            Console.WriteLine("Connection from '{0}' has been terminated.", ip);
            //PlayerLeftGame();
            socket.Close();
        }
    }
}