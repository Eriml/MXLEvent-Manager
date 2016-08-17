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
    public partial class New2 : MetroForm
    {
        private string tabla;
        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        int last;
        string tabla2;
        string id;
        string hora;
        string hora2;
        string tabla3;
        public string nombreTabla
        {
            get { return tabla; }
            set { tabla = value; }
        }


        public New2()
        {
            InitializeComponent();
            this.AcceptButton = metroButton1;
            this.CancelButton = metroButton2;
            for (int i = 1; i <= 12; i++)
            { metroComboBox1.Items.Add(i.ToString()); }
            for (int i = 0; i <= 59; i++)
            { if(i.ToString().Length==2)
                metroComboBox2.Items.Add(i.ToString());
            else metroComboBox2.Items.Add("0"+i.ToString());
            }
            for (int i = 1; i <= 12; i++)
            { metroComboBox4.Items.Add(i.ToString()); }
            for (int i = 0; i <= 59; i++)
            {
                if (i.ToString().Length == 2)
                    metroComboBox5.Items.Add(i.ToString());
                else metroComboBox5.Items.Add("0" + i.ToString());
            }
            metroComboBox1.SelectedIndex=0;
            metroComboBox2.SelectedIndex=0;
            metroComboBox3.SelectedIndex=0;
            metroComboBox4.SelectedIndex=0;
            metroComboBox5.SelectedIndex=0;
            metroComboBox6.SelectedIndex=1;
            metroTextBox4.Text = "Mexicali";
            metroTextBox5.Text = "Baja California";

        }

        private void addCategories()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select clasificacion from " + tabla3;
            reader = command.ExecuteReader();
            while (reader.Read())
            { metroComboBox7.Items.Add(reader.GetString(0)); }
        }
        private void New2_Load(object sender, EventArgs e)
        {
            this.Text = "New " + tabla;
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
            metroComboBox7.SelectedIndex = 0;
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
                command.CommandText = "select id from " + tabla2;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    last = reader.GetInt32(0);
                }
                id = (last+1).ToString();
                while (id.Length < 5)
                { id = "0" + id; }
                reader.Close();
                if (metroComboBox3.Text == "P.M."&&metroComboBox1.Text!="12") hora = (Convert.ToInt32(metroComboBox1.Text) + 12).ToString();
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
                    if (metroComboBox4.Text.Length == 1) hora2= "0" + metroComboBox4.Text;
                    else hora2 = metroComboBox4.Text;
                    
                }
                
                hora = hora + ":" + metroComboBox2.Text + ":00";
                hora2 = hora2 + ":" + metroComboBox5.Text + ":00";
                command.CommandText = "insert into "+tabla2+"(id,nombre,calle,colonia,ciudad,estado,descripcion,clasificacion,horaabre,horacierra,pagina) values('"+id+"','"+metroTextBox1.Text+
                    "','"+metroTextBox2.Text+"','"+metroTextBox3.Text+"','"+metroTextBox4.Text+"','"+metroTextBox5.Text+"','"+metroTextBox6.Text+
                    "','"+metroComboBox7.SelectedIndex.ToString()+"','"+hora+"','"+hora2+"','"+metroTextBox7.Text+"')";
                Console.WriteLine(command.CommandText);
                command.ExecuteReader();
                
                conex.Close();
                this.Close();

            }
       }
    }
}
