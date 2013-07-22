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
        public static int _countc = 0;
        public static void oPCode(Decode decode)
        {
            try
            {
                Systems sys = (Systems)decode.Packet;
                sys.PacketInformation = decode;
                PacketReader Reader = new PacketReader(sys.PacketInformation.buffer);

                //LogDebug.Show("Opcode: {0}", decode.opcode);
                //LogDebug.HexDump(sys.PacketInformation.buffer,16,false);
                Reader.Skip(4);
                switch (decode.opcode)
                {
                    case 0:
                        //Update
                        break;
                    case 6:
                        //Interaction
                        break;
                    case 7:
                        //hit npc
                        break;
                    case 11:
                        //Read Chuk
                        break;
                    case 1684249720:
                        //unk
                        break;
                    case 17: //Client Version
                        int clientvr = Reader.Int32();
                        sys.client.Send(ConnectionInformation(1)); //need add client_id
                        break;
                    default:
                        LogConsole.Show("Default Opcode: {0}", decode.opcode);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }        
    }
}

