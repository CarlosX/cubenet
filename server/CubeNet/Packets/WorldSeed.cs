using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeNet
{
    public partial class Systems
    {
        public static byte[] WorldSeed()
        {
            PacketWriter Writer = new PacketWriter();

            Writer.Create(15);
            Writer.DWord(26879); //ts

            return Writer.GetBytes();
        }
    }
}
