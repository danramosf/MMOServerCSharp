using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    //get send from server to client
    //Client has to listen to serverpackets
    public enum ServerPackets
    {
        SConnectionOK = 1,
        SLoginResponse = 2,
    }

    //get send from client to server
    //server has to listen to clientpackets
    public enum ClientPackets
    {
        CThankYou = 1,
        CLoginRequest = 2,
        CAccountCreate = 3,
    }
}
