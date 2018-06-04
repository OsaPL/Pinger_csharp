// ===============================================
// The Method That Parses The NetStat Output
// And Returns A List Of Port Objects
// ===============================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections;
using System.ComponentModel;

namespace pinger_csharp
{
    #region Managed IP Helper API

    public class TcpTable : IEnumerable<TcpRow>
    {
        #region Private Fields

        private IEnumerable<TcpRow> tcpRows;

        #endregion

        #region Constructors

        public TcpTable(IEnumerable<TcpRow> tcpRows)
        {
            this.tcpRows = tcpRows;
        }

        #endregion

        #region Public Properties

        public IEnumerable<TcpRow> Rows
        {
            get { return this.tcpRows; }
        }

        #endregion

        #region IEnumerable<TcpRow> Members

        public IEnumerator<TcpRow> GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion
    }
    public class UdpTable : IEnumerable<UdpRow>
    {
        #region Private Fields

        private IEnumerable<UdpRow> udpRows;

        #endregion

        #region Constructors

        public UdpTable(IEnumerable<UdpRow> udpRows)
        {
            this.udpRows = udpRows;
        }

        #endregion

        #region Public Properties

        public IEnumerable<UdpRow> Rows
        {
            get { return this.udpRows; }
        }

        #endregion

        #region IEnumerable<UdpRow> Members

        public IEnumerator<UdpRow> GetEnumerator()
        {
            return this.udpRows.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.udpRows.GetEnumerator();
        }

        #endregion
    }
    public class TcpRow
    {
        #region Private Fields

        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private TcpState state;
        private int processId;

        #endregion

        #region Constructors

