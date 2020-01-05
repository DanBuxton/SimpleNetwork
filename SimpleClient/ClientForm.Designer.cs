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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.cbClients = new System.Windows.Forms.ComboBox();
            this.btnMessagePerson = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.btnUploadImage = new System.Windows.Forms.Button();
            this.pnlImg = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlImg.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMessageDisplay
            // 
            this.txtMessageDisplay.Location = new System.Drawing.Point(12, 76);
            this.txtMessageDisplay.Name = "txtMessageDisplay";
            this.txtMessageDisplay.Size = new System.Drawing.Size(365, 231);
            this.txtMessageDisplay.TabIndex = 0;
            this.txtMessageDisplay.Text = "";
            // 
            // txtInputMessage
            // 
            this.txtInputMessage.Location = new System.Drawing.Point(12, 313);
            this.txtInputMessage.Name = "txtInputMessage";
            this.txtInputMessage.Size = new System.Drawing.Size(265, 31);
            this.txtInputMessage.TabIndex = 1;
            this.txtInputMessage.Text = "";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(283, 313);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
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
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(586, 314);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(90, 30);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(0, 0);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(50, 50);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 11;
            this.picImage.TabStop = false;
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Location = new System.Drawing.Point(469, 314);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(100, 30);
            this.btnUploadImage.TabIndex = 12;
            this.btnUploadImage.Text = "Upload Image";
            this.btnUploadImage.UseVisualStyleBackColor = true;
            // 
            // pnlImg
            // 
            this.pnlImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImg.Controls.Add(this.picImage);
            this.pnlImg.Location = new System.Drawing.Point(384, 22);
            this.pnlImg.Name = "pnlImg";
            this.pnlImg.Size = new System.Drawing.Size(373, 285);
            this.pnlImg.TabIndex = 13;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 381);
            this.Controls.Add(this.pnlImg);
            this.Controls.Add(this.btnUploadImage);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnMessagePerson);
            this.Controls.Add(this.cbClients);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtInputMessage);
            this.Controls.Add(this.txtMessageDisplay);
            this.Name = "ClientForm";
            this.Text = "SimpleClient";
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlImg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtMessageDisplay;
        private System.Windows.Forms.RichTextBox txtInputMessage;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.ComboBox cbClients;
        private System.Windows.Forms.Button btnMessagePerson;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnUploadImage;
        private System.Windows.Forms.Panel pnlImg;
    }
}