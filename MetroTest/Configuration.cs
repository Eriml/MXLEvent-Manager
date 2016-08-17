using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Data.Odbc;
using Ini;
using Microsoft.Win32;

namespace MetroTest
{
    public partial class Configuration : MetroForm
    {
        OdbcConnection conex;
        string DSN;

        public Configuration()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
        }


        //LOADING THE DATA ALREADY EXISTENT
        private void Configuration_Load(object sender, EventArgs e)
        {
            int pFrom = Properties.Settings.Default.Connection.IndexOf("DSN=") + "DSN=".Length;
            int pTo = Properties.Settings.Default.Connection.LastIndexOf(";uid=");
            string ODBC = Properties.Settings.Default.Connection.Substring(pFrom, pTo - pFrom);
            metroTextBox1.Text = ODBC;
            pFrom = Properties.Settings.Default.Connection.IndexOf("uid=") + "uid=".Length;
            pTo = Properties.Settings.Default.Connection.LastIndexOf(";pwd=");
            string user = Properties.Settings.Default.Connection.Substring(pFrom, pTo - pFrom);
            metroTextBox2.Text=user;
            pFrom = Properties.Settings.Default.Connection.IndexOf(";pwd=") + ";pwd=".Length;
            pTo = Properties.Settings.Default.Connection.LastIndexOf(";");
            string pass = Properties.Settings.Default.Connection.Substring(pFrom, pTo - pFrom);
            metroTextBox3.Text=pass;
        }

        //CONNECTION EVENT
        private void tryConnection()
        {
            DSN = "DSN=" + metroTextBox1.Text + ";uid=" + metroTextBox2.Text + ";pwd=" + metroTextBox3.Text;
            RegistryKey connectionString = Registry.CurrentUser.CreateSubKey(@"Eriml\Event Manager");
            connectionString.SetValue("Connection String", DSN);
            conex = new OdbcConnection(DSN);

            try { conex.Open(); }
            catch (Exception)
            {
                MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (conex.State.ToString() == "Open")
            {
                RegistryKey dsnr = Registry.CurrentUser.OpenSubKey(@"Eriml\Event Manager\", false);
                if (dsnr != null)
                {
                    MessageBox.Show("NO es nulo");

                    MessageBox.Show(dsnr.GetValue("DSN").ToString());
                }
                else
                    MessageBox.Show("Es nulo");


                Properties.Settings.Default.Connection = DSN;
                Properties.Settings.Default.Connected = true;
                conex.Close();

                //SAVES THE SETTINGS IN THE REGISTRY
                Microsoft.Win32.RegistryKey DSNkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Eriml\Event Manager");
                DSNkey.SetValue("DSN", metroTextBox1.Text);
                DSNkey.Close();
                Microsoft.Win32.RegistryKey USER = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Eriml\Event Manager");
                USER.SetValue("User", metroTextBox2.Text);
                USER.Close();
                Microsoft.Win32.RegistryKey PASS = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Eriml\Event Manager");
                PASS.SetValue("PWD", metroTextBox3.Text);
                PASS.Close();
                RegistryKey cString = Registry.CurrentUser.CreateSubKey(@"Eriml\Event Manager");
                cString.SetValue("Connection String", DSN);
                cString.Close();
                this.Close();
            }
        }

        //CONNECT BUTTON
        private void metroButton1_Click(object sender, EventArgs e)
        {
            tryConnection();
        }

        //CLOSE BUTTON
        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
