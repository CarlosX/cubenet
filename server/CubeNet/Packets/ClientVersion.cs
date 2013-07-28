using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeNet.Packets
{
    public partial class Packet
    {
        public class ClientVersion : Base
        {
            public int Version;

            private ClientVersion(int version, Systems.Client client)
                : base(client)
            {
                this.Version = version;
            }

            public static Base Parse(Systems.Client client, Systems.Server _server)
            {
                int version = client.Reader.ReadInt32();
                return new ClientVersion(version, client);
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
                if (Version != 3)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerMismatch);
                    //Sender.Disconnect("Invalid version");
                    return;
                }
                else if (Sender.Server.Clients.Values.Count >= (int)Global.Config.max_players)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerFull);
                    //Sender.Disconnect("Server full");
                    return;
                }

                Sender.Writer.Write(SCPacketIDs.Join); // ServerData
                Sender.Writer.Write(0);
                Sender.Writer.Write(Sender.ID);
                Sender.Writer.Write(new byte[0x1168]);

                Sender.Writer.Write(SCPacketIDs.SeedData);
                Sender.Writer.Write(1234);

            }
        }
    }
}
