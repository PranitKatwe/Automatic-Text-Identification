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
    public partial class Testing : Form
    {
        List<string> wordgrid = new List<string>();

        List<string> worddata = new List<string>();

        List<string> ngramdata = new List<string>();

        int charcount = 0;
        public Testing()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            charcount = 0;
         //   if (SelectLang.selectlang != "")
            {
                try
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.ShowDialog();
                    StreamReader rd = new StreamReader(op.FileName);
                    richTextBox1.Text = rd.ReadToEnd().Trim().Replace("~", "").Replace(",", "").ToLower();
                    rd.Close();

                    StreamReader rr = new StreamReader(Application.StartupPath + "\\stopword.txt");
                    string[] stopword = rr.ReadToEnd().Split('\n');

                    for (int xx = 0; xx < stopword.Length; xx++)
                    {
                        richTextBox1.Text = richTextBox1.Text.Replace(stopword[xx], "");
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
                charcount = dataGridView1.Rows.Count - 1;
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
                        //EnglishPorter2Stemmer stremmer = new EnglishPorter2Stemmer();
                        dataGridView2.Rows[row].Cells[0].Value = character[x].ToString();
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
                    if (tokens.Contains(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString()) == false && (richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString()).Trim().Length > 1)
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
                    if (tokens.Contains(richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString()) == false && (richTextBox1.Text[x].ToString() + richTextBox1.Text[x + 1].ToString() + richTextBox1.Text[x + 2].ToString()).Trim().Length > 2)
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

            StreamReader rd = new StreamReader(Application.StartupPath +"\\setting.txt");
            string data = rd.ReadToEnd().Trim();

            string[] lang = data.Split('\n');

            DataTable table = new DataTable();

            int cnt = 0;
            int cnt1 = 0;
            //int cnt2 = 0;
           

            table.Columns.Add("Charater");
            table.Columns.Add("Count", typeof(double));
           
           
            //training data.

            string[] files = Directory.GetFiles(Application.StartupPath + "\\Model\\");
            string[] files1 = Directory.GetFiles(Application.StartupPath + "\\Serial\\");
            List<double> moddistance = new List<double>();
            List<double> moddistance1 = new List<double>();

            for (int x = 0; x < files.Length; x++)
            {


                ObjSerial serial = new ObjSerial();
                Stream stream = File.Open(files1[x], FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                serial = (ObjSerial)bformatter.Deserialize(stream);
                stream.Close();

                string val = serial.TextData;

            rd = new StreamReader(files[x]);
            string[] str = rd.ReadToEnd().Trim().Split('~');
            rd.Close();

                string[] characterset = str[0].Trim().Split('\n');
                string[] data1 = characterset[x].Split(',');
                table.Rows.Add(Path.GetFileNameWithoutExtension(files[x]), double.Parse(characterset.Length.ToString ()));
                table.Rows.Add(Path.GetFileNameWithoutExtension(files[x]), double.Parse(characterset.Length.ToString())-1);
                table.Rows.Add(Path.GetFileNameWithoutExtension(files[x]), double.Parse(characterset.Length.ToString())-2);
                table.Rows.Add(Path.GetFileNameWithoutExtension(files[x]), double.Parse(characterset.Length.ToString())-3);
                table.Rows.Add(Path.GetFileNameWithoutExtension(files[x]), double.Parse(characterset.Length.ToString())-4);

                string[] wordlist = str[1].Trim().Split('\n');

                worddata.Clear();
                List<int> wordcount = new List<int>();
              
                for (int y = 0; y < wordlist.Length; y++)
                {
                    string[] data12 = wordlist[y].Split(',');
                    
                    worddata.Add(data12[0]);
                    wordcount.Add(int.Parse (data12[1]));

                    cnt++;
                    if (cnt >= 400)
                    {
                        break;
                    }
                }
                double distance = 0;
                cnt = 0;
                for (int y = 0; y < dataGridView2.Rows.Count - 1; y++)
                {
                    try
                    {
                        string getvalue = dataGridView2.Rows[y].Cells[0].Value.ToString();
                        int getcount = int.Parse(dataGridView2.Rows[y].Cells[1].Value.ToString());
                        int indexof = worddata.IndexOf(dataGridView2.Rows[y].Cells[0].Value.ToString());

                     //   string getvalue1 = dataGridView6.Rows[y].Cells[0].Value.ToString();
                        int getcount1 = wordcount[indexof];

                        int x0 = getcount;
                        int y0 = getcount;

                        int x1 = getcount1;
                        int y1 = getcount1;

                        int dX = x1 - x0;
                        int dY = y1 - y0;
                        distance = Math.Sqrt(dX * dX + dY * dY) + distance;
                        cnt++;
                        if (cnt >= 400)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        distance = distance + 100;
                    }
                    
                    
                }
                moddistance.Add(distance / dataGridView2.Rows.Count);

                string[] ngramlist = str[2].Trim().Split('\n');
                ngramdata.Clear();
                List<int> ngramcount = new List<int>();

                for (int y = 0; y < ngramlist.Length; y++)
                {
                    string[] data12 = ngramlist[y].Split(',');

                    ngramdata.Add(data12[0]);
                    ngramcount.Add(int.Parse(data12[1]));

                    cnt1++;
                    if (cnt1 >= 400)
                    {
                        break;
                    }
                }
                double distance1 = 0;
                cnt1 = 0;
                for (int y = 0; y < dataGridView3.Rows.Count - 1; y++)
                {
                    try
                    {
                        string getvalue = dataGridView3.Rows[y].Cells[0].Value.ToString();
                        int getcount = int.Parse(dataGridView3.Rows[y].Cells[1].Value.ToString());
                        int indexof = ngramdata.IndexOf(dataGridView3.Rows[y].Cells[0].Value.ToString());

                      //  string getvalue1 = dataGridView6.Rows[y].Cells[0].Value.ToString();
                        int getcount1 = ngramcount[indexof];

                        int x0 = getcount;
                        int y0 = getcount;

                        int x1 = getcount1;
                        int y1 = getcount1;

                        int dX = x1 - x0;
                        int dY = y1 - y0;
                        distance1 = Math.Sqrt(dX * dX + dY * dY) + distance1;
                        cnt1++;
                        if (cnt1 >= 400)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        distance1 = distance1 + 100;
                    }


                }
                moddistance1.Add(distance1 / dataGridView3.Rows.Count);
            }

            double mindistance = moddistance.Min();
            string result = Path.GetFileNameWithoutExtension(files[moddistance.IndexOf (mindistance)]);

            double mindistance1 = moddistance1.Min();
            string result1 = Path.GetFileNameWithoutExtension(files[moddistance1.IndexOf(mindistance1)]);

            Classifier classifier = new Classifier();
            classifier.TrainClassifier(table);

            string finalresult = classifier.Classify(new double[] { charcount });

            if (finalresult == result || result== result1)
            {
                //MessageBox.Show("Language Detected :" + result + " Distance Value="+Classifier.distance);
                MessageBox.Show("Language Detected :" + result);
            }
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

        private void Testing_Load(object sender, EventArgs e)
        {

        }

        private void Testing_Load_1(object sender, EventArgs e)
        {

        }
    }
}
