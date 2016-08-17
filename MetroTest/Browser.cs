using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace MetroTest
{
    public partial class Browser : MetroForm
    {
        string page;

        public string webPage
        {
            get { return page; }
            set { page = value; }
        }

        public Browser()
        {
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(page);
        }
    }
}
