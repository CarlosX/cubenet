using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;
using System.Security.Cryptography;
using System.Diagnostics;

namespace CubeNet{
    public partial class Systems
    {
        public class zlibscript
        {
            public static byte[] Compress(byte[] data)
            {
                return ZlibStream.CompressBuffer(data);
            }
            public static byte[] Uncompress(byte[] data)
            {
                return ZlibStream.UncompressBuffer(data);
            }
        }
    }
}
