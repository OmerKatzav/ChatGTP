using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatGTPClient
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (addressBox.Text == "" || portBox.Text == "")
            {
                addressBox.Text = "127.0.0.1";
                portBox.Text = "5000";
            }
            if (IPAddress.TryParse(addressBox.Text, out IPAddress address) && int.TryParse(portBox.Text, out int port) && port > 0 && port < 65536 && nickBox.Text != "" && CheckNickname(nickBox.Text))
                DialogResult = DialogResult.OK;
            Close();
        }

        private bool CheckNickname(string nickname)
        {
            foreach (char c in nickname)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }
            return true;
        }
    }
}
