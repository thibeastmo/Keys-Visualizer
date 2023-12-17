using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Keys_Visualizer
{
    public class InputsCombo
    {
        public static List<Keys> pushingKeys = new List<Keys>();
        public static bool keepPushing;

        public InputsCombo()
        {

        }

        public void runInput()
        {
            Process[] processes = Process.GetProcessesByName("brawlhalla");
            Process game1 = processes[0];

            IntPtr p = game1.MainWindowHandle;

            SetForegroundWindow(p);
            for (int i = 0; i < 10; i++)
            {
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
            }
            //ThreadStart ts = new ThreadStart(pushKeys);
            //Thread thread = new Thread(ts);
            //thread.Start();
            //keepPushing = true;

            ////keys
            //for (int i = 0; i < 10; i++)
            //{
            //    //pushingKeys.Add(Keys.NumPad4);
            //    Thread.Sleep(4000);
            //}

            //keepPushing = false;
            //thread.Abort();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void pushKeys()
        {
            while (keepPushing)
            {
                SystemSounds.Exclamation.Play();
                SendKeys.Send("{ENTER}");
                Thread.Sleep(800);
            }
        }
    }
}
