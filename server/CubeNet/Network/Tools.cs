using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeNet
{
    public partial class Systems
    {
        public static class Tools
        {
            public static string HexStr(byte[] data)
            {
                char[] lookup = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                int i = 0, p = 2, l = data.Length;
                char[] c = new char[l * 2 + 2];
                byte d; c[0] = '0'; c[1] = 'x';
                while (i < l)
                {
                    d = data[i++];
                    c[p++] = lookup[d / 0x10];
                    c[p++] = lookup[d % 0x10];
                }
                return new string(c, 0, c.Length);
            }
            const string formatter = "{0,10}{1,13}";
        }
    }
}
