using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextIdentification
{
    public partial class Training : Form
    {
        List<string> wordgrid = new List<string>();
        int charatercount = 0;
        public Training()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectLang lang = new SelectLang();
            lang.ShowDialog();

           

            if (SelectLang.selectlang != "")
            {
                try
                {
                    dataGridView1.Rows.Clear();
                    dataGridView2.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    dataGridView4.Rows.Clear();
                    dataGridView5.Rows.Clear();
                    dataGridView6.Rows.Clear();

                    OpenFileDialog op = new OpenFileDialog();
                    op.ShowDialog();
                    StreamReader rd = new StreamReader(op.FileName);
                    richTextBox1.Text = rd.ReadToEnd().ToLower().Trim().Replace("~", "").Replace(",", "");
                    rd.Close();


                    StreamReader rr = new StreamReader(Application.StartupPath +"\\stopwords.txt");
                    string []stopword = rr.ReadToEnd().Split ('\n');

                    for (int xx = 0; xx < stopword.Length; xx++)
                    {
                        richTextBox1.Text = richTextBox1.Text.Replace(stopword [xx].Trim (),"");
                    }


                    ObjSerial serial = new ObjSerial();
                    Stream stream = File.Open(Application.StartupPath + "\\Serial\\" + SelectLang.selectlang + ".serial", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    serial = (ObjSerial)bformatter.Deserialize(stream);
                    stream.Close();

                    string val = serial.TextData;

                    rd = new StreamReader(Application .StartupPath +"\\Model\\"+SelectLang.selectlang +".model");
                    string []str = rd.ReadToEnd().Trim().Split('~'); //val;
                    rd.Close();

                    string []characterset = str[0].Trim ().Split('\n') ;
                    string []wordlist = str[1].Trim ().Split('\n');
                    string []ngramlist = str[2].Trim ().Split('\n');

                    dataGridView4.Rows.Clear();
                    dataGridView5.Rows.Clear();
                    dataGridView6.Rows.Clear();

                    for (int x = 0; x<characterset.Length; x++)
                    {
                        string[] data = characterset[x].Split(',');
                        dataGridView4.Rows.Add();

                        dataGridView4.Rows[x].Cells[0].Value = data[0];
                        dataGridView4.Rows[x].Cells[1].Value = data[1];
                    }

                    for (int x = 0; x < wordlist.Length; x++)
                    {
                        string[] data = wordlist[x].Split(',');
                        dataGridView6.Rows.Add();

                       

                        dataGridView6.Rows[x].Cells[0].Value = data[0];
                        dataGridView6.Rows[x].Cells[1].Value = data[1];
                    }

                    for (int x = 0; x < ngramlist.Length; x++)
                    {
                        string[] data = ngramlist[x].Split(',');
                        dataGridView5.Rows.Add();

                        dataGridView5.Rows[x].Cells[0].Value = data[0];
                        dataGridView5.Rows[x].Cells[1].Value = data[1];
                    }
                }
                catch
                {
                }
            }
        }

        private void characterSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                charatercount = 0;
                dataGridView1.Rows.Clear();
                int row =0;
                List<string> tokens = new List<string>();

                for (int x = 0; x < richTextBox1.Text.Length; x++)
                {
                    string ss = richTextBox1.Text[x].ToString().Trim();
                    if (richTextBox1.Text[x].ToString().Trim() != "" && tokens.Contains(ss)==false)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = richTextBox1.Text[x].ToString();
                        string wd = richTextBox1.Text[x].ToString();
                        dataGridView1.Rows[row].Cells[1].Value = TextTool.CountStringOccurrences(richTextBox1.Text, wd);
                        tokens.Add(ss);
                        row++;
                    }
                   
                }
                dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
                charatercount = dataGridView1.Rows.Count - 1;

            }
            catch
            {
            }
        }

        private void wordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                string[] character = richTextBox1.Text.Split();
                List<string> tokens = new List<string>();

                int row=0;
                for (int x = 0; x < character.Length; x++)
                {
                    string ss = character[x].ToString().Trim();
                    if (character[x].ToString().Trim() != "" && tokens.Contains(ss) == false)
                    {
                        dataGridView2.Rows.Add();
                       // destemmer stream = new destemmer();

                       // EnglishPorter2Stemmer stremmer = new EnglishPorter2Stemmer();

                        dataGridView2.Rows[row].Cells[0].Value =(character[x].ToString());
                        string wd = character[x].ToString();
                        dataGridView2.Rows[row].Cells[1].Value = TextTool.CountStringOccurrences(richTextBox1.Text, wd);
                        tokens.Add(ss);
                        row++;
                    }

                }
                dataGridView2.Sort(dataGridView2.Columns[1], ListSortDirection.Descending);
            }
            catch
            {
            }
        }

        private void nGramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowcont = 0;
            List<string> tokens = new List<string>();
            dataGridView3.Rows.Clear();
            try
            {
               
                for (int x = 0; x < richTextBox1.Text.Length; x++)
                {
                    if (tokens.Contains(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString()) == false && (richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString()).Trim ().Length >1)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[x].Cells[0].Value = richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString();
                        string wd = richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString();
                        dataGridView3.Rows[x].Cells[1].Value = TextTool.CountStringOccurrences(richTextBox1.Text, wd);
                        tokens.Add(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString());
                        rowcont++;
                    }
                }

            }
            catch 
            {
            }
            try 
            {
                for (int x = 0; x < richTextBox1.Text.Length; x++)
                {
                    if (tokens.Contains(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString()) == false && (richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString()).Trim ().Length >2)
                    {

                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[rowcont].Cells[0].Value = richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString();
                        string wd = richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString();
                        dataGridView3.Rows[rowcont].Cells[1].Value = TextTool.CountStringOccurrences(richTextBox1.Text, wd);
                        tokens.Add(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString());
                        rowcont++;
                    }
                }

               

            }
            catch
            {
            }
            dataGridView3.Sort(dataGridView3.Columns[1], ListSortDirection.Descending);
        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrainingData();

            ObjSerial serial = new ObjSerial();
            serial.TextData = "";




            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\model\\" +SelectLang.selectlang+ ".model");

            int count =0;

            for (int x = 0; x < dataGridView4.Rows.Count - 1; x++)
            {
                try
                {
                    sw.WriteLine(dataGridView4.Rows[x].Cells[0].Value.ToString() + "," + dataGridView4.Rows[x].Cells[1].Value.ToString());
                    serial.TextData = serial.TextData + dataGridView4.Rows[x].Cells[0].Value.ToString() + "," + dataGridView4.Rows[x].Cells[1].Value.ToString() + "\n";

                }
                catch
                {
                }
            }
            sw.WriteLine("~");
            serial.TextData = serial.TextData + "~";

            for (int x = 0; x < dataGridView6.Rows.Count - 1; x++)
            {
                try
                {
                    if (count < 400)
                    {
                        sw.WriteLine(dataGridView6.Rows[x].Cells[0].Value.ToString() + "," + dataGridView6.Rows[x].Cells[1].Value.ToString());
                        serial.TextData = serial.TextData + dataGridView6.Rows[x].Cells[0].Value.ToString() + "," + dataGridView6.Rows[x].Cells[1].Value.ToString() + "\n";
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                }

            }
            count =0;
            sw.WriteLine("~");
            for (int x = 0; x < dataGridView5.Rows.Count - 1; x++)
            {
                try
                {
                    if (count < 400)
                    {
                        sw.WriteLine(dataGridView5.Rows[x].Cells[0].Value.ToString() + "," + dataGridView5.Rows[x].Cells[1].Value.ToString());
                        serial.TextData = serial.TextData + dataGridView5.Rows[x].Cells[0].Value.ToString() + "," + dataGridView5.Rows[x].Cells[1].Value.ToString() + "\n";
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                { }
            }
            sw.Close();


            Stream stream = File.Open(Application.StartupPath + "\\Serial\\" + SelectLang.selectlang + ".serial", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, serial);
            stream.Close();


            MessageBox.Show("Done");

        }

        public void TrainingData()
        {

            wordgrid = new List<string>();

            for (int x = 0; x < dataGridView6.Rows.Count-1; x++)
            {

                wordgrid.Add(dataGridView6.Rows[x].Cells[0].Value.ToString().Trim ());
                
            }


            int count = 0;
            int rowcount = (dataGridView6.Rows.Count-1);

            for (int x = 0; x < dataGridView2.Rows.Count-1; x++)
            {

                if (wordgrid.Contains(dataGridView2.Rows[x].Cells[0].Value.ToString().Trim ()) == false)
                {
                    dataGridView6.Rows.Add();
                    dataGridView6.Rows[count + rowcount].Cells[0].Value = dataGridView2.Rows[x].Cells[0].Value.ToString ();
                    dataGridView6.Rows[count + rowcount].Cells[1].Value = dataGridView2.Rows[x].Cells[1].Value.ToString ();
                    count++;
                }

                else
                {
                    int index = wordgrid.IndexOf(dataGridView2.Rows[x].Cells[0].Value.ToString().Trim());
                    int value = int.Parse(dataGridView6.Rows[x].Cells[1].Value.ToString().Trim());
                    int newvalue = int.Parse(dataGridView2.Rows[x].Cells[1].Value.ToString());

                    dataGridView6.Rows[index].Cells[1].Value = value+newvalue ;

                }
            }


            wordgrid = new List<string>();

            for (int x = 0; x < dataGridView5.Rows.Count - 1; x++)
            {

                wordgrid.Add(dataGridView5.Rows[x].Cells[0].Value.ToString().Trim());

            }


             count = 0;
             rowcount = (dataGridView5.Rows.Count - 1);

            for (int x = 0; x < dataGridView3.Rows.Count - 1; x++)
            {
                try
                {
                    if (wordgrid.Contains(dataGridView3.Rows[x].Cells[0].Value.ToString().Trim()) == false)
                    {
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[count + rowcount].Cells[0].Value = dataGridView3.Rows[x].Cells[0].Value.ToString();
                        dataGridView5.Rows[count + rowcount].Cells[1].Value = dataGridView3.Rows[x].Cells[1].Value.ToString();
                        count++;
                    }

                    else
                    {
                        int index = wordgrid.IndexOf(dataGridView3.Rows[x].Cells[0].Value.ToString().Trim());
                        int value = int.Parse(dataGridView5.Rows[x].Cells[1].Value.ToString().Trim());
                        int newvalue = int.Parse(dataGridView3.Rows[x].Cells[1].Value.ToString());

                        dataGridView5.Rows[index].Cells[1].Value = value + newvalue;

                    }









                }
                catch
                {
                }
            }


            wordgrid = new List<string>();

            for (int x = 0; x < dataGridView4.Rows.Count - 1; x++)
            {

                wordgrid.Add(dataGridView4.Rows[x].Cells[0].Value.ToString().Trim());

            }


            count = 0;
            rowcount = (dataGridView4.Rows.Count - 1);

            for (int x = 0; x < dataGridView1.Rows.Count - 1; x++)
            {
                try
                {
                    if (wordgrid.Contains(dataGridView1.Rows[x].Cells[0].Value.ToString().Trim()) == false)
                    {
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[count + rowcount].Cells[0].Value = dataGridView1.Rows[x].Cells[0].Value.ToString();
                        dataGridView4.Rows[count + rowcount].Cells[1].Value = dataGridView1.Rows[x].Cells[1].Value.ToString();
                        count++;
                    }

                    else
                    {
                        int index = wordgrid.IndexOf(dataGridView1.Rows[x].Cells[0].Value.ToString().Trim());
                        int value = int.Parse(dataGridView4.Rows[x].Cells[1].Value.ToString().Trim());
                        int newvalue = int.Parse(dataGridView1.Rows[x].Cells[1].Value.ToString());

                        dataGridView4.Rows[index].Cells[1].Value = value + newvalue;

                    }


                }
                catch
                {
                }
            }




        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
