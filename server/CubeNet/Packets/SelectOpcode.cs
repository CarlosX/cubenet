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

                //LogDebug.Show("Opcode: {0}", decode.opcode);
                //LogDebug.HexDump(sys.PacketInformation.buffer,16,false);
                Reader.Skip(4);
                switch (decode.opcode)
                {
                    case 0x00:
                        //Update
                        break;
                    case 0x01:
                        break;
                    case 0x02:
                        break;
                    case 0x06:
                        //Interaction
                        break;
                    case 0x07:
                        //hit npc
                        break;
                    case 0x08:
                        //unk
                        break;
                    case 0x09:
                        //shoot
                        LogConsole.Show("Shoot");
                        break;
                    case 0x0A:
                        //chat msg
                        LogConsole.Show("Msg");
                        break;
                    case 0x0B:
                        //Read Chuk
                        break;
                    case 0x0C:
                        //unk
                        break;
                    case 0x11: 
                        //Client Version
                        int clientvr = Reader.Int32();
                        sys.client.Send(ConnectionInformation(1)); //need add client_id
                        break;
                    case 0x1F:
                        LogConsole.Show("Spell use");
                        break;
                    default:
                        LogConsole.Show("Default Opcode: {0}", decode.opcode);
                        Program.file_log.WriteLine(decode.opcode);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }        
    }
}

