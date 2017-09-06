using IPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class packetsniff : Form
    {
        public packetsniff()
        {
            InitializeComponent();
        }

        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        private void button1_Click(object sender, EventArgs e)
        {
            FindBestInterface();
        }
        string ipstr;
        private void FindBestInterface()
        {
            IPAddress ipv4Address = new IPAddress(134744072); //its 8.8.8.8
            UInt32 ipv4AddressAsUInt32 = BitConverter.ToUInt32(ipv4Address.GetAddressBytes(), 0);
            UInt32 index;
            GetBestInterface(ipv4AddressAsUInt32, out index);
            ipstr = "";

            foreach (UnicastIPAddressInformation ip in GetNetworkInterfaceByIndex(index).GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipstr = ip.Address.ToString();
                }
            }

            button1.Text = ipstr;
            textBox1.Text = GetNetworkInterfaceByIndex(index).Name + " " + ipstr;
        }

        private static NetworkInterface GetNetworkInterfaceByIndex(uint index)
        {
            // Search in all network interfaces that support IPv4.
            NetworkInterface ipv4Interface = (from thisInterface in NetworkInterface.GetAllNetworkInterfaces()
                                              where thisInterface.Supports(NetworkInterfaceComponent.IPv4)
                                              let ipv4Properties = thisInterface.GetIPProperties().GetIPv4Properties()
                                              where ipv4Properties != null && ipv4Properties.Index == index
                                              select thisInterface).SingleOrDefault();
            if (ipv4Interface != null)
                return ipv4Interface;

            // Search in all network interfaces that support IPv6.
            NetworkInterface ipv6Interface = (from thisInterface in NetworkInterface.GetAllNetworkInterfaces()
                                              where thisInterface.Supports(NetworkInterfaceComponent.IPv6)
                                              let ipv6Properties = thisInterface.GetIPProperties().GetIPv6Properties()
                                              where ipv6Properties != null && ipv6Properties.Index == index
                                              select thisInterface).SingleOrDefault();

            return ipv6Interface;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        string GetActiveProcessFileName()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.MainModule.FileName;
        }
        uint GetActiveProcessId()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }


        ProcessPorts ports;
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void GetAllProcessPorts(int id)
        {
            try
            {
                var tcpArray = Functions.GetExtendedTcpTable(true, Win32Funcs.TcpTableType.OwnerPidAll).ToList();
                var updArray = Functions.GetExtendedUdpTable(true, Win32Funcs.UdpTableType.OwnerPid).ToList();

                foreach (TcpRow tcp in tcpArray)
                {
                    if (tcp.ProcessId == id)
                    {
                        if (tcp.LocalEndPoint.Address.ToString() == ipstr)
                        {
                            ports.AddPort(tcp.LocalEndPoint.Port, "tcp");
                        }
                    }
                }

                foreach (UdpRow udp in updArray)
                {
                    if (true)//udp.ProcessId == id) //process id is wrong?
                    {
                        if (udp.LocalEndPoint.Address.ToString() == ipstr)
                        {
                            ports.AddPort(udp.LocalEndPoint.Port, "udp");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        int id;

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (id != (int)GetActiveProcessId())
                {
                    id = (int)GetActiveProcessId();
                    button2.Text = GetActiveProcessFileName();
                    ports = new ProcessPorts(GetActiveProcessFileName(), GetActiveProcessId());
                }
                GetAllProcessPorts(id);
                ports.Decay();
                ports.ToListBox(listBox1);
            }
            catch (Exception ex)
            {

            }
        }


    }

    public class ProcessPorts
    {
        string Name;
        uint PID;
        List<int> Ports;
        List<int> KickInterval;
        List<string> Type;

        public ProcessPorts(string Name, uint PID)
        {
            Ports = new List<int>();
            KickInterval = new List<int>();
            Type = new List<string>();

            this.Name = Name;
            this.PID = PID;
        }

        internal void AddPort(int port, string type)
        {
            if (Ports.Count > 0)
            {
                bool found = false;
                int i = 0;
                foreach (int existingPort in Ports)
                {
                    if (existingPort == port)
                    {
                        if (Type[i] == type)
                        {
                            found = true;
                            KickInterval[i] = 100;
                        }
                    }
                    i++;
                }

                if (!found)
                {
                    Ports.Add(port);
                    KickInterval.Add(100);
                    Type.Add(type);
                }
            }
            else
            {
                //if its empty
                Ports.Add(port);
                KickInterval.Add(100);
                Type.Add(type);
            }
        }

        public void KillPort(int index)
        {
            Ports.RemoveAt(index);
            KickInterval.RemoveAt(index);
            Type.RemoveAt(index);
        }

        public void Decay()
        {
            for (int i = 0; i < KickInterval.Count; i++)
            {
                KickInterval[i]--;
                if (KickInterval[i] < 0)
                {
                    KillPort(i);
                }
            }


        }

        public void ToListBox(ListBox listBox)
        {
            listBox.Items.Clear();
            string txt = String.Empty;

            for (int i = 0; i < Ports.Count; i++)
            {
                txt = Ports[i].ToString() + " ";
                txt += Type[i].ToString() + " ";
                txt += KickInterval[i].ToString();

                listBox.Items.Add(txt);
            }
        }

        public bool IsEmpty()
        {
            if (Ports.Count < 1)
            {
                return true;
            }
            return false;
        }
    }
}
