using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TextIdentification
{
    public partial class SelectLang : Form
    {
        public static string selectlang = "";

        public int Train { get; private set; }

        public SelectLang()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectlang = comboBox1.SelectedItem.ToString();
            this.Close();
        }

        private void SelectLang_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            StreamReader rd = new StreamReader(Application.StartupPath + "\\Setting.txt");
            string[] data = rd.ReadToEnd().Trim().Split('\n');

            rd.Close();

            for (int x = 0; x < data.Length; x++)
            {
                comboBox1.Items.Add(data[x].Trim());
            }

        }

        private void NewMethod(string[] data)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
