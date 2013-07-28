using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeNet
{
    public partial class Systems
    {
        internal Client client;
        //internal Decode PacketInformation;
        //private byte WrongPassword = 1;
        //static short User_Current;

        public Systems(Client de)
        {
            client = de;
        }
    }
}