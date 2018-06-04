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
using pinger_csharp;

namespace SmartAdressIpDetectionTest
{
    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };
    public partial class netstatform : Form
    {
        public netstatform()
        {
            InitializeComponent();
        }

        #region getprocess
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        string GetActiveProcessFileName()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                Process p = Process.GetProcessById((int)pid);
                return p.MainModule.FileName;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        uint GetActiveProcessId()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                return pid;
            }
            catch (Exception ex)
            {
                return 9999;
            }
        }
        #endregion
        #region getbestinterface
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        private string FindBestInterface()
        {
            IPAddress ipv4Address = new IPAddress(134744072); //its 8.8.8.8
            UInt32 ipv4AddressAsUInt32 = BitConverter.ToUInt32(ipv4Address.GetAddressBytes(), 0);
            UInt32 index;
            GetBestInterface(ipv4AddressAsUInt32, out index);
            string ipstr = String.Empty;
            foreach (UnicastIPAddressInformation ip in GetNetworkInterfaceByIndex(index).GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipstr = ip.Address.ToString();
                }
            }

            textBoxInterface.Text = GetNetworkInterfaceByIndex(index).Name + " " + ipstr;

            return ipstr;
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
        #endregion
        #region netstat
        #endregion
        #region packetshiffer
        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[4096];
        private bool continueCapturing = false;
        private List<Packet> Packets = new List<Packet>();
        public void ToggleSniffing()
        {
            try
            {
                if (!continueCapturing)
                {
                    //Start capturing the packets...
                    bestIp = FindBestInterface();
                    continueCapturing = true;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

                    //Bind the socket to the selected IP address
                    mainSocket.Bind(new IPEndPoint(IPAddress.Parse(bestIp), 0));

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
                    continueCapturing = false;
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

                if (continueCapturing)
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
                    case pinger_csharp.Protocol.TCP:

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

                    case pinger_csharp.Protocol.UDP:

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

                    case pinger_csharp.Protocol.Unknown:
                        break;

                        //Thread safe adding of the packets!!
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "parseData", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newPacketParse(Packet packet)
        {
            try
            {
                List<Packet> copy = Packets;
                if (packet.IP.SourceAddress.ToString() == bestIp || packet.IP.DestinationAddress.ToString() == bestIp)
                {
                    if (listBoxPackets.Items.Count > listBoxPackets.Height / (listBoxPackets.ItemHeight + 1))
                    {
                        listBoxPackets.Items.RemoveAt(0);
                    }
                    listBoxPackets.Items.Add(packet);
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
                    Packets = copy;
                    Packets.Add(packet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "newPacketParse", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                if (UDP != null)
                {
                    info += "[UDP] ";
                    info += IP.SourceAddress + ":" + UDP.SourcePort + " -> " + IP.DestinationAddress + ":" + UDP.DestinationPort;
                }
                else if (TCP != null)
                {
                    info += "[TCP] ";
                    info += IP.SourceAddress + ":" + TCP.SourcePort + " -> " + IP.DestinationAddress + ":" + TCP.DestinationPort;
                }
                else
                {
                    info += IP.SourceAddress + " -> " + IP.DestinationAddress;
                }
                if (Count > 0)
                {
                    info += "(" + Count + ")";
                }

                return info;
            }
        }
        #endregion
        #region ping
        private void SendPing()
        {
            try
            {
                string maxip = textBoxIp.Text;
                if (maxip != String.Empty)//&& max > secondmax * 10) add something similar!
                {
                    Ping pingClass = new Ping();
                    PingReply pingReply = pingClass.Send(maxip);
                    long ping = pingReply.RoundtripTime;
                    textBoxPing.Text = ping.ToString();
                    if (pingReply.Status != IPStatus.Success)
                    {
                        maxip = String.Empty;
                    }
                    textBoxPing.ForeColor = pingColor(ping);
                }
                else
                {
                    textBoxPing.ForeColor = Color.White;
                    textBoxPing.Text = "Ping";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "sendPing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Color pingColor(long ping) //using 2 diffrent functions to create green to yellow to red spectrum for the ranges 25 to 230 ms.
        {
            if (ping == -1)
            {
                return Color.DarkOrange;
            }
            if (ping <= 1)
            {
                return Color.Aqua;
            }
            else
            {
                if (ping > 230)
                    ping = 230;

                int r = 0, g = 0;
                if (ping < 25)
                {
                    r = 0;
                    g = 255;
                }
                if (ping < 120)
                {
                    r = (int)(2.55 * ping - 51);
                    if (r > 255)
                        r = 255;
                    if (r < 0)
                        r = 0;
                    g = 255;
                }
                if (ping >= 120)
                {
                    r = 255;
                    g = (int)(-2.125 * ping + 510);
                    if (g > 255)
                        g = 255;
                    if (g < 0)
                        g = 0;
                }

                return Color.FromArgb(r, g, 0);
            }
        }
        #endregion

        List<string> ignored = new List<string>();

        string bestIp;
        uint processId;
        List<Port> Ports;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!continueCapturing)
            {
                bestIp = FindBestInterface();
                timerActiveProcess.Enabled = true;
                Ports = NetStatPorts.GetNetStatPorts();
                timerGetPorts.Enabled = true;
                ToggleSniffing();
                timerShowPackets.Enabled = true;
                timerPing.Enabled = true;
                timerIgnoreCheck.Enabled = true;

                buttonStart.Text = "Staph";
            }
            else
            {
                timerActiveProcess.Enabled = false;
                timerGetPorts.Enabled = false;
                ToggleSniffing();
                timerShowPackets.Enabled = false;
                timerPing.Enabled = false;
                timerIgnoreCheck.Enabled = false;

                buttonStart.Text = "Sturt";
            }

        }

        private void timerActiveProcess_Tick(object sender, EventArgs e)
        {
            uint newid = GetActiveProcessId();
            if (processId != newid)
            {
                processId = newid;
                listBoxPackets.Items.Clear();
                listBoxPorts.Items.Clear();
                listBoxSummed.Items.Clear();
            }

            textBoxProcess.Text = "PID:" + processId.ToString();
            textBoxPath.Text = GetActiveProcessFileName();
            if (textBoxPath.Text.Length > 90)
            {
                textBoxPath.Text = textBoxPath.Text.Substring(textBoxPath.Text.Length - 90);
            }
        }

        private void timerGetPorts_Tick(object sender, EventArgs e)
        {
            listBoxPorts.Items.Clear();

            if (Ports.Count > 0)
            {
                foreach (Port port in Ports)
                {
                    if (port.process_pid == processId.ToString())
                        listBoxPorts.Items.Add(port);
                }
            }

        }

        private void timerShowPackets_Tick(object sender, EventArgs e) //separate this into methods!!
        {
            listBoxSummed.Items.Clear();

            FindProcessPackets();
            FindBestDestinationIp();

            Packets.Clear();
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {
            Thread t1 = new Thread(SendPing);
            t1.Start();
        }
        private void timerIgnoreCheck_Tick(object sender, EventArgs e)
        {
            //checks if app should be ignored, add remebering last process active
            foreach (string process in ignored)
            {
                if (textBoxPath.Text.ToLower().Contains(process))
                {
                    labelIgnored.ForeColor = Color.Red;
                    return;
                }
            }
            labelIgnored.ForeColor = SystemColors.Control;
        }

        private void E_Load(object sender, EventArgs e)
        {
            //example ignore list
            ignored.Add("explorer.exe");
            ignored.Add("cmd.exe");
            ignored.Add("iexplore.exe");

            ignored.Add("winrar.exe");
            //ignored.Add("chrome.exe");
            ignored.Add("vlc.exe");
            ignored.Add("devenv.exe");
        }

        private void FindProcessPackets()
        {
            List<Packet> copy = Packets;

            foreach (Packet packet in copy)
            {
                bool found = false;
                string foundPort = String.Empty;

                foreach (Port port in Ports)
                {
                    if (packet.IP.ProtocolType == pinger_csharp.Protocol.TCP)
                    {
                        if (packet.IP.SourceAddress.ToString() == bestIp)
                        {
                            foundPort = packet.TCP.SourcePort;

                            found = true;
                        }
                        else if (packet.IP.DestinationAddress.ToString() == bestIp)
                        {
                            foundPort = packet.TCP.DestinationPort;
                            found = true;
                        }
                    }
                    else if (packet.IP.ProtocolType == pinger_csharp.Protocol.UDP)
                    {
                        if (packet.IP.SourceAddress.ToString() == bestIp)
                        {
                            foundPort = packet.UDP.SourcePort;
                            found = true;
                        }
                        else if (packet.IP.DestinationAddress.ToString() == bestIp)
                        {
                            foundPort = packet.UDP.DestinationPort;
                            found = true;
                        }
                    }
                    if (found)
                    {
                        if (port.port_number == foundPort && port.process_pid == processId.ToString())
                        {
                            listBoxSummed.Items.Add(packet);
                            break;
                        }
                    }
                }
            }

        }

        private void FindBestDestinationIp()
        {
            string maxIp = String.Empty;
            int maxcount = 0;
            foreach (Packet packet in listBoxSummed.Items)
            {
                if (maxcount < packet.Count)
                {
                    maxcount = packet.Count;

                    if (packet.IP.SourceAddress.ToString() == bestIp)
                    {
                        maxIp = packet.IP.DestinationAddress.ToString();
                    }
                    else if (packet.IP.DestinationAddress.ToString() == bestIp)
                    {
                        maxIp = packet.IP.SourceAddress.ToString();
                    }
                }
            }
            textBoxIp.Text = maxIp;
        }

    }
}
