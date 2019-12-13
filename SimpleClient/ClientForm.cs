using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClient
{
    public partial class ClientForm : Form
    {
        public List<string> ClientNames { get; set; } = new List<string>();

        delegate void UpdateChatWindowDelegate(string message);
        delegate void UpdateClientListDelegate(List<string> names);

        private UpdateChatWindowDelegate updateChatWindowDelegate;
        private UpdateClientListDelegate updateClientListDelegate;

        private readonly SimpleClient client;

        private readonly NicknameForm nicknameForm = new NicknameForm();

        public ClientForm(SimpleClient c)
        {
            InitializeComponent();

            client = c;

            txtInputMessage.Select();

            txtMessageDisplay.ReadOnly = true;

            btnDisconnect.Enabled = false;
            btnMessagePerson.Enabled = false;
            btnNickname.Enabled = false;
            btnRefreshList.Enabled = false;

            btnUploadImage.Enabled = false;
            btnReset.Enabled = false;

            FormClosed += (s, e) =>
            {
                client.TCPStop();
            };
            // Connect to server
            btnConnect.Click += (s, e) =>
            {
                if (client.TCPConnect("127.0.0.1", 4444))
                {
                    client.Run();
                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                    updateClientListDelegate = new UpdateClientListDelegate(UpdateClientList);

                    Visible = false;
                    if (nicknameForm.ShowDialog() == DialogResult.OK)
                    {
                        client.TCPSendNickname(nicknameForm.Name);
                        btnNickname.Enabled = !btnNickname.Enabled;
                    }
                    Visible = true;

                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    btnMessagePerson.Enabled = true;
                    btnRefreshList.Enabled = true;

                    btnUploadImage.Enabled = true;
                    btnReset.Enabled = true;
                }
            };
            btnDisconnect.Click += (s, e) =>
            {
                client.TCPStop();

                cbClients.Items.Clear();

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnMessagePerson.Enabled = false;
                btnNickname.Enabled = false;
                btnRefreshList.Enabled = false;

                btnUploadImage.Enabled = false;
                btnReset.Enabled = false;
            };

            btnSubmit.Click += (s, e) =>
            {
                string message = txtInputMessage.Text;
                if (string.IsNullOrWhiteSpace(message)) return; // No message
                else if (message.Contains('@')) // Direct message
                {
                    int space = message.IndexOf(' ');
                    //client.SendDirectMessage(message.Remove(0, 1).Remove(space, message.Length - 1), message);
                    var name = cbClients.SelectedItem as string;
                    string msg = message.Remove(0, ++space);
                    client.TCPSendDirectMessage(name, msg);

                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else if (message.ToLower() != "exit") // Normal message
                {
                    client.TCPSendMessage(message);
                    txtInputMessage.Clear();

                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else client.TCPStop(); // Exit
            };

            //btnNickname.Click += (s, e) =>
            //{
            //    Visible = false;
            //    if (nicknameForm.ShowDialog() == DialogResult.OK)
            //    {
            //        client.TCPSendNickname(nicknameForm.Name);
            //        btnNickname.Enabled = false;
            //    }
            //    Visible = true;
            //};
            btnRefreshList.Click += (s, e) =>
            {
                //picImage.Dispose();
            };

            btnMessagePerson.Click += (s, e) =>
            {
                if (cbClients.SelectedItem != null)
                {
                    txtInputMessage.Text = $"@{cbClients.SelectedItem as string} ";
                }
            };

            btnUploadImage.Click += (s, e) =>
            {
                Thread t = new Thread(new ThreadStart(()=>
                {
                    if (oFD.ShowDialog(this) == DialogResult.OK)
                    {
                        using (System.IO.Stream file = oFD.OpenFile())
                        {
                            byte[] imageBinary = new byte[file.Length];
                            file.Read(imageBinary, 0, (int)file.Length);

                            picImage.Image = new Bitmap(file);

                            file.Close();
                        }
                    }
                }));
                t.Start();
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

        public void UpdateClientList(List<string> names)
        {
            if (cbClients.InvokeRequired)
            {
                cbClients.Invoke(updateClientListDelegate, names);
            }
            else
            {
                cbClients.Items.Clear();

                foreach (var c in names)
                {
                    if (!cbClients.Items.Contains(c))
                    {
                        cbClients.Items.Add(c);
                    }
                }
            }
        }
    }
}
