using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;
using pinger_csharp;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loop = true;
            while (loop)
            {
            Console.WriteLine("Active Connections");
            Console.WriteLine();
            Console.WriteLine("  Proto  Local Address          Foreign Address        State         PID");
            foreach (TcpRow tcpRow in ManagedIpHelper.GetExtendedTcpTable(true))
            {
                Process process = Process.GetProcessById(tcpRow.ProcessId);
                Console.WriteLine("  {0,-7}{1,-23}{2, -23}{3,-14}{4,-7}{5}", "TCP", tcpRow.LocalEndPoint, tcpRow.RemoteEndPoint, tcpRow.State, tcpRow.ProcessId, process.ProcessName);

                try
                {
                    /*if (process.ProcessName != "System")
                    {
                        foreach (ProcessModule processModule in process.Modules)
                        {
                            Console.WriteLine("  {0}", processModule.FileName);
                        }

                        Console.WriteLine("  [{0}]", Path.GetFileName(process.MainModule.FileName));
                    }
                    else
                    {
                        Console.WriteLine("  -- unknown component(s) --");
                        Console.WriteLine("  [{0}]", "System");
                    }*/
                }
                catch(Exception ex)
                {
                    Console.WriteLine(process.ProcessName + " error'd");
                }

                Console.WriteLine();
            }

            foreach (UdpRow udpRow in ManagedIpHelper.GetExtendedUdpTable(true))
            {
                
                Process process = Process.GetProcessById(udpRow.ProcessId);
                    try
                    {
                        Console.WriteLine("  {0,-7}{1,-23}{2, -23}{3,-14}{4,-7}{5}", "UDP", udpRow.LocalEndPoint,"","", udpRow.ProcessId, process.ProcessName);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error'd at {0}", process.ProcessName);
                        Console.WriteLine("{0}", ex.ToString());
                    }
            }

                Console.Write("{0}Press any key to continue...", Environment.NewLine);
                Console.ReadKey();

                loop = false;
            }
        }
    }
}
