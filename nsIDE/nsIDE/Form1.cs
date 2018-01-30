using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsIDE
{
    public partial class nsIDE : Form
    {
        static bool keyControl = false;
        static bool keyS = false;

        static string fileName = "Untitled";
        public nsIDE()
        {
            InitializeComponent();
            this.Text += " - " + fileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        private void Run_CLick(object sender, EventArgs e)
        {
            Run();
        }
        public void Run()
        {
            if (Properties.Settings.Default.Path != null)
            {
                if (fileName != "Untitled")
                {
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Properties.Settings.Default.Path,
                            Arguments = fileName,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    proc.Start();
                    while (!! proc.StandardOutput.EndOfStream)
                    {
                        string line = proc.StandardOutput.ReadLine();
                        Output.Text += line;
                    }
                }
                else
                {
                    SaveAs();
                }
            }
            else
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
            }
        }
        public void SaveAs()
        {
            string Contents = Input.Text;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "ns files (*.ns)|*.ns"; //txt files (*.txt)|*.txt|All files (*.*)|*.*
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {


                System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName.ToString());
                fileName = saveFileDialog1.FileName.ToString();
                using (StringReader reader = new StringReader(Contents))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            file.WriteLine(line);
                        }

                    } while (line != null);
                }
                file.Close();
                this.Text = "nsIDE - " + fileName;
            }

        }
        public void Save()
        {
            if (fileName != "Untitled") {
                string Contents = Input.Text;
                StreamWriter file = new StreamWriter(fileName);
                using (StringReader reader = new StringReader(Contents))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            file.WriteLine(line);
                        }

                    } while (line != null);
                }
                file.Close();
            }
            else
            {
                MessageBox.Show("This file Should be saved using Save As, look under File", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Open()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "ns files (*.ns)|*.ns";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName.ToString();
                Input.Text = File.ReadAllText(fileName);
            }
            this.Text = "nsIDE - " + fileName;
        }
        public void New()
        {
            Input.Text = "";
            fileName = "Untitled";
            this.Text = "nsIDE - " + fileName;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }



        private void nsIDE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
            {
                keyControl = true;
            }
            if (e.KeyCode == Keys.S)
            {
                keyS = true;
            }

            if (keyS && keyControl)
            {
                Save();
            }
        }

        private void nsIDE_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
            {
                keyControl = false;
            }
            if (e.KeyCode == Keys.S)
            {
                keyS = false;
            }
        }

        private void setNodeSCRIPTPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Settings.Default.Path);
        }
    }
}
