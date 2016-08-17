using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;

namespace MetroTest
{
    public partial class SplashScreen : MetroForm
    {
        Main main = new Main();
 
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
            
            main.Show();
            this.Hide();
        }


    }
}
