using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<string> ClientNames { get; set; } = new List<string>();

        delegate void UpdateChatWindowDelegate(string message);

        private UpdateChatWindowDelegate updateChatWindowDelegate;

        private readonly SimpleClient client;

        private readonly NicknameForm nicknameForm = new NicknameForm();

        public ClientForm(SimpleClient c)
        {
            InitializeComponent();

            client = c;

            txtInputMessage.Select();

            txtMessageDisplay.ReadOnly = true;

            cbClients.DataSource = ClientNames;

            btnDisconnect.Enabled = false;
            btnMessagePerson.Enabled = false;
            btnNickname.Enabled = false;
            btnRefreshList.Enabled = false;

            

            FormClosed += (s, e) =>
            {
                client.Stop();
            };
            // Connect to server
            btnConnect.Click += (s, e) =>
            {


                client.Connect("127.0.0.1", 4444);
                client.Run();
                updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);

                Visible = false;
                if (nicknameForm.ShowDialog() == DialogResult.OK)
                {
                    client.SendNickname(nicknameForm.Name);
                    btnNickname.Enabled = false;
                }
                Visible = true;

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnMessagePerson.Enabled = true;
                btnNickname.Enabled = true;
                btnRefreshList.Enabled = true;
            };
            btnDisconnect.Click += (s, e) =>
            {
                client.Stop();

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnMessagePerson.Enabled = false;
                btnNickname.Enabled = false;
                btnRefreshList.Enabled = false;

            };

            btnSubmit.Click += (s, e) =>
            {
                string message = txtInputMessage.Text;
                if (string.IsNullOrWhiteSpace(message)) return; // No message
                else if (message.Contains('@')) // Direct message
                {
                    int space = message.IndexOf(' ');
                    client.SendDirectMessage(message.Remove(0, 1).Remove(space, message.Length-1), message);
                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else if (message.ToLower() != "exit") // Normal message
                {
                    client.SendMessage(message);
                    txtInputMessage.Clear();

                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else client.Stop(); // Exit
            };

            btnNickname.Click += (s, e) =>
            {
                Visible = false;
                if (nicknameForm.ShowDialog() == DialogResult.OK)
                {
                    client.SendNickname(nicknameForm.Name);
                    btnNickname.Enabled = false;
                }
                Visible = true;
            };

            //btnRefreshList.Click += (s, e) => client.Stop();

            btnMessagePerson.Click += (s, e) =>
            {
                if (cbClients.SelectedItem != null)
                {

                }
            };

            btnRefreshList.Click += (s, e) =>
            {

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

        public void UpdateClientList(List<string> clients) => ClientNames = clients;
    }
}
