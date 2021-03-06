﻿using System;
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
    public partial class Modify2 : MetroForm
    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        Browser browser;
        string hora;
        string hora2;
        string tabla2;
        string tabla3;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public Modify2()
        {
            InitializeComponent();
            this.AcceptButton = metroButton2;
            this.CancelButton = metroButton2;
            for (int i = 1; i <= 12; i++)
            { metroComboBox1.Items.Add(i.ToString()); }
            for (int i = 0; i <= 59; i++)
            {
                if (i.ToString().Length == 2)
                    metroComboBox2.Items.Add(i.ToString());
                else metroComboBox2.Items.Add("0" + i.ToString());
            }
            for (int i = 1; i <= 12; i++)
            { metroComboBox4.Items.Add(i.ToString()); }
            for (int i = 0; i <= 59; i++)
            {
                if (i.ToString().Length == 2)
                    metroComboBox5.Items.Add(i.ToString());
                else metroComboBox5.Items.Add("0" + i.ToString());
            }
            
        }

        private void New2_Load(object sender, EventArgs e)
        {
            this.Text = "Modifying " + tabla;
            if (tabla == "Restaurant")
            {
                tabla2 = "t_restaurante";
                tabla3 = "t_clasificacionres";
            }
            if (tabla == "Club/Bar")
            {
                tabla2 = "t_antrobar";
                tabla3 = "t_clasificacionbar";
            }
            if (tabla == "Other")
            {
                tabla2 = "t_otro";
                tabla3 = "t_clasificacionotro";
            }
            addCategories();
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select * from " + tabla2 + " where id='" + Properties.Settings.Default.ID + "'";
            reader = command.ExecuteReader();

            reader.Read();

            metroTextBox1.Text = reader.GetString(1);
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

            if (Convert.ToInt32(reader.GetString(3).Substring(0, 2)) > 12)
            {
                metroComboBox6.Text = "P.M.";
                hora2 = (Convert.ToInt32(reader.GetString(3).Substring(0, 2)) - 12).ToString();
            }

            else
            {
                metroComboBox6.Text = "A.M.";
                hora2 = reader.GetString(3).Substring(0, 2);
            }
            if (hora2.ElementAt(0) == '0') hora2 = hora2.Substring(1, 1);

            metroComboBox1.Text = hora;
            metroComboBox2.Text = reader.GetString(2).Substring(3, 2);
            metroComboBox4.Text = hora2;
            metroComboBox5.Text = reader.GetString(3).Substring(3, 2);
            metroTextBox6.Text = reader.GetString(4);
            metroTextBox3.Text = reader.GetString(5);
            metroTextBox2.Text = reader.GetString(6);
            metroTextBox4.Text = reader.GetString(8);
            metroTextBox5.Text = reader.GetString(9);
            metroTextBox7.Text = reader.GetString(10);
            reader.Close();
            command.CommandText = "select clasificacion from "+tabla3+" where id=(select clasificacion from "+tabla2+" where id='" + Properties.Settings.Default.ID + "')";
            reader = command.ExecuteReader();
            while (reader.Read())
                metroComboBox7.Text = reader.GetString(0);
            reader.Close();
            conex.Close();
        }

        private void addCategories()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select clasificacion from "+tabla3;
            reader = command.ExecuteReader();
            while (reader.Read())
            { metroComboBox7.Items.Add(reader.GetString(0)); }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "" || metroTextBox2.Text == "" || metroTextBox3.Text == "" || metroTextBox4.Text == "" || metroTextBox5.Text == "" || metroTextBox6.Text == "")
                MessageBox.Show("One or more fields are Empty.", "Empty fields!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                conex = new OdbcConnection(Properties.Settings.Default.Connection);
                conex.Open();
                command = conex.CreateCommand();
                if (metroComboBox3.Text == "P.M." && metroComboBox1.Text != "12") hora = (Convert.ToInt32(metroComboBox1.Text) + 12).ToString();
                else if (metroComboBox3.Text == "A.M." && metroComboBox1.Text == "12") hora = "00";
                else
                {
                    if (metroComboBox1.Text.Length == 1) hora = "0" + metroComboBox1.Text;
                    else hora = metroComboBox1.Text;
                }

                if (metroComboBox6.Text == "P.M." && metroComboBox4.Text != "12") hora2 = (Convert.ToInt32(metroComboBox4.Text) + 12).ToString();
                else if (metroComboBox6.Text == "A.M." && metroComboBox4.Text == "12") hora2 = "00";
                else
                {
                    if (metroComboBox4.Text.Length == 1) hora2 = "0" + metroComboBox4.Text;
                    else hora2 = metroComboBox4.Text;

                }

                hora = hora + ":" + metroComboBox2.Text + ":00";
                hora2 = hora2 + ":" + metroComboBox5.Text + ":00";
                command.CommandText = "update "+tabla2+" set nombre='" + metroTextBox1.Text + "',calle='" + metroTextBox2.Text + "',colonia='" + metroTextBox3.Text + "',ciudad='" + metroTextBox4.Text +
                "',estado='" + metroTextBox5.Text + "',descripcion='" + metroTextBox6.Text + "',clasificacion=" + metroComboBox7.SelectedIndex.ToString() + ",horaabre='" + hora + "',horacierra='" + hora2 + "'" +
                ",pagina='" + metroTextBox7.Text + "' where id='" + Properties.Settings.Default.ID + "'";
                command.ExecuteReader();

                conex.Close();
                this.Close();

            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            browser = new Browser();
            browser.webPage = metroTextBox7.Text;
            browser.ShowDialog();
        }
    }
}
