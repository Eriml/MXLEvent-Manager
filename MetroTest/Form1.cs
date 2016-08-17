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
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        private void metroTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            metroLabel1.Text = Convert.ToString(metroTrackBar1.Value);
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataSet1.a_lecturas' Puede moverla o quitarla según sea necesario.
            this.a_lecturasTableAdapter.Fill(this.dataSet1.a_lecturas);

        }

        private void metroRadioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

 


    }
}
