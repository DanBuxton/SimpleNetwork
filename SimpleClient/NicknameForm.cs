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
    public partial class NicknameForm : Form
    {
        public new string Name { get; private set; } = string.Empty;

        public NicknameForm()
        {
            InitializeComponent();

            btnSubmit.Click += (s, e) =>
            {
                Name = txtNickname.Text;
                DialogResult = DialogResult.OK;
            };
        }
    }
}
