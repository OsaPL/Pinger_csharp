using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace pinger_csharp
{
    class dynamicthreads
    {
        private void createControls(int id)
        {
            Label newlbl = new Label();

            newlbl.BackColor = Label1.Backcolor;
            newlbl.Text = "Add";
            newlbl.Location = new System.Drawing.Point(90, 25);
            newlbl.Size = new System.Drawing.Size(50, 25);
            newlbl.Name = "Label" + id.ToString();
            this.Controls.Add(newlbl);

            PictureBox newpb = new PictureBox();

            newpb.BackColor = Label1.Backcolor;
            newpb.Text = "Add";
            newpb.Location = new System.Drawing.Point(90, 25);
            newpb.Size = new System.Drawing.Size(50, 25);
            newpb.Name = "PictureBox" + id.ToString();
            this.Controls.Add(newpb);
        }
        private void createThreads(int amount)
        {
            Thread th;
            for (int i = 1; i < amount; i++)
            {
                th = new Thread(pingthread(i));
                th.Start();
            }
        }
        private void pingthread(int id)
        {
            try
            {
                string lbl = "Label" + id.ToString(); //label
                string pl = "PingList" + id.ToString(); //pinglist
                this.Controls.Find(lbl);
                Ping pingClass = new Ping();
                PingReply pingReply = pingClass.Send(validatedaddress1.ToString());
                long ping = pingReply.RoundtripTime;
                if (ping == 0)
                {
                    label1.Text = "Timeout!";
                    label1.ForeColor = Color.White;
                }
                else {
                    label1.Text = "(1)" + ping + "ms";

                    label1.ForeColor = pingColor(ping);
                }
                if (graphActivated == true)
                {
                    if (ping == 0)
                        ping = 1;
                    if (graphPings1.Count > graphLimit - 1)
                    {
                        graphPings1.Insert(graphLimit, (int)ping);
                        graphPings1.RemoveAt(0);
                    }
                    else {
                        int temp = (int)ping;
                        graphPings1.Add(temp);
                    }
                    //change the graphping1 array from {first, ... , last2nd, last1st} to {..., last2nd,last1st, NEWping }
                    //PING1 and PING2 Should use DIFFERENT TABLES!!! otherwise access violation can be caused!  
                }
            }
            catch (Exception e)
            {
                label1.Text = "Unreachable!";
                label1.ForeColor = Color.White;
            }
        }
    }
    }
