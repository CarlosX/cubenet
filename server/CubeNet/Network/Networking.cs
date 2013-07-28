using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CubeNet.Packets;
using CubeNet.Structures;

namespace CubeNet
{
    public partial class Systems
    {
        public class Server
        {
            #region define
            public delegate void dReceive(Decode de);
            public delegate void dConnect(ref object de, Client net);
            public delegate void dError(Exception ex);
            public delegate void dDisconnect(object o);

            public event dConnect OnConnect;
            public event dError OnError;

            public delegate Packet.Base PacketParserCl(Client client, Server _server);
            public Dictionary<int, PacketParserCl> PacketParsers;
            public Dictionary<ulong, Client> Clients;

            TcpListener clientListener;
            #endregion
            public void Start(string _ip, int _port)
            {
                Clients = new Dictionary<ulong, Client>();
                PacketParsers = new Dictionary<int, PacketParserCl>();
                PacketParsers.Add((int)CSPacketIDs.EntityUpdate, Packet.EntityUpdate.Parse);
                PacketParsers.Add((int)CSPacketIDs.Interact, Packet.Interact.Parse);
                PacketParsers.Add((int)CSPacketIDs.Hit, Packet.Hit.Parse);
                PacketParsers.Add((int)CSPacketIDs.Shoot, Packet.Shoot.Parse);
                PacketParsers.Add((int)CSPacketIDs.ClientChatMessage, Packet.ChatMessage.Parse);
                PacketParsers.Add((int)CSPacketIDs.ChunkDiscovered, Packet.UpdateChunk.Parse);
                PacketParsers.Add((int)CSPacketIDs.SectorDiscovered, Packet.UpdateSector.Parse);
                PacketParsers.Add((int)CSPacketIDs.ClientVersion, Packet.ClientVersion.Parse);

                IPEndPoint _endp = new IPEndPoint(IPAddress.Parse(_ip), _port);

                try
                {
                    clientListener = new TcpListener(_endp);
                    clientListener.Start();
                    clientListener.BeginAcceptTcpClient(ClientConnect, null);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode == 10049)
                        LogDebug.Show("ErrorCode: 10049");
                    else if (ex.ErrorCode == 10048)
                        LogDebug.Show("ErrorCode: 10048");
                    else 
                        LogDebug.Show("ErrorCode: {0}", ex.ErrorCode);
                }
                catch (Exception ex) { OnError(ex); }
                finally { }
            }
            private void ClientConnect(IAsyncResult ar)
            {
                try
                {
                    var tcpClient = clientListener.EndAcceptTcpClient(ar);
                    string ip = (tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                    Client newClient = new Client(tcpClient, this);
                    Clients.Add(newClient.ID, newClient);
                    object p = null;
                    try
                    {
                        OnConnect(ref p, newClient);
                        newClient.Packets = p;
                        newClient.IP = ip;
                    }
                    catch (Exception){}
                    try
                    {
                        clientListener.BeginAcceptTcpClient(ClientConnect, null);
                    }
                    catch (SocketException){}
                    catch (Exception){}
                }
                catch (ObjectDisposedException){}
                catch (Exception ex){ OnError(ex); }
            }
            public void HandleRecvPacket(int id, Client client)
            {
                if(id != 0)
                Console.WriteLine("opcode: {0}", id);

                Packet.Base message = PacketParsers[id].Invoke(client, this);
                if (message != null)
                    message.Process();
            }
            public ulong CreateID()
            {
                if (Clients.Count() > 0)
                {
                    for (ulong i = 1; i < Global.Config.max_players; i++)
                    {
                        if (Clients.ContainsKey(i) == false)
                            return i;
                    }
                }
                else
                    return 1;
                return 0;
            }
            public Client[] GetClients()
            {
                return GetClients(null);
            }
            public Client[] GetClients(Client except)
            {
                return Clients.Values.Where(cl => cl != except && cl.Joined).ToArray();
            }
        }
        public class Client
        {
            #region define
            public delegate void dDisconnect(object o);
            public static event dDisconnect OnDisconnect;
            public object Packets { get; set; }
            public bool Joined;
            public NetReader Reader;
            public BinaryWriter Writer;
            public NetworkStream NetStream;
            public ulong ID { get; private set; }
            public Entity Entity;
            public string IP;
            public Server Server { get; private set; }
            private bool disconnecting;
            TcpClient tcp;
            byte[] recvBuffer;
            #endregion
            public Client(TcpClient tcpClient, Server _serv)
            {
                Joined = false;
                Entity = null;
                disconnecting = false;
                tcp = tcpClient;
                IP = (tcp.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                NetStream = tcp.GetStream();
                Reader = new NetReader(NetStream);
                Writer = new BinaryWriter(NetStream);
                Server = _serv;
                ID = Server.CreateID();
                if (ID == 0)
                    ID = Global.Config.max_players + 1;
                recvBuffer = new byte[4];
                NetStream.BeginRead(recvBuffer, 0, 4, HeadCallback, null);
            }
            void HeadCallback(IAsyncResult result)
            {
                if (!tcp.Connected)
                {
                    tcp.Close();
                    return;
                }
                if (disconnecting)
                    return;
                int bytesRead = 0;
                try
                {
                    bytesRead = NetStream.EndRead(result);
                    if (bytesRead == 4)
                    {
                        int op = BitConverter.ToInt32(recvBuffer, 0);
                        Server.HandleRecvPacket(op, this);
                    }
                    NetStream.BeginRead(recvBuffer, 0, 4, HeadCallback, null);
                }
                catch{ LocalDisconnect(tcp); }
            }
            public void Send(byte[] buff)
            {
                try
                {
                    if (buff!=null && buff.Length>0 && tcp.Connected)
                        Writer.Write(buff);
                }
                catch (Exception) {}
            }
            void LocalDisconnect(TcpClient s)
            {
                if (s != null)
                {
                    try
                    {
                        if (OnDisconnect != null)
                            OnDisconnect(this.Packets);
                    }
                    catch (Exception) { }
                }
            }
            public void Disconnect(TcpClient s)
            {
                if (s.Connected)
                    s.Close();
            }
        }
    }
}
