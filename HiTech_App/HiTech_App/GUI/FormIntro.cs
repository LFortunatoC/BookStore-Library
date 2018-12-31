using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiTech.GUI
{
   

    public partial class FormIntro : Form
    {
        public static FormIntro FormIntroInstance;

        public FormIntro()
        {

            //Everyone eveywhere in the app should know me as Form1.Form1Instance
            FormIntroInstance = this;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

                progressBarLoading.PerformStep();
            if (progressBarLoading.Value >= 100)
            {
                progressBarLoading.Value = 100;
                timer1.Enabled = false;
                //Make sure I am kept hidden
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                Visible = false;
                FormLogin frmLogin = new FormLogin();
                frmLogin.TopMost = true; //since we open it from a minimezed window - it will not be focused unless we put it as TopMost.
                frmLogin.Show();
                frmLogin.Activate();
                frmLogin.TopMost = false;
            }

        }

        private void FormIntro_Load(object sender, EventArgs e)
        {
            progressBarLoading.Value = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
