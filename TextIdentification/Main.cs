using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextIdentification
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void trainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Training train = new Training();
            train.TopLevel = false;
            train.Dock = DockStyle.Fill;
            train.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            panel1.Controls.Clear();
            panel1.Controls.Add(train);
            train.Show();

            
        }

        private void identificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Testing train = new Testing();
            train.TopLevel = false;
            train.Dock = DockStyle.Fill;
            train.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            panel1.Controls.Clear();
            panel1.Controls.Add(train);
            train.Show();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }
    }
}
