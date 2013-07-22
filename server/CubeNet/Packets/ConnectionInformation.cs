using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeNet
{
    public partial class Systems
    {
        public static byte[] ConnectionInformation(ulong uid)
        {
            PacketWriter Writer = new PacketWriter();
            Writer.Create(16);
            Writer.DWord(0);
            Writer.LWord((ulong)1);
            Writer.Bytes(new byte[0x1168]);

            Writer.DWord(15);
            Writer.DWord(26879);
            return Writer.GetBytes();
        }
    }
}
