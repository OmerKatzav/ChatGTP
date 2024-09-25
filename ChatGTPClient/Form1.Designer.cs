namespace ChatGTPClient
{
    partial class Form1
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
            this.messageListBox = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nickBox = new System.Windows.Forms.ComboBox();
            this.privMessageBtn = new System.Windows.Forms.Button();
            this.promoteBtn = new System.Windows.Forms.Button();
            this.kickBtn = new System.Windows.Forms.Button();
            this.muteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageListBox
            // 
            this.messageListBox.FormattingEnabled = true;
            this.messageListBox.Location = new System.Drawing.Point(12, 42);
            this.messageListBox.Name = "messageListBox";
            this.messageListBox.Size = new System.Drawing.Size(496, 264);
            this.messageListBox.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(219, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(12, 314);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(415, 20);
            this.messageTextBox.TabIndex = 2;
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageTextBox_KeyDown);
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(433, 313);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(75, 21);
            this.sendBtn.TabIndex = 3;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(644, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nickBox
            // 
            this.nickBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nickBox.FormattingEnabled = true;
            this.nickBox.Location = new System.Drawing.Point(514, 42);
            this.nickBox.Name = "nickBox";
            this.nickBox.Size = new System.Drawing.Size(118, 21);
            this.nickBox.TabIndex = 5;
            // 
            // privMessageBtn
            // 
            this.privMessageBtn.Location = new System.Drawing.Point(514, 69);
            this.privMessageBtn.Name = "privMessageBtn";
            this.privMessageBtn.Size = new System.Drawing.Size(118, 23);
            this.privMessageBtn.TabIndex = 6;
            this.privMessageBtn.Text = "Private Message";
            this.privMessageBtn.UseVisualStyleBackColor = true;
            this.privMessageBtn.Click += new System.EventHandler(this.privMessageBtn_Click);
            // 
            // promoteBtn
            // 
            this.promoteBtn.Enabled = false;
            this.promoteBtn.Location = new System.Drawing.Point(514, 98);
            this.promoteBtn.Name = "promoteBtn";
            this.promoteBtn.Size = new System.Drawing.Size(118, 23);
            this.promoteBtn.TabIndex = 7;
            this.promoteBtn.Text = "Promote";
            this.promoteBtn.UseVisualStyleBackColor = true;
            this.promoteBtn.Click += new System.EventHandler(this.promoteBtn_Click);
            // 
            // kickBtn
            // 
            this.kickBtn.Location = new System.Drawing.Point(514, 156);
            this.kickBtn.Name = "kickBtn";
            this.kickBtn.Size = new System.Drawing.Size(118, 23);
            this.kickBtn.TabIndex = 8;
            this.kickBtn.Text = "Kick";
            this.kickBtn.UseVisualStyleBackColor = true;
            this.kickBtn.Click += new System.EventHandler(this.kickBtn_Click);
            // 
            // muteBtn
            // 
            this.muteBtn.Location = new System.Drawing.Point(514, 127);
            this.muteBtn.Name = "muteBtn";
            this.muteBtn.Size = new System.Drawing.Size(118, 23);
            this.muteBtn.TabIndex = 9;
            this.muteBtn.Text = "Mute";
            this.muteBtn.UseVisualStyleBackColor = true;
            this.muteBtn.Click += new System.EventHandler(this.muteBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 343);
            this.Controls.Add(this.muteBtn);
            this.Controls.Add(this.kickBtn);
            this.Controls.Add(this.promoteBtn);
            this.Controls.Add(this.privMessageBtn);
            this.Controls.Add(this.nickBox);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.messageListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox messageListBox;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ComboBox nickBox;
        private System.Windows.Forms.Button privMessageBtn;
        private System.Windows.Forms.Button promoteBtn;
        private System.Windows.Forms.Button kickBtn;
        private System.Windows.Forms.Button muteBtn;
    }
}

