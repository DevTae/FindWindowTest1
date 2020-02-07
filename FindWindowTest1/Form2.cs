using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindWindowTest1
{
    public partial class Form2 : Form
    {
        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private uint VK_LBUTTON = 0x01;
        private uint VK_CONTROL = 0x11;
        private uint VK_A = 0x41;
        private uint VK_C = 0x43;
        private uint VK_V = 0x56;
        private uint WM_KEYDOWN = 0x0100;
        private uint WM_KEYUP = 0x0101;
        private uint WM_COPY = 0x0301;
        private uint WM_PASTE = 0x0302;
        private uint WM_GETTEXTLENGTH = 0x000e;
        private uint WM_GETTEXT = 0x000d;
        private uint WM_SETTEXT = 0x000c;


        private void loadMessage_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = Form1.hChatRoomPtr;
            Int32 textLength = (Int32) SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            //MessageBox.Show(textLength.ToString()); // 디버깅용

            IntPtr textPtr = Marshal.AllocHGlobal(10000); // 약 500글자 로드
            SendMessage(hWnd, WM_GETTEXT, new IntPtr(10000), textPtr);

            IntPtr hChatLogViewer = FindWindow(null, "Chat Log Viewer");
            IntPtr hTextBox = FindWindowEx(hChatLogViewer, IntPtr.Zero, "WindowsForms10.EDIT.app.0.141b42a_r9_ad1", null);
            if (hTextBox == IntPtr.Zero)
                Application.Exit();

            SendMessage(hTextBox, WM_SETTEXT, IntPtr.Zero, textPtr);
            Marshal.FreeHGlobal(textPtr);
        }
    }
}
