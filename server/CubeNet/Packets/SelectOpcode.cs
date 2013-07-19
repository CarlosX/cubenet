using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CubeNet
{
    public partial class Systems
    {        
        public static void oPCode(Decode decode)
        {
            try
            {
                Systems sys = (Systems)decode.Packet;
                sys.PacketInformation = decode;
                PacketReader Reader = new PacketReader(sys.PacketInformation.buffer);

                LogDebug.Show("Opcode: {0}", decode.opcode);
                LogDebug.HexDump(sys.PacketInformation.buffer);
                switch (decode.opcode)
                {
                    case 0x11: //ChunkDiscovered
                        Reader.Skip(2);
                        //int - 2 bytes : Chunk X
                        //int - 2 bytes : Chunk y
                        int chunkx = Reader.Int16();
                        int chunky = Reader.Int16();
                        LogDebug.Show("Chunk: X: {0} Y: {1}", chunkx, chunky);
                        break;
                    default:
                        LogConsole.Show("Default Opcode: {0:X}", decode.opcode);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }        
    }
}

