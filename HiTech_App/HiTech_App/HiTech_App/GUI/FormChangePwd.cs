using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech.Security;
using HiTech.Validation;

namespace HiTech.GUI
{
    public partial class FormChangePwd : Form
    {
        public FormChangePwd()
        {
            InitializeComponent();
        }

        private void textBoxConfirmPwd_TextChanged(object sender, EventArgs e)
        {
            if(textBoxConfirmPwd.Text!=textBoxNewPwd.Text)
            {
                textBoxConfirmPwd.ForeColor = Color.Red;
            }
            else
            {
                textBoxConfirmPwd.ForeColor = Color.Green;
            }
        }

        private void buttonChangePwd_Click(object sender, EventArgs e)
        {
            if(textBoxUserName.TextLength == 0 )
            {
                MessageBox.Show("Please enter a valid User Name","Invalid User Name",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxCurrPwd.TextLength >0) 
                if((textBoxNewPwd.TextLength>0) && (textBoxNewPwd.Text== textBoxConfirmPwd.Text))
                {
                    if (!(Validator.IsValidId(textBoxNewPwd.Text, 4)))
                    {
                        MessageBox.Show("Password length must be 4 and can include letters or numbers","Wrong Password Length",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        textBoxNewPwd.Clear();
                        textBoxConfirmPwd.Clear();
                        textBoxNewPwd.Focus();
                        return;
                    }

                    if (Login.UserLogin(textBoxUserName.Text, textBoxCurrPwd.Text) == true)
                    {
                        Login.ChangePwd(Login.LoggedUserId, textBoxCurrPwd.Text, textBoxNewPwd.Text);
                        Login.UserLogout();
                        //textBoxConfirmPwd.ForeColor = Color.Black;
                        //textBoxConfirmPwd.Clear();
                        //textBoxCurrPwd.Clear();
                        //textBoxNewPwd.Clear();
                        MessageBox.Show("Password Changed Susscesfully\nYou need to Login now using your new password","Change Password Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        //MessageBox.Show("Invalid User Name or Current Password","Login Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show("New Password and Confirmation doesn't match.", "New Password doesn't match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxNewPwd.Clear();
                    textBoxConfirmPwd.Clear();
                    textBoxNewPwd.Focus();
                }
            else
            {
                MessageBox.Show("Please enter current password.","Missing Current Password",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                textBoxCurrPwd.Clear();
                textBoxCurrPwd.Focus();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            byte Option = Convert.ToByte(MessageBox.Show("Cancel Change Password ?","Cancel Password change", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6)
            {
                this.Close();
            }
        }
    }
}
