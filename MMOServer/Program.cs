﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer
{

    class Program
    {
        static void Main(String[] args)
        {
            ServerHandleNetworkPackets.InitializeNetworkPackages();
            ServerTCP.SetupServer();

            Console.ReadLine();
        }
    }
}
