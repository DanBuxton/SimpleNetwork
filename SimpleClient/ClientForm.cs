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
        delegate void _UpdateChatWindowDelegate();

        private SimpleClient client;

        public ClientForm(SimpleClient client)
        {
            this.client = client;

            InitializeComponent();

            txtInputMessage.Select();

            txtMessageDisplay.ReadOnly = true;

            this.Load += (s, e) => client.Run();
            //this.FormClosed += (s, e) => client.Stop();

            btnSubmit.Click += BtnSubmit_Click;
        }

        private void UpdateChatWindow()
        {

        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
