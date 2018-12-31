using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech;
using HiTech.Security;
using HiTech.Validation;

//<div>Icons made by <a href="https://www.flaticon.com/authors/inipagistudio" title="inipagistudio">inipagistudio</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a> is licensed by <a href="http://creativecommons.org/licenses/by/3.0/" title="Creative Commons BY 3.0" target="_blank">CC 3.0 BY</a></div>

namespace HiTech.GUI
{
    public partial class FormLogin : Form
    {
        public bool IsOtherFormOpen = false;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (Validator.IsValidId(textBoxPassword.Text,4) && textBoxUserName.Text!="")
            { 
               if(Login.UserLogin(textBoxUserName.Text, textBoxPassword.Text)==true)
                {

                   switch (Login.CurUserLevel)
                    {
                        case (Login.UserLevel.MIS_MANAGER):
                            FormMain formMain = new FormMain();
                            formMain.Show();
                            IsOtherFormOpen = true;
                            this.Close();
                            break;

                        case (Login.UserLevel.SALES_MANAGER):
                            FormSalesMng formSales = new FormSalesMng();
                            formSales.Show();
                            this.Close();
                            break;

                        case (Login.UserLevel.CONTROLLER):
                            FormIventory frmIventory = new FormIventory();
                            frmIventory.Show();
                            this.Close();
                            break;

                        case (Login.UserLevel.CLERK):
                            FormOrder frmOrder = new FormOrder();
                            frmOrder.Show();
                            this.Close();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    textBoxPassword.Text = "";
                    textBoxUserName.Text = "";
                    textBoxUserName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Invalid User Name or Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Text = "";
                textBoxUserName.Text = "";
                textBoxUserName.Focus();
            }

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;  // Remove Close Button in the TOP of this form.

            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 300;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.buttonLogin, "Login");
            toolTip1.SetToolTip(this.buttonExit, "Cancel/Exit");
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void linkLabelForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact the system admistrator to reset your UserName/Password","Forgot User Name / Passowrd",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            byte Option = 0;
            //Ask for user confirmation before close the application
            Option = Convert.ToByte(MessageBox.Show("Do you really want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6) { Application.Exit(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChangePwd frmChangPwd = new FormChangePwd();
            frmChangPwd.ShowDialog();
        }
    }
}
