namespace WSDemo.Client.Win
{
    partial class MainForm
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
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btDeleteKeyPair = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbUser
            // 
            this.tbUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUser.Location = new System.Drawing.Point(85, 12);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(256, 20);
            this.tbUser.TabIndex = 0;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(85, 38);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(256, 20);
            this.tbPassword.TabIndex = 1;
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Location = new System.Drawing.Point(12, 15);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(61, 13);
            this.lbUser.TabIndex = 2;
            this.lbUser.Text = "User name:";
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(12, 41);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(56, 13);
            this.lbPassword.TabIndex = 3;
            this.lbPassword.Text = "Password:";
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(85, 64);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(75, 23);
            this.btConnect.TabIndex = 4;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // btDisconnect
            // 
            this.btDisconnect.Location = new System.Drawing.Point(166, 64);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 5;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(15, 93);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(326, 109);
            this.tbLog.TabIndex = 6;
            // 
            // btDeleteKeyPair
            // 
            this.btDeleteKeyPair.Location = new System.Drawing.Point(247, 64);
            this.btDeleteKeyPair.Name = "btDeleteKeyPair";
            this.btDeleteKeyPair.Size = new System.Drawing.Size(94, 23);
            this.btDeleteKeyPair.TabIndex = 7;
            this.btDeleteKeyPair.Text = "Delete key pair";
            this.btDeleteKeyPair.UseVisualStyleBackColor = true;
            this.btDeleteKeyPair.Click += new System.EventHandler(this.btDeleteKeyPair_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 214);
            this.Controls.Add(this.btDeleteKeyPair);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUser);
            this.MinimumSize = new System.Drawing.Size(369, 253);
            this.Name = "MainForm";
            this.Text = "WebSocket Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btDeleteKeyPair;
    }
}

