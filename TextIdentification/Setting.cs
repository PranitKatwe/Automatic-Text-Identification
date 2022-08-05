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
    public partial class Setting : Form
    {
        List<string> lang = new List<string>();
        int rowcount = 0;
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            lang = new List<string>();
            StreamReader rd = new StreamReader(Application.StartupPath +"\\Setting.txt");
            string []data = rd.ReadToEnd().Trim ().Split ('\n');

            rd.Close();

            dataGridView1.Rows.Clear();

            for (int x = 0; x < data.Length; x++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[x].Cells[0].Value = x + 1;
                dataGridView1.Rows[x].Cells[1].Value = data[x];
                lang.Add(data [x].ToLower ());
                rowcount++;
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lang.Contains(textBox1.Text.Trim().ToLower()) == false && textBox1.Text.Trim()!="")
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[rowcount].Cells[0].Value = rowcount + 1;
                dataGridView1.Rows[rowcount ].Cells[1].Value = textBox1.Text.Trim();

                rowcount++;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\Setting.txt");

            for (int x = 0; x < dataGridView1.Rows.Count - 1; x++)
            {
                sw.WriteLine(dataGridView1.Rows[x].Cells [1].Value .ToString ());
            }
            MessageBox.Show("Done");
            sw.Close();

            lang = new List<string>();
            StreamReader rd = new StreamReader(Application.StartupPath + "\\Setting.txt");
            string[] data = rd.ReadToEnd().Trim ().Split('\n');

            rd.Close();

            dataGridView1.Rows.Clear();

            for (int x = 0; x < data.Length; x++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[x].Cells[0].Value = x + 1;
                dataGridView1.Rows[x].Cells[1].Value = data[x];
                lang.Add(data[x].ToLower());
                rowcount++;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
