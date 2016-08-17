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
    public partial class New : MetroForm
    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        Browser browser;
        int last;
        string id;
        string hora;
        string fecha;

        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public New()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
            for (int i = 1; i < 12; i++)
                metroComboBox1.Items.Add(i.ToString());
            for (int i = 1; i <= 12; i++)
                metroComboBox1.Items.Add(i.ToString());
            for (int i = 0; i <= 59; i++)
            {
                if (i.ToString().Length == 1) metroComboBox2.Items.Add("0" + i.ToString());
                else
                    metroComboBox2.Items.Add(i.ToString());
            }
            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
            metroComboBox3.SelectedIndex = 0;
            metroTextBox4.Text = "Mexicali";
            metroTextBox5.Text = "Baja California";

        }

        private void New_Load(object sender, EventArgs e)
        {
            this.Text = "New " + tabla;
            addCategories();
            metroComboBox4.SelectedIndex = 0;
        }
        private void addCategories()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select clasificacion from t_clasificacion";
            reader = command.ExecuteReader();
            while (reader.Read())
            { metroComboBox4.Items.Add(reader.GetString(0)); }
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
                command.CommandText = "select id from t_evento";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    last = reader.GetInt32(0);
                }
                id = (last + 1).ToString();
                while (id.Length < 5)
                { id = "0" + id; }
                reader.Close();
                if (metroComboBox3.Text == "P.M." && metroComboBox1.Text != "12") hora = (Convert.ToInt32(metroComboBox1.Text) + 12).ToString();
                else if (metroComboBox3.Text == "A.M." && metroComboBox1.Text == "12") hora = "00";
                else
                {
                    if (metroComboBox1.Text.Length == 1) hora = "0" + metroComboBox1.Text;
                    else hora = metroComboBox1.Text;
                }

                hora = hora + ":" + metroComboBox2.Text + ":00";
                fecha = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                command.CommandText = "insert into t_evento (id,nombre,calle,colonia,ciudad,estado,descripcion,clasificacion,hora,fecha) values('" + id + "','" + metroTextBox1.Text +
                    "','" + metroTextBox2.Text + "','" + metroTextBox3.Text + "','" + metroTextBox4.Text + "','" + metroTextBox5.Text + "','" + metroTextBox6.Text +
                    "','" + metroComboBox4.SelectedIndex.ToString() + "','" + hora + "','" + fecha + "'" + ")";
                Console.WriteLine(command.CommandText);
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
