using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatGTPClient
{
    public partial class Form1 : Form
    {
        bool connected = false;
        Client client;
        public Form1()
        {
            InitializeComponent();
            sendBtn.Enabled = false;
            messageTextBox.Enabled = false;
            privMessageBtn.Enabled = false;
            promoteBtn.Enabled = false;
            muteBtn.Enabled = false;
            kickBtn.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                Input input = new Input();
                input.ShowDialog();
                if (input.DialogResult == DialogResult.OK)
                {
                    string address = input.addressBox.Text;
                    int port = int.Parse(input.portBox.Text);
                    string nickname = input.nickBox.Text;
                    client = new Client(nickname, address, port);
                    client.MessageReceived += (s, msg) =>
                    {
                        Invoke(new Action(() => AddMessage(msg)));
                    };
                    client.ExceptionReceived += (s, err) =>
                    {
                        Invoke(new Action(() => ExceptionReceived(err)));
                    };
                    client.Disconnected += (s, err) =>
                    {
                        Invoke(new Action(() => Disconnected()));
                    };
                    client.MembersReceived += (s, members) =>
                    {
                        Invoke(new Action(() => MembersRecieved(members)));
                    };
                    client.AdminReceived += (s, err) =>
                    {
                        Invoke(new Action(() => AdminRecieved()));
                    };
                    messageListBox.Items.Clear();
                    connected = true;
                    sendBtn.Enabled = true;
                    messageTextBox.Enabled = true;
                    privMessageBtn.Enabled = true;
                    btnConnect.Text = "Disconnect";
                    client.Start();
                }
                else
                {
                    MessageBox.Show("Invalid input");
                }
            }
            else
            {
                client.Stop();
                connected = false;
                sendBtn.Enabled = false;
                messageTextBox.Enabled = false;
                privMessageBtn.Enabled = false;
                promoteBtn.Enabled = false;
                muteBtn.Enabled = false;
                kickBtn.Enabled = false;
                nickBox.Items.Clear();
                btnConnect.Text = "Connect";
            }
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { Send(); }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void Send()
        {
            if (messageTextBox.Text == "") { return; }
            client.Send(1, messageTextBox.Text);
            messageTextBox.Text = "";
        }

        public void AddMessage(string message)
        {
            messageListBox.Items.Add(message);
        }

        public void ExceptionReceived(Exception err)
        {
            connected = false;
            sendBtn.Enabled = false;
            messageTextBox.Enabled = false;
            privMessageBtn.Enabled = false;
            promoteBtn.Enabled = false;
            muteBtn.Enabled = false;
            kickBtn.Enabled = false;
            nickBox.Items.Clear();
            btnConnect.Text = "Connect";
            MessageBox.Show(err.Message);
        }

        public void Disconnected()
        {
            connected = false;
            sendBtn.Enabled = false;
            messageTextBox.Enabled = false;
            privMessageBtn.Enabled = false;
            promoteBtn.Enabled = false;
            muteBtn.Enabled = false;
            kickBtn.Enabled = false;
            nickBox.Items.Clear();
            btnConnect.Text = "Connect";
            MessageBox.Show("Disconnected");
        }

        public void MembersRecieved(string[] members)
        {
            nickBox.Items.Clear();
            foreach (string member in members)
            {
                nickBox.Items.Add(member);
            }
        }

        public void AdminRecieved()
        {
            promoteBtn.Enabled = true;
            muteBtn.Enabled = true;
            kickBtn.Enabled = true;
        }

        private void privMessageBtn_Click(object sender, EventArgs e)
        {
            client.Send(5, nickBox.SelectedItem.ToString());
        }

        private void promoteBtn_Click(object sender, EventArgs e)
        {
            client.Send(2, nickBox.SelectedItem.ToString());
        }

        private void muteBtn_Click(object sender, EventArgs e)
        {
            client.Send(4, nickBox.SelectedItem.ToString());
        }

        private void kickBtn_Click(object sender, EventArgs e)
        {
            client.Send(3, nickBox.SelectedItem.ToString());
        }
    }
}
