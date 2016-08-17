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

namespace MetroTest
{
    public partial class Main : MetroForm
    {

        OdbcConnection conex;
        OdbcDataReader reader;
        OdbcCommand command;
        AddCategory addC;
        New add;
        New2 add2;
        Modify mod;
        Modify2 mod2;
        Delete del;
        Delete2 del2;
        Details det;
        Details2 det2;
        Search search;
        Search2 search2;
        bool connected;
        Configuration config;
        Browser browser;
        string path;
        IniFile ini;
        string website;
        
        public Main()
        {
            InitializeComponent();
            metroTabControl1.SelectedIndex = 0;
            connected = false;
            getPath();
            string dsn = ini.IniReadValue("Default", "DSN");
            string user = ini.IniReadValue("Default", "user");
            string pass = ini.IniReadValue("Default", "pass");
            website = ini.IniReadValue("Information", "website");
            Properties.Settings.Default.Connection = "DSN="+dsn+";uid="+user+";pwd="+pass+";";
            tryConnection();
        }

        private void getPath()
        {
            path = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6, path.Length - 6)+"/config.ini";
            path=path.Replace("\\","/");
            ini = new IniFile(path);
        }

        //TRY CONNECTION 
        private void tryConnection()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            try { conex.Open(); }
            catch (Exception) 
            {
                
            }
            if (conex.State.ToString() == "Open")
            {
                connected = true;
                Properties.Settings.Default.Connected = true;
                updateEvents();
                updateRestaurants();
                updateBars();
                updateOthers();
                conex.Close();
            }
        }

        //UPDATE EVENTS
        private void updateEvents()
        {   
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select id,nombre,fecha,hora,colonia,calle,descripcion,ciudad,estado from t_evento";
            reader = command.ExecuteReader();
            metroGrid1.Rows.Clear();
            while (reader.Read())
            {
                metroGrid1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3).Substring(0, 8), reader.GetString(4), reader.GetString(5),
                     reader.GetString(6), reader.GetString(7), reader.GetString(8));
            }
            conex.Close();
        }

        //UPDATE RESTAURANTS
        private void updateRestaurants()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select id,nombre,horaabre,horacierra,colonia from t_restaurante";
            reader = command.ExecuteReader();
            metroGrid2.Rows.Clear();
            while (reader.Read())
            {
                metroGrid2.Rows.Add(reader.GetString(0), reader.GetString(1),
                    reader.GetString(2).Substring(0, 8), reader.GetString(3).Substring(0, 8), reader.GetString(4));
            }
            conex.Close();
        }

        //UPDATE BARS
        private void updateBars()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select id,nombre,horaabre,horacierra,colonia  from t_antrobar";
            reader = command.ExecuteReader();
            metroGrid3.Rows.Clear();
            while (reader.Read())
            {
                metroGrid3.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2).Substring(0, 8), reader.GetString(3).Substring(0, 8), reader.GetString(4));
            }
            conex.Close();
        }

        //UPDATE OTHERS
        private void updateOthers()
        {
            conex = new OdbcConnection(Properties.Settings.Default.Connection);
            conex.Open();
            command = conex.CreateCommand();
            command.CommandText = "select id,nombre,horaabre,horacierra,colonia  from t_otro";
            reader = command.ExecuteReader();
            metroGrid4.Rows.Clear();
            while (reader.Read())
            {
                metroGrid4.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2).Substring(0, 8), reader.GetString(3).Substring(0, 8), reader.GetString(4));
            }
            conex.Close();
        }

        //CLOSE BUTTON
        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //ADD CATEGORY EVENT
        private void addCategory(string table)
        {
            if (connected)
            {
                addC = new AddCategory();
                if (table == "Event") metroGrid1.Style = MetroFramework.MetroColorStyle.Blue;
                if (table == "Restaurant") metroGrid1.Style = MetroFramework.MetroColorStyle.Green;
                if (table == "Club/Bar") metroGrid1.Style = MetroFramework.MetroColorStyle.Purple;
                if (table == "Other") metroGrid1.Style = MetroFramework.MetroColorStyle.Red;
                addC.nombreTabla = table;
                addC.ShowDialog();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        /// 
        /// ADD CATEGORY BUTTONS
        /// 
        private void metroButton5_Click(object sender, EventArgs e)
        {
            addCategory("Event");
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            addCategory("Restaurant");
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            addCategory("Club/Bar");
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            addCategory("Other");
        }

        ///
        /// ADD BUTTONS
        ///
        //ADD BUTTON FOR EVENTS
        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                add = new New();
                add.Style = MetroFramework.MetroColorStyle.Blue;
                add.nombreTabla = "Event";
                add.ShowDialog();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //ADD BUTTON FOR RESTAURANTS
        private void metroButton11_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                add2 = new New2();
                add2.Style = MetroFramework.MetroColorStyle.Green;
                add2.nombreTabla = "Restaurant";
                add2.ShowDialog();
                updateRestaurants();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //ADD BUTTON FOR BARS
        private void metroButton14_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                add2 = new New2();
                add2.Style = MetroFramework.MetroColorStyle.Purple;
                add2.nombreTabla = "Club/Bar";
                add2.ShowDialog();
                updateBars();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //ADD BUTTON FOR OTHERS
        private void metroButton17_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                add2 = new New2();
                add2.Style = MetroFramework.MetroColorStyle.Red;
                add2.nombreTabla = "Other";
                add2.ShowDialog();
                updateOthers();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        ///
        /// MODIFY BUTTONS
        /// 
        //MODIFY BUTTON FOR EVENTS
        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 2;
                search = new Search();
                search.nombreTabla = "Event";
                search.ShowDialog();
                updateEvents();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //MODIFY BUTTON FOR RESTAURANTS
        private void metroButton10_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 2;
                search2 = new Search2();
                search2.nombreTabla = "Restaurant";
                search2.ShowDialog();
                updateRestaurants();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //MODIFY BUTTON FOR BARS
        private void metroButton13_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 2;
                search2 = new Search2();
                search2.nombreTabla = "Club/Bar";
                search2.ShowDialog();
                updateBars();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //MODIFY BUTTON FOR OTHERS
        private void metroButton16_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 2;
                search2 = new Search2();
                search2.nombreTabla = "Other";
                search2.ShowDialog();
                updateOthers();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //DELETE BUTTON FOR EVENTS
        private void metroButton4_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 3;
                search = new Search();
                search.nombreTabla = "Event";
                search.ShowDialog();
                updateEvents();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //DELETE BUTTON FOR RESTAURANTS
        private void metroButton9_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 3;
                search2 = new Search2();
                search2.nombreTabla = "Restaurant";
                search2.ShowDialog();
                updateRestaurants();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //DELETE BUTTON FOR BARS
        private void metroButton12_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 3;
                search2 = new Search2();
                search2.nombreTabla = "Club/Bar";
                search2.ShowDialog();
                updateBars();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //DELETE BUTTON FOR OTHERS
        private void metroButton15_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                Properties.Settings.Default.TYPE = 3;
                search2 = new Search2();
                search2.nombreTabla = "Other";
                search2.ShowDialog();
                updateOthers();
            }
            else { MessageBox.Show("Can't connect to Database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //CONFIGURATION BUTTON
        private void metroButton22_Click(object sender, EventArgs e)
        {
                config = new Configuration();
                config.ShowDialog();
                if (Properties.Settings.Default.Connected)
                {
                    updateEvents();
                    updateRestaurants();
                    updateBars();
                    updateOthers();
                    connected = true;
                }

        }

        ///
        /// RIGHT CLICK MENU FOR EVENTS
        /// 
        private void metroGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            var r = metroGrid1.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && r.Type != DataGridViewHitTestType.None)
            {
                ContextMenu m = new ContextMenu();
                MenuItem opc1 = new MenuItem("Details");
                MenuItem opc2= new MenuItem("Modify");
                MenuItem opc3 = new MenuItem("Delete");
                m.MenuItems.Add(opc1);
                m.MenuItems.Add("-");
                m.MenuItems.Add(opc2);
                m.MenuItems.Add(opc3);
                metroGrid1.MultiSelect = false;
                
                metroGrid1.Rows[r.RowIndex].Selected = true;
                opc1.Click += new EventHandler(opc1_Click);
                opc2.Click += new EventHandler(opc2_Click);
                opc3.Click += new EventHandler(opc3_Click);
                m.Show(metroGrid1, new Point(e.X, e.Y));
            }
        }

        //CLICK ON DETAILS FOR EVENTS
        private void opc1_Click(object sender, EventArgs e)
        {
                Properties.Settings.Default.ID = metroGrid1.Rows[metroGrid1.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
                det = new Details();
                det.Style = MetroFramework.MetroColorStyle.Blue;
                det.nombreTabla = "Event";
                det.ShowDialog();
        }
        //CLICK ON MODIFY FOR EVENTS
        private void opc2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid1.Rows[metroGrid1.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            mod = new Modify();
            mod.Style = MetroFramework.MetroColorStyle.Blue;
            mod.nombreTabla = "Event";
            mod.ShowDialog();
            updateEvents();
        }
        //CLICK ON DELETE FOR EVENTS
        private void opc3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid1.Rows[metroGrid1.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString(); 
            del = new Delete();
            del.Style = MetroFramework.MetroColorStyle.Blue;
            del.nombreTabla = "Event";
            del.ShowDialog();
            updateEvents();
        }

        ///
        /// RIGHT CLICK MENU FOR RESTAURANTS
        /// 
        private void metroGrid2_MouseClick(object sender, MouseEventArgs e)
        {
            var r = metroGrid2.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && r.Type != DataGridViewHitTestType.None)
            {
                ContextMenu m = new ContextMenu();
                MenuItem opc1 = new MenuItem("Details");
                MenuItem opc2 = new MenuItem("Modify");
                MenuItem opc3 = new MenuItem("Delete");
                m.MenuItems.Add(opc1);
                m.MenuItems.Add("-");
                m.MenuItems.Add(opc2);
                m.MenuItems.Add(opc3);
                metroGrid2.MultiSelect = false;
                metroGrid2.Rows[r.RowIndex].Selected = true;
                opc1.Click += new EventHandler(opc11_Click);
                opc2.Click += new EventHandler(opc12_Click);
                opc3.Click += new EventHandler(opc13_Click);
                m.Show(metroGrid2, new Point(e.X, e.Y));
            }
        }

        //CLICK ON DETAILS FOR RESTAURANTS
        private void opc11_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid2.Rows[metroGrid2.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            det2 = new Details2();
            det2.Style = MetroFramework.MetroColorStyle.Blue;
            det2.nombreTabla = "Restaurant";
            det2.ShowDialog();
        }
        //CLICK ON MODIFY FOR RESTAURANTS
        private void opc12_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid2.Rows[metroGrid2.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            mod2 = new Modify2();
            mod2.Style = MetroFramework.MetroColorStyle.Green;
            mod2.nombreTabla = "Restaurant";
            mod2.ShowDialog();
            updateRestaurants();
        }
        //CLICK ON DELETE FOR RESTAURANTS
        private void opc13_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid2.Rows[metroGrid2.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            del2 = new Delete2();
            del2.Style = MetroFramework.MetroColorStyle.Purple;
            del2.nombreTabla = "Restaurant";
            del2.ShowDialog();
            updateRestaurants();
        }

        ///
        /// RIGHT CLICK MENU FOR BARS
        /// 
        private void metroGrid3_MouseClick(object sender, MouseEventArgs e)
        {
            var r = metroGrid3.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && r.Type != DataGridViewHitTestType.None)
            {
                ContextMenu m = new ContextMenu();
                MenuItem opc1 = new MenuItem("Details");
                MenuItem opc2 = new MenuItem("Modify");
                MenuItem opc3 = new MenuItem("Delete");
                m.MenuItems.Add(opc1);
                m.MenuItems.Add("-");
                m.MenuItems.Add(opc2);
                m.MenuItems.Add(opc3);
                metroGrid3.MultiSelect = false;
                metroGrid3.Rows[r.RowIndex].Selected = true;
                opc1.Click += new EventHandler(opc21_Click);
                opc2.Click += new EventHandler(opc22_Click);
                opc3.Click += new EventHandler(opc23_Click);
                m.Show(metroGrid3, new Point(e.X, e.Y));
            }
        }

        //CLICK ON DETAILS FOR BARS
        private void opc21_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid3.Rows[metroGrid3.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            det2 = new Details2();
            det2.Style = MetroFramework.MetroColorStyle.Blue;
            det2.nombreTabla = "Club/Bar";
            det2.ShowDialog();
        }
        //CLICK ON MODIFY FOR BARS
        private void opc22_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid3.Rows[metroGrid3.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            mod2 = new Modify2();
            mod2.Style = MetroFramework.MetroColorStyle.Purple;
            mod2.nombreTabla = "Club/Bar";
            mod2.ShowDialog();
            updateBars();
        }
        //CLICK ON DELETE FOR BARS
        private void opc23_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid3.Rows[metroGrid3.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            del2 = new Delete2();
            del2.Style = MetroFramework.MetroColorStyle.Purple;
            del2.nombreTabla = "Club/Bar";
            del2.ShowDialog();
            updateBars();
        }

        ///
        /// RIGHT CLICK MENU FOR BARS
        /// 
        private void metroGrid4_MouseClick(object sender, MouseEventArgs e)
        {
            var r = metroGrid4.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && r.Type != DataGridViewHitTestType.None)
            {
                ContextMenu m = new ContextMenu();
                MenuItem opc1 = new MenuItem("Details");
                MenuItem opc2 = new MenuItem("Modify");
                MenuItem opc3 = new MenuItem("Delete");
                m.MenuItems.Add(opc1);
                m.MenuItems.Add("-");
                m.MenuItems.Add(opc2);
                m.MenuItems.Add(opc3);
                metroGrid4.MultiSelect = false;
                metroGrid4.Rows[r.RowIndex].Selected = true;
                opc1.Click += new EventHandler(opc31_Click);
                opc2.Click += new EventHandler(opc32_Click);
                opc3.Click += new EventHandler(opc33_Click);
                m.Show(metroGrid4, new Point(e.X, e.Y));
            }
        }

        //CLICK ON DETAILS FOR OTHERS
        private void opc31_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid4.Rows[metroGrid4.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            det2 = new Details2();
            det2.Style = MetroFramework.MetroColorStyle.Blue;
            det2.nombreTabla = "Other";
            det2.ShowDialog();
        }
        //CLICK ON MODIFY FOR OTHERS
        private void opc32_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid4.Rows[metroGrid4.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            mod2 = new Modify2();
            mod2.Style = MetroFramework.MetroColorStyle.Red;
            mod2.nombreTabla = "Other";
            mod2.ShowDialog();
            updateOthers();
        }

        //CLICK ON DETAILS FOR OTHERS
        private void opc33_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID = metroGrid4.Rows[metroGrid4.Rows.GetFirstRow(DataGridViewElementStates.Selected)].Cells[0].Value.ToString();
            del2 = new Delete2();
            del2.Style = MetroFramework.MetroColorStyle.Red;
            del2.nombreTabla = "Other";
            del2.ShowDialog();
            updateOthers();
        }

        //BROWSER BUTTON
        private void metroButton18_Click(object sender, EventArgs e)
        {
            browser = new Browser();
            browser.webPage = website;
            browser.ShowDialog();
            
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            //Main Window resize
            metroTabControl1.Width = this.Width - 50;
            metroTabControl1.Height = this.Height - 140;
            metroButton1.Location = new Point(this.Width - 110 - 29, this.Height - 50);
            metroButton22.Location = new Point(29, this.Height - 50);
            metroButton18.Location = new Point(154, this.Height - 50);

            //Grid Resize
            metroGrid1.Width = metroTabControl1.Width - 40;
            metroGrid1.Height = metroTabControl1.Height - 110;
            metroGrid2.Width = metroTabControl1.Width - 40;
            metroGrid2.Height = metroTabControl1.Height - 110;
            metroGrid3.Width = metroTabControl1.Width - 40;
            metroGrid3.Height = metroTabControl1.Height - 110;
            metroGrid4.Width = metroTabControl1.Width - 40;
            metroGrid4.Height = metroTabControl1.Height - 110;

            //Event Part Resize
            metroButton5.Location = new Point(15, metroTabControl1.Height - 78);
            metroButton2.Location = new Point(metroTabPage1.Width - 100 - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton3.Location = new Point(metroTabPage1.Width - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton4.Location = new Point(metroTabPage1.Width - 100 - 15, metroTabControl1.Height - 78);

            //Restaurant Part Resize
            metroButton6.Location = new Point(15, metroTabControl1.Height - 78);
            metroButton11.Location = new Point(metroTabPage1.Width - 100 - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton10.Location = new Point(metroTabPage1.Width - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton9.Location = new Point(metroTabPage1.Width - 100 - 15, metroTabControl1.Height - 78);

            //Bar Part Resize
            metroButton7.Location = new Point(15, metroTabControl1.Height - 78);
            metroButton14.Location = new Point(metroTabPage1.Width - 100 - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton13.Location = new Point(metroTabPage1.Width - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton12.Location = new Point(metroTabPage1.Width - 100 - 15, metroTabControl1.Height - 78);

            //Other Part Resize
            metroButton8.Location = new Point(15, metroTabControl1.Height - 78);
            metroButton17.Location = new Point(metroTabPage1.Width - 100 - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton16.Location = new Point(metroTabPage1.Width - 100 - 100 - 15, metroTabControl1.Height - 78);
            metroButton15.Location = new Point(metroTabPage1.Width - 100 - 15, metroTabControl1.Height - 78);

            if (metroGrid1.Width > 1100)
            {
                metroGrid1.Columns[1].Width = this.Width - 1080;
            }
            else { metroGrid1.Columns[1].Width = 200; }
        }

    }
}
