namespace HiTech.GUI
{
    partial class FormChangePwd
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
            this.textBoxCurrPwd = new System.Windows.Forms.TextBox();
            this.textBoxNewPwd = new System.Windows.Forms.TextBox();
            this.textBoxConfirmPwd = new System.Windows.Forms.TextBox();
            this.buttonChangePwd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxCurrPwd
            // 
            this.textBoxCurrPwd.Location = new System.Drawing.Point(163, 28);
            this.textBoxCurrPwd.Name = "textBoxCurrPwd";
            this.textBoxCurrPwd.PasswordChar = '*';
            this.textBoxCurrPwd.Size = new System.Drawing.Size(127, 22);
            this.textBoxCurrPwd.TabIndex = 1;
            // 
            // textBoxNewPwd
            // 
            this.textBoxNewPwd.Location = new System.Drawing.Point(13, 83);
            this.textBoxNewPwd.Name = "textBoxNewPwd";
            this.textBoxNewPwd.PasswordChar = '*';
            this.textBoxNewPwd.Size = new System.Drawing.Size(127, 22);
            this.textBoxNewPwd.TabIndex = 2;
            // 
            // textBoxConfirmPwd
            // 
            this.textBoxConfirmPwd.Location = new System.Drawing.Point(163, 83);
            this.textBoxConfirmPwd.Name = "textBoxConfirmPwd";
            this.textBoxConfirmPwd.PasswordChar = '*';
            this.textBoxConfirmPwd.Size = new System.Drawing.Size(127, 22);
            this.textBoxConfirmPwd.TabIndex = 3;
            this.textBoxConfirmPwd.TextChanged += new System.EventHandler(this.textBoxConfirmPwd_TextChanged);
            // 
            // buttonChangePwd
            // 
            this.buttonChangePwd.Location = new System.Drawing.Point(13, 126);
            this.buttonChangePwd.Name = "buttonChangePwd";
            this.buttonChangePwd.Size = new System.Drawing.Size(127, 31);
            this.buttonChangePwd.TabIndex = 4;
            this.buttonChangePwd.Text = "Change";
            this.buttonChangePwd.UseVisualStyleBackColor = true;
            this.buttonChangePwd.Click += new System.EventHandler(this.buttonChangePwd_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(163, 126);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(127, 31);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "New Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Confirm Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "User Name:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(12, 28);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(127, 22);
            this.textBoxUserName.TabIndex = 0;
            // 
            // FormChangePwd
            // 
            this.AcceptButton = this.buttonChangePwd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 168);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonChangePwd);
            this.Controls.Add(this.textBoxConfirmPwd);
            this.Controls.Add(this.textBoxNewPwd);
            this.Controls.Add(this.textBoxCurrPwd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormChangePwd";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HiTech Change User Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCurrPwd;
        private System.Windows.Forms.TextBox textBoxNewPwd;
        private System.Windows.Forms.TextBox textBoxConfirmPwd;
        private System.Windows.Forms.Button buttonChangePwd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxUserName;
    }
}