        public TcpRow(IpHelper.TcpRow tcpRow)
        {
            this.state = tcpRow.state;
            this.processId = tcpRow.owningPid;

            int localPort = (tcpRow.localPort1 << 8) + (tcpRow.localPort2) + (tcpRow.localPort3 << 24) + (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            this.localEndPoint = new IPEndPoint(localAddress, localPort);

            int remotePort = (tcpRow.remotePort1 << 8) + (tcpRow.remotePort2) + (tcpRow.remotePort3 << 24) + (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            this.remoteEndPoint = new IPEndPoint(remoteAddress, remotePort);
        }

        #endregion

        #region Public Properties

        public IPEndPoint LocalEndPoint
        {
            get { return this.localEndPoint; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return this.remoteEndPoint; }
        }

        public TcpState State
        {
            get { return this.state; }
        }

        public int ProcessId
        {
            get { return this.processId; }
        }

        #endregion
    }

    public class UdpRow
    {
        #region Private Fields

        private IPEndPoint localEndPoint;
        private int processId;

        #endregion

        #region Constructors

        public UdpRow(IpHelper.UdpRow udpRow)
        {
            this.processId = udpRow.owningPid;

            int localPort = (udpRow.localPort1 << 8) + (udpRow.localPort2) + (udpRow.localPort3 << 24) + (udpRow.localPort4 << 16);
            long localAddress = udpRow.localAddr;
            this.localEndPoint = new IPEndPoint(localAddress, localPort);
        }

        #endregion

        #region Public Properties

        public IPEndPoint LocalEndPoint
        {
            get { return this.localEndPoint; }
        }

        public int ProcessId
        {
            get { return this.processId; }
        }

        #endregion
    }

    public static class ManagedIpHelper
    {
        #region Public Methods

        public static TcpTable GetExtendedTcpTable(bool sorted)
        {
            List<TcpRow> tcpRows = new List<TcpRow>();

            IntPtr tcpTable = IntPtr.Zero;
            int tcpTableLength = 0;

            if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, sorted, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                    if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, true, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        IpHelper.TcpTable table = (IpHelper.TcpTable)Marshal.PtrToStructure(tcpTable, typeof(IpHelper.TcpTable));

                        IntPtr rowPtr = (IntPtr)((long)tcpTable + Marshal.SizeOf(table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            tcpRows.Add(new TcpRow((IpHelper.TcpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.TcpRow))));
                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.TcpRow)));
                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(tcpTable);
                    }
                }
            }

            return new TcpTable(tcpRows);
        }

        public static UdpTable GetExtendedUdpTable(bool sorted)
        {
            List<UdpRow> udpRows = new List<UdpRow>();

            IntPtr udpTable = IntPtr.Zero;
            int udpTableLength = 0;

            if (IpHelper.GetExtendedUdpTable(udpTable, ref udpTableLength, sorted, IpHelper.AfInet, IpHelper.UdpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    udpTable = Marshal.AllocHGlobal(udpTableLength);
                    if (IpHelper.GetExtendedUdpTable(udpTable, ref udpTableLength, true, IpHelper.AfInet, IpHelper.UdpTableType.OwnerPidAll, 0) == 0)
                    {
                        IpHelper.UdpTable table = (IpHelper.UdpTable)Marshal.PtrToStructure(udpTable, typeof(IpHelper.UdpTable));

                        IntPtr rowPtr = (IntPtr)((long)udpTable + Marshal.SizeOf(table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            udpRows.Add(new UdpRow((IpHelper.UdpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.UdpRow))));
                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.UdpRow)));
                        }
                    }
                }
                finally
                {
                    if (udpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(udpTable);
                    }
                }
            }

            return new UdpTable(udpRows);
        }

        #endregion
    }

    #endregion

    #region P/Invoke IP Helper API

    /// <summary>
    /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366073.aspx"/>
    /// </summary>
    public static class IpHelper
    {
        #region Public Fields

        public const string DllName = "iphlpapi.dll";
        public const int AfInet = 2;

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa365928.aspx"/>
        /// </summary>
        [DllImport(IpHelper.DllName, SetLastError = true)]
        public static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);
        [DllImport(IpHelper.DllName, SetLastError = true)]
        public static extern uint GetExtendedUdpTable(IntPtr udpTable, ref int udpTableLength, bool sort, int ipVersion, UdpTableType udpTableType, int reserved);

        #endregion

        #region Public Enums

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366386.aspx"/>
        /// </summary>
        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }
        public enum UdpTableType
        {
            BasicAll,
            OwnerPidAll,
            OwnerModuleAll,
        }

        #endregion

        #region Public Structs

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366921.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpTable
        {
            public uint length;
            public TcpRow row;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UdpTable
        {
            public uint length;
            public UdpRow row;
        }

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366913.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpRow
        {
            public TcpState state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UdpRow
        {
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public int owningPid;
        }

        #endregion
    }

    #endregion
    public class NetStatPorts
    {
        public static List<Port> GetNetStatPorts()
        {
            List<Port> Ports = new List<Port>();

            try
            {
                foreach (TcpRow tcpRow in ManagedIpHelper.GetExtendedTcpTable(true))
                {
                    Ports.Add(new Port(
                        ExtractPort(tcpRow.LocalEndPoint.ToString()),
                        tcpRow.ProcessId.ToString(),
                        "TCP"));
                }

                foreach (UdpRow udpRow in ManagedIpHelper.GetExtendedUdpTable(true))
                {
                    Ports.Add(new Port(
                        ExtractPort(udpRow.LocalEndPoint.ToString()),
                        udpRow.ProcessId.ToString(),
                        "UDP"));
                }
            }
            catch (Exception ex)
            {
                Ports = new List<Port>();
                Ports.Add(new Port("0","0","NA"));
                return Ports;
            }
            return Ports;
        }
        public static string ExtractPort(string ip)
        {
            return ip.Substring(ip.IndexOf(@":") + 1);
        }

        public static string LookupProcess(short v)
        {
            return Process.GetProcessById(v).ProcessName;
        }
    }

    // ===============================================
    // The Port Class We're Going To Create A List Of
    // ===============================================
    public class Port
    {
        public Port(string port_number, string process_pid, string protocol)
        {
            this.port_number = port_number;
            this.process_pid = process_pid;
            this.protocol = protocol;
            this.process_name = "N/A";
        }
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