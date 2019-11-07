using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClient
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string message);

        private readonly UpdateChatWindowDelegate updateChatWindowDelegate;

        private readonly SimpleClient client;

        public ClientForm(SimpleClient c)
        {
            InitializeComponent();

            updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);

            client = c;

            txtInputMessage.Select();

            txtMessageDisplay.ReadOnly = true;

            Load += (s, e) => client.Run();
            FormClosed += (s, e) => client.Stop();

            btnSubmit.Click += (s, e) =>
            {
                string message = txtInputMessage.Text;
                if (message.ToLower() != "exit")
                {
                    client.SendMessage(message);
                    txtInputMessage.Clear();
                }
                else client.Stop();
            };
        }

        public void UpdateChatWindow(string message)
        {
            if (txtMessageDisplay.InvokeRequired)
            {
                //btnSubmit?.Invoke(updateChatWindowDelegate, message);
                txtMessageDisplay.Invoke(updateChatWindowDelegate, message);
            }
            else
            {
                txtMessageDisplay.Text += message;
                txtMessageDisplay.SelectionStart = txtMessageDisplay.Text.Length;
                txtMessageDisplay.ScrollToCaret();
            }
        }
    }
}
