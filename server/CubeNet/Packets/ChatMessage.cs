using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeNet.Packets
{
    public partial class Packet
    {
        public class ChatMessage : Base
        {
            public string Message;

            public ChatMessage(string message, Systems.Client client)
                : base(client)
            {
                this.Message = message;
            }

            public static Base Parse(Systems.Client client, Systems.Server Server)
            {
                int length = client.Reader.ReadInt32();
                string message = Encoding.Unicode.GetString(client.Reader.ReadBytes(length * 2));

                if (message.Length > 250)
                    message = message.Substring(0, 250);

                return new ChatMessage(message, client);
            }

            public override bool CallScript()
            {
                return false;
            }

            public override void Process()
            {
                //Sender.Server.World.BroadcastChat(Sender.ID, Message);
            }
        }

    }
}
