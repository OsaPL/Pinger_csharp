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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

        List<Packet> packetsList = new List<Packet>();

        private void packetsniff_Load(object sender, EventArgs e)
        {
            //Form1 connections = new Form1();
            //connections.Show();
        }

        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[4096];
        private bool bContinueCapturing = false;            //A flag to check if packets are to be captured or not

        private void sniffButton_Click(object sender, EventArgs e)
        {
            if (button1.Text == String.Empty)
            {
                button1.PerformClick();
            }
            try
            {
                if (!bContinueCapturing)
                {
                    //Start capturing the packets...

                    sniffButton.Text = "&Stop";

                    bContinueCapturing = true;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

                    //Bind the socket to the selected IP address
                    mainSocket.Bind(new IPEndPoint(IPAddress.Parse(button1.Text), 0));

                    //Set the socket  options
                    mainSocket.SetSocketOption(SocketOptionLevel.IP,            //Applies only to IP packets
                                               SocketOptionName.HeaderIncluded, //Set the include the header
                                               true);                           //option to true

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4] { 1, 0, 0, 0 }; //Capture outgoing packets

                    //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                    mainSocket.IOControl(IOControlCode.ReceiveAll,              //Equivalent to SIO_RCVALL constant
                                                                                //of Winsock 2
                                         byTrue,
                                         byOut);

                    //Start receiving the packets asynchronously
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
                else
                {
                    sniffButton.Text = "&Start";
                    bContinueCapturing = false;
                    //To stop capturing the packets close the socket
                    mainSocket.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "sniff", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);

                //Analyze the bytes received...

                ParseData(byteData, nReceived);

                if (bContinueCapturing)
                {
                    byteData = new byte[4096];

                    //Another call to BeginReceive so that we continue to receive the incoming
                    //packets
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OnReceive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParseData(byte[] byteData, int nReceived)
        {

            try
            {
                //Since all protocol packets are encapsulated in the IP datagram
                //so we start by parsing the IP header and see what protocol data
                //is being carried by it
                IPHeader ipHeader = new IPHeader(byteData, nReceived);

                //Now according to the protocol being carried by the IP datagram we parse 
                //the data field of the datagram
                switch (ipHeader.ProtocolType)
                {
                    case Protocol.TCP:

                        TCPHeader tcpHeader = new TCPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                        //carried by the IP datagram
                                                            ipHeader.MessageLength);//Length of the data field                    


                        //If the port is equal to 53 then the underlying protocol is DNS
                        //Note: DNS can use either TCP or UDP thats why the check is done twice
                        if (tcpHeader.DestinationPort == "53" || tcpHeader.SourcePort == "53")
                        {

                        }
                        newPacketParse(new Packet(ipHeader, tcpHeader));
                        break;

                    case Protocol.UDP:

                        UDPHeader udpHeader = new UDPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                        //carried by the IP datagram
                                                           (int)ipHeader.MessageLength);//Length of the data field                    

                        //If the port is equal to 53 then the underlying protocol is DNS
                        //Note: DNS can use either TCP or UDP thats why the check is done twice
                        if (udpHeader.DestinationPort == "53" || udpHeader.SourcePort == "53")
                        {
                            //Length of UDP header is always eight bytes so we subtract that out of the total 
                            //length to find the length of the data
                        }
                        newPacketParse(new Packet(ipHeader, udpHeader));
                        break;

                    case Protocol.Unknown:
                        break;

                        //Thread safe adding of the packets!!
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "parseData", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            


        }

        private void newPacketParse(Packet packet)
        {
            try
            {
                List<Packet> copy = packetsList;
                if (packet.IP.SourceAddress.ToString() == ipstr || packet.IP.DestinationAddress.ToString() == ipstr)
                {

                    foreach (Packet other in copy)
                    {
                        if (other.IP.SourceAddress.ToString() == packet.IP.SourceAddress.ToString() && other.IP.DestinationAddress.ToString() == packet.IP.DestinationAddress.ToString())
                        {
                            other.Count++;
                            return;
                        }
                        else if (other.IP.SourceAddress.ToString() == packet.IP.DestinationAddress.ToString() && other.IP.DestinationAddress.ToString() == packet.IP.SourceAddress.ToString())
                        {
                            other.Count++;
                            return;
                        }
                    }
                    packetsList = copy;
                    packetsList.Add(packet);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "newPacketParse", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void packetsniff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bContinueCapturing)
            {
                mainSocket.Close();
            }
        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            if (bContinueCapturing)
            {
                //Thread.Sleep(200);
                listBox2.Items.Clear();
                List<Packet> copy = packetsList;
                foreach (Packet packet in copy)
                {
                    listBox2.Items.Add(packet);
                }
                packetsList.Clear();
                //Thread.Sleep(200);

                if (button1.BackColor != Color.Gray)
                {
                    button1.BackColor = Color.Gray;
                }
                else
                {
                    button1.BackColor = SystemColors.Control;
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SendPing();
        }

        private void SendPing()
        {
            try
            {
                List<Packet> copy = packetsList;
                int max = 0;
                string maxip = String.Empty;
                int secondmax = 0;
                foreach (Packet packet in copy)
                {
                    if (packet.Count > max)
                    {
                        secondmax = max;
                        max = packet.Count;
                        if (packet.IP.SourceAddress.ToString() != ipstr)
                        {
                            maxip = packet.IP.SourceAddress.ToString();
                        }
                        else
                        {
                            maxip = packet.IP.DestinationAddress.ToString();
                        }
                    }
                }
                if (maxip != String.Empty && max > secondmax * 10)
                {
                    Ping pingClass = new Ping();
                    PingReply pingReply = pingClass.Send(maxip);
                    label1.Text = pingReply.RoundtripTime.ToString();
                    button2.Text = maxip;
                    if (pingReply.Status != IPStatus.Success)
                    {
                        maxip = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "sendPing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }

    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };

    public class Packet
    {
        public IPHeader IP;
        public UDPHeader UDP;
        public TCPHeader TCP;
        public DNSHeader DNS;
        public int Count = 0;

        public Packet(IPHeader IP, UDPHeader UDP)
        {
            this.IP = IP;
            this.UDP = UDP;
        }
        public Packet(IPHeader IP, TCPHeader TCP)
        {
            this.IP = IP;
            this.TCP = TCP;
        }
        public Packet(IPHeader IP)
        {
            this.IP = IP;
        }
        public override string ToString()
        {
            string info = String.Empty;

            info += IP.SourceAddress + " -> " + IP.DestinationAddress;
            if (UDP != null)
            {
                info += "[UDP]";
            }
            if (TCP != null)
            {
                info += "[TCP]";
            }
            info += "(" + Count + ")";
            return info;
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
