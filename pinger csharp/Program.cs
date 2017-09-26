using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace pinger_csharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles(); //to make the bar colorable
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OverlayForm());
        }
    }
}
