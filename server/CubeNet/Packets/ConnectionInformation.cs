using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeNet
{
    public partial class Systems
    {
        public static byte[] ConnectionInformation(long uid)
        {
            PacketWriter Writer = new PacketWriter();
            Writer.Create(16);
            Writer.LWord(uid);
            Writer.AddBuffer(new byte[0x1168]);

            //World Seed
            Writer.DWord(15);
            Writer.DWord(26879); //ts
            return Writer.GetBytes();
        }
    }
}
