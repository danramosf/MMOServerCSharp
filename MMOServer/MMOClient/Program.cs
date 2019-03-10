using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHandleNetworkPackets.InitializeNetworkPackages();
            ClientTCP.ConnectToServer();

            Console.ReadLine();
        }
    }
}
