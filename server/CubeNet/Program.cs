using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using CubeNet.Network;

namespace CubeNet
{
    class Program
    {
        public static bool debug = true;
        public static StreamReader file_opcodes;
        public static string path_exe = "";
        public static List<OpcodeT> OpcodeList = new List<OpcodeT> { };
        public static List<OpcodeT> OpcodeLog = new List<OpcodeT> { };
        public static bool Execut = true;

        static void Main(string[] args)
        {
            Program pro = new Program();
            Definitions.Bootlogo._Load();
            CubeNet.Systems.Ini ini = null;

            path_exe = Environment.CurrentDirectory;
            file_opcodes = new StreamReader(path_exe + "\\opcodes.cfg");

            #region Default Settings
            int LSPort = 12345;
            string LSIP = "192.168.1.5";
            #endregion

            #region Load Settings
            try
            {
                if (File.Exists(Environment.CurrentDirectory + @"\Settings\Settings.ini"))
                {
                    ini = new CubeNet.Systems.Ini(Environment.CurrentDirectory + @"\Settings\Settings.ini");
                    LSPort = Convert.ToInt32(ini.GetValue("Server", "port", 12345));
                    LSIP = ini.GetValue("Server", "ip", "192.168.1.5").ToString();
                    debug = ini.GetValue("CONSOLE", "debug", false);
                    ini = null;
                    LogConsole.Show("Has loaded your ip settings successfully");
                }
                else
                {
                    LogConsole.Show("Settings.ini could not be found, using default setting");
                }
            }
            catch (Exception)
            {
                return;
            }
            #endregion

            #region Load Opcodes
            string _temp_line;
            int _count_op=0;
            while((_temp_line = file_opcodes.ReadLine()) != null)
            {
                try
                {
                    string[] _tmp_split = _temp_line.Split('=');
                    if (_tmp_split[0] != "" && _tmp_split[1] != "")
                    {
                        OpcodeT _tmp_op = new OpcodeT();
                        _tmp_op.opcode = uint.Parse(_tmp_split[0].Replace(" ", ""));
                        _tmp_op.nombre = _tmp_split[1];
                        OpcodeList.Add(_tmp_op);
                        _count_op++;
                    }
                }
                catch { }
            }
            file_opcodes.Close();
            LogConsole.Show("Has loaded {0} Opcodes", _count_op);
            #endregion

            Systems.Server net = new Systems.Server();

            net.OnConnect += new Systems.Server.dConnect(pro._OnClientConnect);
            net.OnError += new Systems.Server.dError(pro._ServerError);

            Systems.Client.OnReceiveData += new Systems.Client.dReceive(pro._OnReceiveData);
            Systems.Client.OnDisconnect += new Systems.Client.dDisconnect(pro._OnClientDisconnect);

            try
            {
                net.Start(LSIP, LSPort);
            }
            catch (Exception ex)
            {
                LogConsole.Show("Starting Server error: {0}", ex);
                Execut = false;
            }
            while (Execut)
            {
                string _command = Console.ReadLine();
                if (_command != "")
                {
                    string[] _dat_c = _command.Split(' ');
                    switch (_dat_c[0])
                    {
                        case "opcode":
                            if (_dat_c[1] == "dump"){
                                
                                using (StreamWriter w = File.AppendText("log_opcode.txt"))
                                {
                                    foreach (OpcodeT ad in OpcodeLog)
                                    {
                                        w.WriteLine(ad.opcode + "=" + ad.nombre);
                                    }
                                    w.Close();
                                }
                                OpcodeLog.Clear();
                            }else if(_dat_c[1] == "count"){
                                LogDebug.Show("OpcodeCount: {0}", OpcodeLog.Count());
                            }
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        }
        public void _OnReceiveData(CubeNet.Systems.Decode de)
        {
            Systems.oPCode(de);
        }
        public void _OnClientConnect(ref object de, CubeNet.Systems.Client net)
        {
            LogConsole.Show("Client Connect!");
            de = new Systems(net);
        }

        public void _OnClientDisconnect(object o)
        {
            LogConsole.Show("Client Disconnect!");
            try
            {
                Systems s = (Systems)o;
                s.client.clientSocket.Close();
            }
            catch { }
        }
        private void _ServerError(Exception ex)
        {
            LogDebug.Show("ServerError: {0}", ex.Message);
        }
    }
}
