using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProjectUGC
{
    public partial class login : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        public login()
        {
            InitializeComponent();
            m_aeroEnabled = false;

        }
        private void login_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.rememberme)
            {
                username.Text = Properties.Settings.Default.username;
                password.Text = Properties.Settings.Default.password;
                checkBox1.Checked = true;
            }
        }
        int mouseX, mouseY;
        bool mouseDown;
        int mouseinX, mouseinY;
        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseinX = MousePosition.X - Bounds.X;
            mouseinY = MousePosition.Y - Bounds.Y;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Properties.Settings.Default.username = username.Text;
                Properties.Settings.Default.password = password.Text;
            }
            else
            {
                Properties.Settings.Default.username = null;
                Properties.Settings.Default.password = null;
            }
            Properties.Settings.Default.rememberme = checkBox1.Checked;
            Properties.Settings.Default.Save();

            if (API.Login(username.Text, password.Text))
            {
                Form form2 = new test();
                form2.Show();
                this.Hide();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Your Login Credentials Are Invalid, Check Your Credentials Again. For Any Help Contact Owners" + Environment.NewLine + "Discord Invite: https://discord.gg/4B9B75C" + Environment.NewLine + "Do You Want To Copy Invite Link?", "Project UGC", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    Clipboard.SetText("https://discord.gg/4B9B75C");
                    return;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2Transition1.Show(reg);
        }

        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseinX = MousePosition.X - Bounds.X;
            mouseinY = MousePosition.Y - Bounds.Y;
        }

        private void label6_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseX = MousePosition.X - mouseinX;
                mouseY = MousePosition.Y - mouseinY;

                SetDesktopLocation(mouseX, mouseY);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Transition1.Hide(reg);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (API.Register(guna2TextBox2.Text, guna2TextBox1.Text, guna2TextBox3.Text, guna2TextBox4.Text))
            {
                MessageBox.Show("Account Created, Thanks For Using UGC Tool", "Project UGC", MessageBoxButtons.OK, MessageBoxIcon.Information);

                WebClient wc = new WebClient { };
                string myip = wc.DownloadString($"https://api.ipify.org/");
                //sendWebHook("https://discordapp.com/api/webhooks/751907875781083216/LP4OT1Bcudf7pUT-cfKCNj6S_lWqmWY0nLTvPMkN8Cld-XUL3Wtadld-ZkL8-KoSQ_uJ ", string.Concat(new string[] { ">>> New Register" + Environment.NewLine + "User: " + username.Text + Environment.NewLine + "Email: " + email.Text + Environment.NewLine + "Password: " + password.Text + Environment.NewLine + "License Used: " + license.Text + "IP: " + myip }), "REGISTER BOT");
            }
            else
            {
                MessageBox.Show("Someting Went Wrong, Check Your Register Credentials", "Project UGC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WebClient wc = new WebClient { };
                string myip = wc.DownloadString($"https://api.ipify.org/");
                //sendWebHook("https://discordapp.com/api/webhooks/751907875781083216/LP4OT1Bcudf7pUT-cfKCNj6S_lWqmWY0nLTvPMkN8Cld-XUL3Wtadld-ZkL8-KoSQ_uJ ", string.Concat(new string[] { ">>> Faild Register" + Environment.NewLine + "User: " + username.Text + Environment.NewLine + "Email: " + email.Text + Environment.NewLine + "Password: " + password.Text + Environment.NewLine + "License Used: " + license.Text + "IP: " + myip }), "REGISTER  FAILD BOT");
                return;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void reg_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseX = MousePosition.X - mouseinX;
                mouseY = MousePosition.Y - mouseinY;

                SetDesktopLocation(mouseX, mouseY);
            }
        }
    }
}
