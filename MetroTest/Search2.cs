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
    public partial class Search2 : MetroForm

    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        string nombre = "";
        Modify2 mod;
        Delete2 del;
        string tabla2;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public Search2()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            search();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    command.CommandText = "select id,nombre,horaabre,horacierra,colonia from " + tabla2 + " where id like '%" + metroTextBox1.Text + "'";
                if (metroComboBox1.Text == "Name" && metroComboBox2.SelectedIndex == 0)
                    command.CommandText = "select id,nombre,horaabre,horacierra,colonia from " + tabla2 + " where nombre like '%" + metroTextBox1.Text + "%'";
                if (metroComboBox1.Text == "Name" && metroComboBox2.SelectedIndex == 1)
                    command.CommandText = "select id,nombre,horaabre,horacierra,colonia from " + tabla2 + " where nombre like '" + metroTextBox1.Text + "%'";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    metroGrid1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2).Substring(0,8), reader.GetString(3).Substring(0, 8), reader.GetString(4));
                }
                reader.Close();
                conex.Close();
                metroGrid1.Focus();

            }
            else { MessageBox.Show("The name is Empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        
        private void Search2_Load(object sender, EventArgs e)
        {
            this.Text = "Search " + tabla;
            if (tabla == "Restaurant") 
            { 
                metroGrid1.Style = MetroFramework.MetroColorStyle.Green;
                tabla2 = "t_restaurante";
            }
            if (tabla == "Club/Bar") 
            {
                metroGrid1.Style = MetroFramework.MetroColorStyle.Purple;
                tabla2 = "t_antrobar";
            }
            if (tabla == "Other") 
            {
                metroGrid1.Style = MetroFramework.MetroColorStyle.Red;
                tabla2 = "t_otro";
            }

            
        }

        private void metroGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid1.Rows[metroGrid1.CurrentRow.Index].Cells[0].Value.ToString();
            if (Properties.Settings.Default.TYPE == 3)
            {
                del = new Delete2();
                if (tabla == "Restaurant")
                {
                    del.Style = MetroFramework.MetroColorStyle.Green;
                }
                if (tabla == "Club/Bar")
                {
                    del.Style = MetroFramework.MetroColorStyle.Purple;
                }
                if (tabla == "Other")
                {
                    del.Style = MetroFramework.MetroColorStyle.Red;
                }
                del.nombreTabla = tabla;
                del.ShowDialog();
                search();
            }
            if (Properties.Settings.Default.TYPE == 2)
            {
                mod = new Modify2();
                if (tabla == "Restaurant")
                {
                    mod.Style = MetroFramework.MetroColorStyle.Green;
                }
                if (tabla == "Club/Bar")
                {
                    mod.Style = MetroFramework.MetroColorStyle.Purple;
                }
                if (tabla == "Other")
                {
                    mod.Style = MetroFramework.MetroColorStyle.Red;
                }
                mod.nombreTabla = tabla;
                mod.ShowDialog();
                search();
            }
        }



    }
}
