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

                LogDebug.Show("Opcode: {0:X}", decode.opcode);
                LogDebug.HexDump(sys.PacketInformation.buffer);
                switch (decode.opcode)
                {
                    case 17: //Client Version
                        int clientvr = Reader.Int32();
                        LogDebug.Show("ClientV: {0}", clientvr);
                        sys.client.Send(ConnectionInformation(1));
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

