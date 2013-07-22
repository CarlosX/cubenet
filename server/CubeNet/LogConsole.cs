﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CubeNet
{
    class LogConsole
    {
        public static void Show(string lg1)
        {
            Show(lg1, new object[] {  });
        }
        public static void Show(string lg1,object arg0)
        {
            Show(lg1, new object[] { arg0 });
        }
        public static void Show(string lg1, object arg0, object arg1)
        {
            Show(lg1, new object[] { arg0, arg1 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2)
        {
            Show(lg1, new object[] { arg0, arg1, arg2 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2, object arg3)
        {
            Show(lg1, new object[] { arg0, arg1, arg2, arg3 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2, object arg3, object arg4)
        {
            Show(lg1, new object[] { arg0, arg1, arg2, arg3, arg4 });
        }
        public static void Show(string lg1, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[SERVER]: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(lg1, arg);
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
    class LogDebug
    {
        public static void Show(string lg1)
        {
            Show(lg1, new object[] { });
        }
        public static void Show(string lg1, object arg0)
        {
            Show(lg1, new object[] { arg0 });
        }
        public static void Show(string lg1, object arg0, object arg1)
        {
            Show(lg1, new object[] { arg0, arg1 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2)
        {
            Show(lg1, new object[] { arg0, arg1, arg2 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2, object arg3)
        {
            Show(lg1, new object[] { arg0, arg1, arg2, arg3 });
        }
        public static void Show(string lg1, object arg0, object arg1, object arg2, object arg3, object arg4)
        {
            Show(lg1, new object[] { arg0, arg1, arg2, arg3, arg4 });
        }
        public static void Show(string lg1, params object[] arg)
        {
            if (Program.debug)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[DEBUG]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(lg1, arg);
                Console.Write("\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        //static StreamWriter file_log = new StreamWriter("D:\\GitClones\\cubenet\\server\\CubeNet\\bin\\Debug\\log.txt", false);

        public static void HexDump(byte[] bytes, int bytesPerLine = 16, bool showc = true)
        {
            if (bytes != null)
            {
                int bytesLength = bytes.Length;

                char[] HexChars = "0123456789ABCDEF".ToCharArray();

                int firstHexColumn =
                      8                   // 8 characters for the address
                    + 3;                  // 3 spaces

                int firstCharColumn = firstHexColumn
                    + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                    + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                    + 2;                  // 2 spaces 

                int lineLength = firstCharColumn
                    + bytesPerLine           // - characters to show the ascii value
                    + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

                char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
                int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
                StringBuilder result = new StringBuilder(expectedLines * lineLength);

                for (int i = 0; i < bytesLength; i += bytesPerLine)
                {
                    line[0] = HexChars[(i >> 28) & 0xF];
                    line[1] = HexChars[(i >> 24) & 0xF];
                    line[2] = HexChars[(i >> 20) & 0xF];
                    line[3] = HexChars[(i >> 16) & 0xF];
                    line[4] = HexChars[(i >> 12) & 0xF];
                    line[5] = HexChars[(i >> 8) & 0xF];
                    line[6] = HexChars[(i >> 4) & 0xF];
                    line[7] = HexChars[(i >> 0) & 0xF];

                    int hexColumn = firstHexColumn;
                    int charColumn = firstCharColumn;

                    for (int j = 0; j < bytesPerLine; j++)
                    {
                        if (j > 0 && (j & 7) == 0) hexColumn++;
                        if (i + j >= bytesLength)
                        {
                            line[hexColumn] = ' ';
                            line[hexColumn + 1] = ' ';
                            line[charColumn] = ' ';
                        }
                        else
                        {
                            byte b = bytes[i + j];
                            line[hexColumn] = HexChars[(b >> 4) & 0xF];
                            line[hexColumn + 1] = HexChars[b & 0xF];
                            line[charColumn] = (b < 32 ? '·' : (char)b);
                        }
                        hexColumn += 3;
                        charColumn++;
                    }
                    result.Append(line);
                }
                string fff = result.ToString();
                //file_log.WriteLine(fff);

                if(showc)
                    Console.WriteLine(fff);
            }
        }
        public static string HexDumpS(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes != null)
            {
                int bytesLength = bytes.Length;
                char[] HexChars = "0123456789ABCDEF".ToCharArray();

                int firstHexColumn =
                      8                   // 8 characters for the address
                    + 3;                  // 3 spaces

                int firstCharColumn = firstHexColumn
                    + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                    + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                    + 2;                  // 2 spaces 

                int lineLength = firstCharColumn
                    + bytesPerLine           // - characters to show the ascii value
                    + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

                char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
                int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
                StringBuilder result = new StringBuilder(expectedLines * lineLength);

                for (int i = 0; i < bytesLength; i += bytesPerLine)
                {
                    line[0] = HexChars[(i >> 28) & 0xF];
                    line[1] = HexChars[(i >> 24) & 0xF];
                    line[2] = HexChars[(i >> 20) & 0xF];
                    line[3] = HexChars[(i >> 16) & 0xF];
                    line[4] = HexChars[(i >> 12) & 0xF];
                    line[5] = HexChars[(i >> 8) & 0xF];
                    line[6] = HexChars[(i >> 4) & 0xF];
                    line[7] = HexChars[(i >> 0) & 0xF];

                    int hexColumn = firstHexColumn;
                    int charColumn = firstCharColumn;

                    for (int j = 0; j < bytesPerLine; j++)
                    {
                        if (j > 0 && (j & 7) == 0) hexColumn++;
                        if (i + j >= bytesLength)
                        {
                            line[hexColumn] = ' ';
                            line[hexColumn + 1] = ' ';
                            line[charColumn] = ' ';
                        }
                        else
                        {
                            byte b = bytes[i + j];
                            line[hexColumn] = HexChars[(b >> 4) & 0xF];
                            line[hexColumn + 1] = HexChars[b & 0xF];
                            line[charColumn] = (b < 32 ? '·' : (char)b);
                        }
                        hexColumn += 3;
                        charColumn++;
                    }
                    result.Append(line);
                }
                return result.ToString();
            }
            return "";
        }
    }
}
