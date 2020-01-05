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
        delegate void UpdateWindowDelegate(bool error);
        delegate void UpdateImageDelegate(Bitmap img);
        delegate void UpdateImageLocationDelegate(int x, int y);

        private UpdateChatWindowDelegate updateChatWindowDelegate;
        private UpdateClientListDelegate updateClientListDelegate;
        private UpdateWindowDelegate updateWindowDelegate;
        private UpdateImageDelegate updateImageDelegate;
        private UpdateImageLocationDelegate updateImageLocationDelegate;

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

            btnSubmit.Enabled = false;
            btnUploadImage.Enabled = false;
            btnReset.Enabled = false;

            updateWindowDelegate = new UpdateWindowDelegate(UpdateWindow);

            FormClosed += (s, e) =>
            {
                client.Stop();
            };
            // Connect to server
            btnConnect.Click += (s, e) =>
            {
                if (client.Connect("127.0.0.1", 25565))
                {
                    client.Run();
                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                    updateClientListDelegate = new UpdateClientListDelegate(UpdateClientList);
                    updateImageDelegate = new UpdateImageDelegate(UpdateImage);
                    updateImageLocationDelegate = new UpdateImageLocationDelegate(UpdateImageLocation);

                    Visible = false;
                    if (nicknameForm.ShowDialog() == DialogResult.OK)
                    {
                        client.TCPSendNickname(nicknameForm.Name);
                    }
                    Visible = true;

                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    btnMessagePerson.Enabled = true;

                    btnSubmit.Enabled = true;
                    btnUploadImage.Enabled = true;
                    btnReset.Enabled = true;
                }
                else
                {
                    //txtMessageDisplay.Text += $"Can\'t connect";
                    UpdateChatWindow($"Can\'t Connect\n");
                }
            };
            btnDisconnect.Click += (s, e) =>
            {
                client.Stop();

                cbClients.Items.Clear();

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnMessagePerson.Enabled = false;

                btnSubmit.Enabled = false;
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
                    txtInputMessage.Clear();

                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else if (message.ToLower() != "exit") // Normal message
                {
                    client.TCPSendMessage(message);
                    txtInputMessage.Clear();

                    updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
                }
                else client.Stop(); // Exit
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
                new Thread(new ThreadStart(() =>
                {
                    var oF = new OpenFileDialog();
                    if (oF.ShowDialog(this) == DialogResult.OK)
                    {
                        using (Stream file = oF.OpenFile())
                        {
                            byte[] imageBinary = new byte[file.Length];
                            file.Read(imageBinary, 0, (int)file.Length);

                            Bitmap img = new Bitmap(file);
                            client.TCPSendImage(img, oF.FileName.Split(new char[] { '\\' }).Last());
                            file.Close();

                        }
                    }
                })).Start();
            };

            pnlImg.MouseClick += (s, e) =>
            {
                base.OnMouseClick(e);

                if (picImage.Image != null)
                    client.TCPSendImagePositionUpdate(e.X, e.Y);
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

        public void UpdateWindow(bool error)
        {
            if (!InvokeRequired)
            {
                if (error)
                {
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                    btnMessagePerson.Enabled = false;

                    btnSubmit.Enabled = false;
                    btnUploadImage.Enabled = false;
                    btnReset.Enabled = false;
                }
                else
                {
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    btnMessagePerson.Enabled = true;

                    btnSubmit.Enabled = true;
                    btnUploadImage.Enabled = true;
                    btnReset.Enabled = true;
                }
            }
            else
            {
                Invoke(updateWindowDelegate, error);
            }
        }

        public void UpdateImage(Bitmap img)
        {
            if (picImage.InvokeRequired)
            {
                picImage.Invoke(updateImageDelegate, img);
            }
            else
            {
                picImage.Image = img;
            }
        }

        public void UpdateImageLocation(int x, int y)
        {
            if (picImage.InvokeRequired)
            {
                picImage.Invoke(updateImageLocationDelegate, x, y);
            }
            else
            {
                if (picImage.Image != null)
                {
                    int newX = x; // Destination of the x coordinate
                    int newY = y; // Destination of the y coordinate

                    bool HasArrived = false; // Flag to check that if it has arrived to the destination

                    while (HasArrived == false)
                    {
                        // Execute code to add or subtract from values to get closer to destX and destY
                        if (newX > picImage.Left)
                        {
                            picImage.Left += 1;
                        }
                        else if (newX < picImage.Left)
                        {
                            picImage.Left -= 1;
                        }

                        if (newY > picImage.Top)
                        {
                            picImage.Top += 1;
                        }
                        else if (newY < picImage.Top)
                        {
                            picImage.Top -= 1;
                        }

                        // Check if the picture box has arrived if so then change flag to true to end loop
                        if (picImage.Left == newX && picImage.Top == newY)
                        {
                            HasArrived = true;
                        }
                    }
                }
            }
        }
    }
}
