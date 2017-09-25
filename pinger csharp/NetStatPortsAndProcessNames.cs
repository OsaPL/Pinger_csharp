// ===============================================
// The Method That Parses The NetStat Output
// And Returns A List Of Port Objects
// ===============================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
namespace pinger_csharp
{
    public class NetStatPorts
    {
        public static List<Port> GetNetStatPorts()
        {
            List<Port> Ports = new List<Port>();

            try
            {
                using (Process p = new Process())
                {

                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.Arguments = "-a -n -o";
                    ps.FileName = "netstat.exe";
                    ps.UseShellExecute = false;
                    ps.WindowStyle = ProcessWindowStyle.Hidden;
                    ps.RedirectStandardInput = true;
                    ps.RedirectStandardOutput = true;
                    ps.RedirectStandardError = true;
                    ps.CreateNoWindow = true;   //Small thing to keep window from flashing

                    p.StartInfo = ps;
                    p.Start();

                    StreamReader stdOutput = p.StandardOutput;
                    StreamReader stdError = p.StandardError;

                    string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    string exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0")
                    {
                        return new List<Port>();
                    }

                    //Get The Rows
                    string[] rows = Regex.Split(content, "\r\n");
                    foreach (string row in rows)
                    {
                        //Split it baby
                        string[] tokens = Regex.Split(row, "\\s+");
                        if (tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                        {
                            string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                            Ports.Add(new Port
                            {
                                protocol = localAddress.Contains("1.1.1.1") ? String.Format("{0}v6", tokens[1]) : String.Format("{0}v4", tokens[1]),
                                port_number = localAddress.Split(':')[1],
                                process_name = tokens[1] == "UDP" ? LookupProcess(Convert.ToInt16(tokens[4])) : LookupProcess(Convert.ToInt16(tokens[5])),
                                process_pid = tokens[1] == "UDP" ? Convert.ToInt16(tokens[4]).ToString() : Convert.ToInt16(tokens[5]).ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<Port>();
            }
            return Ports;
        }
        public static string LookupProcess(int pid)
        {
            string procName;
            try { procName = Process.GetProcessById(pid).ProcessName; }
            catch (Exception) { procName = "-"; }
            return procName;
        }
    }

    // ===============================================
    // The Port Class We're Going To Create A List Of
    // ===============================================
    public class Port
    {
        public string name
        {
            get
            {
                return string.Format("{0}:{1} ({2} port {3})", this.process_name, this.process_pid, this.protocol, this.port_number);
            }
            set { }
        }
        public string port_number { get; set; }
        public string process_name { get; set; }
        public string process_pid { get; set; }
        public string protocol { get; set; }
        public override string ToString()
        {
            return string.Format("{0}:{1} ({2} port {3})", this.process_name, this.process_pid, this.protocol, this.port_number);
        }
    }
}