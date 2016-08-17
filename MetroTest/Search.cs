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
    public partial class Search : MetroForm

    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        string nombre = "";
        Modify mod;
        Delete del;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public Search()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
            this.Text = "Search " + tabla;
            
            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
        }

        private void search()
        {
            if (metroTextBox1.Text != "")
            {
                nombre = metroTextBox1.Text.ToUpper();
                //CLASIFICACION DE EVENTO
                metroGrid1.Rows.Clear();
                conex = new OdbcConnection(Properties.Settings.Default.Connection);
                conex.Open();
                command = conex.CreateCommand();

                if (metroComboBox1.Text == "ID")
                    command.CommandText = "select id,nombre,fecha,hora,colonia from t_evento where id like '%" + metroTextBox1.Text + "'";
                if (metroComboBox1.Text == "Name" && metroComboBox2.SelectedIndex == 0)
                    command.CommandText = "select id,nombre,fecha,hora,colonia from t_evento where nombre like '%" + metroTextBox1.Text + "%'";
                if (metroComboBox1.Text == "Name" && metroComboBox2.SelectedIndex == 1)
                    command.CommandText = "select id,nombre,fecha,hora,colonia from t_evento where nombre like '" + metroTextBox1.Text + "%'";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    metroGrid1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3).Substring(0, 8), reader.GetString(4));
                }
                reader.Close();
                conex.Close();
                metroGrid1.Focus();

            }
            else { MessageBox.Show("The name is Empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            search();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_Load(object sender, EventArgs e)
        {
            this.Text = "Search " + tabla;
        }

        private void metroGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid1.Rows[metroGrid1.CurrentRow.Index].Cells[0].Value.ToString();
            if (Properties.Settings.Default.TYPE == 3)
            {
                del = new Delete();
                del.Style = MetroFramework.MetroColorStyle.Blue;
                del.nombreTabla = "Event";
                del.ShowDialog();
                search();
            }
            if (Properties.Settings.Default.TYPE == 2)
            {
                mod = new Modify();
                mod.Style = MetroFramework.MetroColorStyle.Blue;
                mod.nombreTabla = "Event";
                mod.ShowDialog();
                search();
            }
        }



    }
}
