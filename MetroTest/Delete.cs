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

namespace MetroTest
{
    public partial class Delete : MetroForm
    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        string hora;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }

        public Delete()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
            for(int i=1;i<12;i++)
                metroComboBox1.Items.Add(i.ToString());
            for (int i = 1; i <= 12; i++)
                metroComboBox1.Items.Add(i.ToString());
            for (int i = 0; i <= 59; i++)
            {
                if (i.ToString().Length== 1) metroComboBox2.Items.Add("0"+i.ToString());
                else
                metroComboBox2.Items.Add(i.ToString());
            }
        }

        //LOADING THE INFORMATION FROM THE EVENT
        private void New_Load(object sender, EventArgs e)
        {
            this.Text = "Delete " + tabla;
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select * from t_evento where id='"+Properties.Settings.Default.ID+"'";
            reader = command.ExecuteReader();
            
            reader.Read();
            
            metroTextBox1.Text=reader.GetString(1);
            if (Convert.ToInt32(reader.GetString(2).Substring(0, 2)) > 12)
            {
                metroComboBox3.Text = "P.M.";
                hora = (Convert.ToInt32(reader.GetString(2).Substring(0, 2)) - 12).ToString();
            }

            else
            {
                metroComboBox3.Text = "A.M.";
                hora = reader.GetString(2).Substring(0, 2);
            }
            if (hora.ElementAt(0) == '0') hora = hora.Substring(1, 1);
            metroComboBox1.Text = hora;
            metroComboBox2.Text = reader.GetString(2).Substring(3, 2);
            metroDateTime1.Value =Convert.ToDateTime( reader.GetString(3).Substring(8, 2) + "/" + reader.GetString(3).Substring(5, 2) + "/" + reader.GetString(3).Substring(0, 4)+" 00:00:00");
            metroTextBox6.Text=reader.GetString(4);
            metroTextBox3.Text = reader.GetString(5);
            metroTextBox2.Text = reader.GetString(6);
            metroTextBox4.Text = reader.GetString(8);
            metroTextBox5.Text = reader.GetString(9);
            reader.Close();
            conex.Close();

        }

        //CLOSE BUTTON
        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //CONFIRM DELETE BUTTON
        private void metroButton1_Click(object sender, EventArgs e)
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "delete from t_evento where id='" + Properties.Settings.Default.ID + "'";
            reader = command.ExecuteReader();
            reader.Close();
            conex.Close();
            this.Close();
        }
    }
}
