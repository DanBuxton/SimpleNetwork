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

        private UpdateChatWindowDelegate updateChatWindowDelegate;

        private readonly SimpleClient client;

        public ClientForm(SimpleClient client)
        {
            InitializeComponent();

            updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);

            this.client = client;

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
