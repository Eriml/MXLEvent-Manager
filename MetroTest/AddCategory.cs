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
    public partial class AddCategory : MetroForm

    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        string clave = "";
        string nombre = "";
        int altas=0;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public AddCategory()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text != "")
            {
                nombre = metroTextBox1.Text.ToUpper();
                //CLASIFICACION DE EVENTO
                if (tabla == "Event")
                {
                    conex = new OdbcConnection(Properties.Settings.Default.Connection);
                    conex.Open();
                    command = conex.CreateCommand();
                    command.CommandText = "select * from t_clasificacion where clasificacion='" + nombre + "'";
                    reader = command.ExecuteReader();

                    if (reader.HasRows) MessageBox.Show("Classification already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        command.CommandText = "select * from t_clasificacion";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            altas++;
                        }
                        reader.Close();
                        clave = Convert.ToString(altas);
                        command.CommandText = "insert into t_clasificacion values('" + clave + "','" + nombre + "')";
                        reader = command.ExecuteReader();
                        reader.Close();
                        this.Close();
                    }

                    conex.Close();
                }
                //CLASIFICACION DE RESTAURANTE
                if (tabla == "Restaurant")
                {
                    conex = new OdbcConnection(Properties.Settings.Default.Connection);
                    conex.Open();
                    command = conex.CreateCommand();
                    command.CommandText = "select * from t_clasificacionres where clasificacion='" + nombre + "'";
                    reader = command.ExecuteReader();

                    if (reader.HasRows) MessageBox.Show("Classification already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        command.CommandText = "select * from t_clasificacionres";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            altas++;
                        }
                        reader.Close();
                        clave = Convert.ToString(altas);
                        command.CommandText = "insert into t_clasificacionres values('" + clave + "','" + nombre + "')";
                        reader = command.ExecuteReader();
                        reader.Close();
                        this.Close();
                    }

                    conex.Close();
                }
                //CLASIFICACION DE ANTRO
                if (tabla == "Club/Bar")
                {
                    conex = new OdbcConnection(Properties.Settings.Default.Connection);
                    conex.Open();
                    command = conex.CreateCommand();
                    command.CommandText = "select * from t_clasificacionbar where clasificacion='" + nombre + "'";
                    reader = command.ExecuteReader();

                    if (reader.HasRows) MessageBox.Show("Classification already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        command.CommandText = "select * from t_clasificacionbar";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            altas++;
                        }
                        reader.Close();
                        clave = Convert.ToString(altas);
                        command.CommandText = "insert into t_clasificacionbar values('" + clave + "','" + nombre + "')";
                        reader = command.ExecuteReader();
                        reader.Close();
                        this.Close();
                    }

                    conex.Close();
                }

                //CLASIFICACION DE OTROS
                if (tabla == "Other")
                {
                    conex = new OdbcConnection(Properties.Settings.Default.Connection);
                    conex.Open();
                    command = conex.CreateCommand();
                    command.CommandText = "select * from t_clasificacionotro where clasificacion='" + nombre + "'";
                    reader = command.ExecuteReader();

                    if (reader.HasRows) MessageBox.Show("Classification already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        command.CommandText = "select * from t_clasificacionotro";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            altas++;
                        }
                        reader.Close();
                        clave = Convert.ToString(altas);
                        command.CommandText = "insert into t_clasificacionotro values('" + clave + "','" + nombre + "')";
                        reader = command.ExecuteReader();
                        reader.Close();
                        this.Close();
                    }

                    conex.Close();
                }
            }
            else { MessageBox.Show("The name is Empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
