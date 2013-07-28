﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CubeNet.Structures;

namespace CubeNet.Packets
{
    public partial class Packet
    {

        public enum InteractType : byte
        {
            NPC = 2,
            Normal = 3,
            Pickup = 5,
            Drop = 6,
            Examine = 8
        }

        public class Interact : Base
        {
            public Item Item;
            public int ChunkX, ChunkY;
            public int ItemIndex; // Index of item in ChunkItems.
            public uint Something4;
            public InteractType InteractType;
            public byte Something6;
            public ushort Something7;

            public Interact(Systems.Client client)
                : base(client)
            {
            }

            public static Base Parse(Systems.Client client, Systems.Server server)
            {
                Item Item = new Item();
                Item.Read(client.Reader);
                return new Interact(client)
                {
                    Item = Item,
                    ChunkX = client.Reader.ReadInt32(),
                    ChunkY = client.Reader.ReadInt32(),
                    ItemIndex = client.Reader.ReadInt32(),
                    Something4 = client.Reader.ReadUInt32(),
                    InteractType = (InteractType)client.Reader.ReadByte(),
                    Something6 = client.Reader.ReadByte(),
                    Something7 = client.Reader.ReadUInt16()
                };
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
                //BinaryWriter writer = Sender.Writer;
                //
                //Item.Write(writer);
                //writer.Write(ChunkX);
                //writer.Write(ChunkY);
                //writer.Write(ItemIndex);
                //writer.Write(Something4);
                //writer.Write((byte)InteractType);
                //writer.Write(Something6);
                //writer.Write(Something7);
            }
        }
    }
}
