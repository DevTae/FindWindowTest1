using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindWindowTest1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public Form1()
        {
            InitializeComponent();
        }

        // How about implementing unlock function to make process comfortable.
        /*
        private string className = "EVA_Window_Dblclk";
        private string captionName = "카카오톡";
        private string hChildDialogClass = "EVA_Window";
        private string hChildDialogCaption = null;
        private string hChatRoomListClass = "_EVA_CustomScrollCtrl";
        private string hChatRoomListCaption = null;
        */
        private string className = "Notepad";
        private string captionName = "제목 없음 - 메모장";
        private string hChildDialogClass = "Edit";
        private string hChildDialogCaption = "";
        private string hChatRoomListClass = "EVA_Window";
        private string hChatRoomListCaption = "ChatRoomListView_0x001004a8";

        public static IntPtr hChatRoomPtr;
        Form2 newForm = new Form2();
        private void loadKakao_Click(object sender, EventArgs e)
        {
            IntPtr kakaoPtr = FindWindow(className, captionName);
            if(kakaoPtr == IntPtr.Zero)
            {
                MessageBox.Show(null, "Wasn't able to find window class of KakaoTalk.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsValidKakaotalkWindow(kakaoPtr))
            {
                MessageBox.Show(null, "Finded Successly.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show(null, "Was able to find window class of KakaoTalk.\nBut external reason occured. As like a updated KakaoTalk before.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            newForm.Show();
            this.Hide();
        }

        public bool IsValidKakaotalkWindow(IntPtr hWnd)
        {
            // Check if hWnd is Window Handle or not.
            if (!IsWindow(hWnd)) return false;

            //StringBuilder titleName = new StringBuilder(50);
            //int name = GetWindowText(hWnd, titleName, 50);
            //if (name == 0) return false;
            //test(hWnd);
            IntPtr hChildDialog1 = FindWindowEx(hWnd, IntPtr.Zero, hChildDialogClass, hChildDialogCaption);
            if (hChildDialog1 == IntPtr.Zero) return false;
            //test(hChildDialog1);
            IntPtr hChildDialog2 = FindWindowEx(hChildDialog1, IntPtr.Zero, hChatRoomListClass, hChatRoomListCaption);
            //if (hChildDialog2 == IntPtr.Zero) return false;
            //test(hChildDialog2);
            Form1.hChatRoomPtr = hChildDialog1;

            return true;
        }



        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

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

        public void test(IntPtr hWnd)
        {
            Int32 textLength = (Int32)SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            MessageBox.Show(textLength.ToString()); // 디버깅용

            IntPtr textPtr = Marshal.AllocHGlobal(10000); // 약 500글자 로드
            SendMessage(hWnd, WM_GETTEXT, new IntPtr(10000), textPtr);

            //IntPtr hChatLogViewer = FindWindow(null, "Chat Log Viewer");
            //IntPtr hTextBox = FindWindowEx(hChatLogViewer, IntPtr.Zero, "WindowsForms10.EDIT.app.0.141b42a_r9_ad1", null);
            //if (hTextBox == IntPtr.Zero)
            //    Application.Exit();

            //SendMessage(hTextBox, WM_SETTEXT, IntPtr.Zero, textPtr);
            Marshal.FreeHGlobal(textPtr);
        }
    }
}
