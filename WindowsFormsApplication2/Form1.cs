using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using IPHelper;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.PerformClick();

            if (!checkBox1.Checked)
            {
                Thread thread = new Thread(new ThreadStart(SendPing));
                thread.Start();
            }

        }

        private void SendPing()
        {
            if (ip != String.Empty)
            {
                Ping pingClass = new Ping();
                PingReply pingReply = pingClass.Send(ip);
                button1.Text = pingReply.RoundtripTime.ToString();
                if (pingReply.Status != IPStatus.Success)
                {
                    ip = String.Empty;
                }
            }
        }
        string ip = String.Empty;
        int lastId;
        private void GetActiveProcessesConnections(object sender, EventArgs e)
        {
            try
            {
                string name = GetActiveProcessFileName();

                uint id = GetActiveProcessId();
                if (id != lastId)
                {
                    ip = String.Empty;
                    lastId = (int)id;
                    button1.Text = String.Empty;
                    IpCounting.Clear();
                }

                textBox1.Text = name + "\r\nID:" + id;
                textBox2.Clear();

                var tcpArray = Functions.GetExtendedTcpTable(true, Win32Funcs.TcpTableType.OwnerPidAll).ToList();
                var updArray = Functions.GetExtendedUdpTable(true, Win32Funcs.UdpTableType.OwnerPid).ToList();
                listBox1.Items.Clear();

                if (checkBox2.Checked)
                {
                    timer1.Interval = 5000;
                    foreach (UdpRow udp in updArray)
                    {
                        //if (udp.ProcessId == id)
                            listBox1.Items.Add(udp);
                    }
                }
                else
                {
                    foreach (TcpRow tcp in tcpArray)
                    {
                        if (tcp.ProcessId == id)
                        {
                            if ((!tcp.RemoteEndPoint.ToString().Contains("127.0.0.1") && !tcp.RemoteEndPoint.ToString().Contains("0.0.0.0")) || checkBox1.Checked)
                            {
                                listBox1.Items.Add(tcp);

                                int max = 0;
                                string maxip = "";
                                foreach (IPcounter ip in IpCounting)
                                {
                                    if (ip.Count > max)
                                    {
                                        max = ip.Count;
                                        maxip = ip.Ip;
                                    }
                                }
                                if (max > 20)
                                {
                                    ip = maxip;
                                }

                                textBox1.Text += "\r\n" + ip + " Count:" + max;

                                textBox2.Text = ip;
                            }
                        }
                    }

                    for (int i = IpCounting.Count - 1; i >= 0; i--)
                    {
                        bool found = false;
                        foreach (TcpRow tcp in listBox1.Items)
                        {
                            if (IpCounting[i].Ip == tcp.RemoteEndPoint.ToString().Substring(0, tcp.RemoteEndPoint.ToString().IndexOf(":")))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            IpCounting.RemoveAt(i);
                        }
                    }

                    listBox2.Items.Clear();
                    foreach (IPcounter ip in IpCounting)
                    {
                        listBox2.Items.Add(ip);
                    }

                    if (listBox1.Items.Count < 1)
                    {
                        ip = String.Empty;
                    }
                }
            }
            catch (Exception er)
            {
                textBox2.Text = er.ToString();
            }
        }
        List<IPcounter> IpCounting = new List<IPcounter>();
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                return;
            }
            if (listBox1.Items.Count >= 1)
            {
                foreach (TcpRow tcp in listBox1.Items)
                {
                    string temp = tcp.RemoteEndPoint.ToString().Substring(
                                    0, tcp.RemoteEndPoint.ToString().IndexOf(":"));
                    bool found = false;
                    if (IpCounting.Count >= 1)
                    {
                        foreach (IPcounter ip in IpCounting)
                        {
                            if (ip.Ip == temp)
                            {
                                ip.Count++;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        if (tcp.State == TcpState.Established)
                            IpCounting.Add(new IPcounter(temp));
                    }
                }
            }
        }
    }
    public class IPcounter
    {
        public string Ip;
        public int Count = 1;

        public IPcounter(string Ip)
        {
            this.Ip = Ip;
        }

        public override string ToString()
        {
            return "IP:" + Ip + " Count:" + Count;
        }
    }
}
