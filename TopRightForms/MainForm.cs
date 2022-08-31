using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using topright;
using TopRightForms.Properties;


namespace TopRightForms
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //[DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
        bool TopTaskbar=false;
        bool MiniTaskbar;
        public Form1()
        {
            InitializeComponent();
            InitializeTray();

            IntPtr hwnd;
            Language.LanguageSelect();
            new Thread(o =>
            {
                while (true)
                {
                    hwnd = FindWindow("Windows.UI.Core.CoreWindow", Language.LastWindowName);
                    //Hides the notification
                    //ShowWindow(hwnd, 0);

                    TopRight(hwnd);

                    Thread.Sleep(10);
                }
            }).Start();
        }
        void TopRight(IntPtr hwnd)
        {
            if (!TopTaskbar)
                MoveNotifyWindow(hwnd, -50); //Topright
            else if (TopTaskbar)
            {
                if (MiniTaskbar)
                    MoveNotifyWindow(hwnd, -28); //TopRight_TopMiniTaskbar (есть проблема в том что когда выходит уведомлением нельзя пользоваться верхним Taskbar а точнее его треем и временем)
                else
                    MoveNotifyWindow(hwnd, -18); //TopRight_TopTaskbar (такая я же проблема и тут)
                //Позже сделаю фикс
            }
        }
        void MoveNotifyWindow(IntPtr hwnd, int y)
        {
            Rectangle NotifyRect = new Rectangle();
            GetWindowRect(hwnd, ref NotifyRect);
            NotifyRect.Width = NotifyRect.Width - NotifyRect.X;
            NotifyRect.Height = NotifyRect.Height - NotifyRect.Y;
            SetWindowPos(hwnd, 0, Screen.PrimaryScreen.Bounds.Width - NotifyRect.Width, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
        }
        private void InitializeTray()
        {
            notifyIcon1.Icon = Resources.notify;
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Topright";
            notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("TopRight", null, ContextMenu_TopRight);
            notifyIcon1.ContextMenuStrip.Items.Add("TopRight_TopTaskbar", null, ContextMenu_TopRight_TopTaskbar);
            notifyIcon1.ContextMenuStrip.Items.Add("TopRight_TopMiniTaskbar", null, ContextMenu_TopRight_TopMiniTaskbar);
            notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, Exit);
        }
        private void Exit(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Process.Start(new ProcessStartInfo
             {
                 FileName = @"C:\Windows\System32\taskkill.exe",
                 Arguments = $"/PID {Process.GetCurrentProcess().Id} /F",
                 WindowStyle = ProcessWindowStyle.Hidden,
                 CreateNoWindow = true
             });
        }
        private void ContextMenu_TopRight(object sender, EventArgs e){ TopTaskbar = false; MiniTaskbar = false; }
        private void ContextMenu_TopRight_TopTaskbar(object sender, EventArgs e) { TopTaskbar = true; MiniTaskbar = false; }
        private void ContextMenu_TopRight_TopMiniTaskbar(object sender, EventArgs e) { TopTaskbar = true; MiniTaskbar = true; }
    }
}
