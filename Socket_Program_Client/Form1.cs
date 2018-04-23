using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//----------------------------------
using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace Socket_Program_Client
{
    public partial class FrmClient : Form
    {
        public FrmClient()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }


        private Socket SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public DateTime TimePm
        {
            get
            {

                return DateTime.UtcNow;

            }
        }
        public void Connect()
        {
            try
            {
                IPEndPoint ipendpointServer = new IPEndPoint(IPAddress.Parse(Txt_IP.Text), int.Parse(Txt_Port.Text));

                SocketClient.Connect(ipendpointServer);

                Thread tr = new Thread(new ThreadStart(ReciveMessage));

                tr.Start();

                Txt_Status.Text = "Connected";
            }
            catch (Exception ex)
            {
                Txt_Status.Text = ex.Message;
            }
        }

        public void ReciveMessage()
        {
            try
            {
                while (true)
                {
                    byte[] Buffer = new byte[1024];

                    int ReciveData = SocketClient.Receive(Buffer);

                    if (ReciveData > 0)
                    {
                        string message = Encoding.Unicode.GetString(Buffer, 0, ReciveData);

                        listBox1.Items.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Txt_Status.Text = ex.Message;
            }
        }

        public void SendPM()
        {

            byte[] Buffer = new byte[1024];

            Buffer = Encoding.Unicode.GetBytes(string.Format("{0} {1}: {2}", Txt_Username.Text, TimePm, Txt_Message.Text));

            richTextBox1.Text += string.Format("{0} {1}: {2}", Txt_Username.Text, TimePm, Txt_Message.Text) + Environment.NewLine;

            SocketClient.Send(Buffer);

            Txt_Message.Text = string.Empty;
        }

        public void Disconnect()
        {
            try
            {
                if (SocketClient != null)
                {
                    SocketClient.Shutdown(SocketShutdown.Both);
                }
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception ex)
            {
                Txt_Status.Text = (ex.Message);
            }
        }

        public void Exit()
        {
            try
            {
                SocketClient.Shutdown(SocketShutdown.Both);

                Environment.Exit(Environment.ExitCode);

                Application.Exit();

            }
            catch (Exception ex)
            {
                Txt_Status.Text = (ex.Message);
            }
        }

        private void Btn_Send_Click(object sender, EventArgs e)
        {
            if (Txt_Message.Text != string.Empty)
            {
                SendPM();
            }

        }
        private void Btn_Connect_Click(object sender, EventArgs e)
        {
            Connect();
        }
        private void Btn_DisConnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        #region RightClick
        private void RC_Cut_Click(object sender, EventArgs e)
        {
            Txt_Message.Cut();
        }

        private void RC_Copy_Click(object sender, EventArgs e)
        {
            Txt_Message.Copy();
        }

        private void RC_Paste_Click(object sender, EventArgs e)
        {
            Txt_Message.Paste();
        }

        private void RC_Clear_Click(object sender, EventArgs e)
        {
            Txt_Message.Clear();
        }

        private void RC_SelectAll_Click(object sender, EventArgs e)
        {
            Txt_Message.SelectAll();
        }
        #endregion

    }
}
