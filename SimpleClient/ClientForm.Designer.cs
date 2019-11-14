namespace SimpleClient
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMessageDisplay = new System.Windows.Forms.RichTextBox();
            this.txtInputMessage = new System.Windows.Forms.RichTextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnNickname = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.cbClients = new System.Windows.Forms.ComboBox();
            this.btnMessagePerson = new System.Windows.Forms.Button();
            this.btnRefreshList = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessageDisplay
            // 
            this.txtMessageDisplay.Location = new System.Drawing.Point(12, 76);
            this.txtMessageDisplay.Name = "txtMessageDisplay";
            this.txtMessageDisplay.Size = new System.Drawing.Size(388, 231);
            this.txtMessageDisplay.TabIndex = 0;
            this.txtMessageDisplay.Text = "";
            // 
            // txtInputMessage
            // 
            this.txtInputMessage.Location = new System.Drawing.Point(12, 313);
            this.txtInputMessage.Name = "txtInputMessage";
            this.txtInputMessage.Size = new System.Drawing.Size(287, 31);
            this.txtInputMessage.TabIndex = 1;
            this.txtInputMessage.Text = "";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(306, 313);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(94, 31);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // btnNickname
            // 
            this.btnNickname.Location = new System.Drawing.Point(324, 47);
            this.btnNickname.Name = "btnNickname";
            this.btnNickname.Size = new System.Drawing.Size(75, 23);
            this.btnNickname.TabIndex = 3;
            this.btnNickname.Text = "Nickname";
            this.btnNickname.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 47);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(93, 47);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 5;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // cbClients
            // 
            this.cbClients.FormattingEnabled = true;
            this.cbClients.Location = new System.Drawing.Point(12, 12);
            this.cbClients.Name = "cbClients";
            this.cbClients.Size = new System.Drawing.Size(111, 21);
            this.cbClients.TabIndex = 6;
            // 
            // btnMessagePerson
            // 
            this.btnMessagePerson.Location = new System.Drawing.Point(129, 10);
            this.btnMessagePerson.Name = "btnMessagePerson";
            this.btnMessagePerson.Size = new System.Drawing.Size(75, 23);
            this.btnMessagePerson.TabIndex = 7;
            this.btnMessagePerson.Text = "Message";
            this.btnMessagePerson.UseVisualStyleBackColor = true;
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.Location = new System.Drawing.Point(210, 10);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshList.TabIndex = 8;
            this.btnRefreshList.Text = "Refresh List";
            this.btnRefreshList.UseVisualStyleBackColor = true;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(439, 346);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 23);
            this.btnDraw.TabIndex = 9;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(520, 346);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 381);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.btnRefreshList);
            this.Controls.Add(this.btnMessagePerson);
            this.Controls.Add(this.cbClients);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnNickname);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtInputMessage);
            this.Controls.Add(this.txtMessageDisplay);
            this.Name = "ClientForm";
            this.Text = "SimpleClient";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtMessageDisplay;
        private System.Windows.Forms.RichTextBox txtInputMessage;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnNickname;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.ComboBox cbClients;
        private System.Windows.Forms.Button btnMessagePerson;
        private System.Windows.Forms.Button btnRefreshList;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button btnReset;
    }
}