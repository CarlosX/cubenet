using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CubeNet.Structures;
using System.IO;

namespace CubeNet.Packets
{
    public partial class Packet
    {
        public class EntityUpdate : Base
        {
            public Entity Entity;
            public Entity Changes;
            public long UpdateBitmask;
            public static byte[] FullBitmask = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00 };
            public bool IsJoin;

            public EntityUpdate(Entity entity, Entity changes, bool join, Systems.Client client)
                : base(client)
            {
                this.Entity = entity;
                this.Changes = changes;
                this.IsJoin = join;
            }

            public static Base Parse(Systems.Client client, Systems.Server server)
            {
                int length = client.Reader.ReadInt32();

                byte[] compressedData = client.Reader.ReadBytes(length);
                byte[] maskedData = Systems.zlibscript.Uncompress(compressedData);

                Entity entity;

                using (var ms = new MemoryStream(maskedData))
                using (var br = new BinaryReader(ms))
                {
                    ulong id = br.ReadUInt64();

                    if (id != client.ID)
                        throw new NotImplementedException();

                    if (client.Entity == null)
                    {
                        entity = new Entity();

                        client.Entity = entity;
                        entity.ID = client.ID;
                        //server.World.Entities[client.ID] = client.Entity;

                        entity.ReadByMask(br);

                        return new EntityUpdate(client.Entity, client.Entity, true, client);
                    }
                    else
                    {
                        //entity = server.World.Entities[id];

                        Entity changes = new Entity();
                        changes.ReadByMask(br);

                        return new EntityUpdate(client.Entity, changes, false, client);
                    }
                }
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
                Entity.CopyByMask(Changes);
                
                byte[] compressed;
                
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Entity.ID);
                    bw.Write(Changes.LastBitmask);
                    Entity.WriteByMask(Changes.LastBitmask, bw);

                    byte[] uncompressed = ms.ToArray();
                    compressed = Systems.zlibscript.Compress(uncompressed);
                }

                byte[] peercompressed;
                foreach (var peer in Sender.Server.GetClients(Sender))
                {
                    peer.Writer.Write(SCPacketIDs.EntityUpdate);
                    peer.Writer.Write(compressed.Length);
                    peer.Writer.Write(compressed);

                    if (IsJoin)
                    {
                        using (var ms = new MemoryStream())
                        using (var bw = new BinaryWriter(ms))
                        {
                            bw.Write(peer.Entity.ID);
                            bw.Write(Changes.LastBitmask);
                            peer.Entity.WriteByMask(EntityUpdate.FullBitmask, bw);

                            byte[] uncompressed = ms.ToArray();
                            peercompressed = Systems.zlibscript.Compress(uncompressed);
                        }

                        Sender.Writer.Write(SCPacketIDs.EntityUpdate);
                        Sender.Writer.Write(peercompressed.Length);
                        Sender.Writer.Write(peercompressed);
                    }
                }
            }
        }
    }
}
