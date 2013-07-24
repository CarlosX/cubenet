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
                byte[] op_hex = Reader.Bytes(4);
                //Reader.Skip(4);
                switch (decode.opcode)
                {
                    case 0x00:
                        {
                            //Update
                            int size = (int)Reader.UInt32();
                            byte[] data = zlibscript.Uncompress(Reader.Bytes(size));
                            PacketReader pkr = new PacketReader(data);
                            ulong entryu = pkr.UInt64();
                        }
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
                    case 0xA:
                        {
                            //chat msg
                            int msg_size = (int)Reader.UInt32();
                            //byte[] msg_data = Reader.Bytes(msg_size * 2);
                            LogConsole.Show("Msg: sz {0} pkl: {1}", (msg_size * 2), decode.dataSize);
                        }
                        break;
                    case 0x0B:
                        //ChunkDiscovered
                        break;
                    case 0x0C:
                        //SectorDiscovered
                        break;
                    case 0x11:
                        {
                            //Client Version
                            int clientvr = Reader.Int32();
                            sys.client.Send(ConnectionInformation(1)); //need add client_id
                        }
                        break;
                    case 0x1F:
                        LogConsole.Show("Spell use");
                        break;
                    default:
                        
                        bool opexits = Program.OpcodeLog.Exists(a => a.opcode == decode.opcode);
                        bool opload_exi = Program.OpcodeList.Exists(a => a.opcode == decode.opcode);
                        if (!opexits && !opload_exi)
                        {
                            OpcodeT tmm = new OpcodeT();
                            tmm.opcode = decode.opcode;
                            tmm.nombre = "unk: "+ BitConverter.ToString(op_hex);
                            Program.OpcodeLog.Add(tmm);
                        }
                        else
                        {
                            //LogConsole.Show("Default Opcode: {0}", decode.opcode);
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
        }        
    }
}

