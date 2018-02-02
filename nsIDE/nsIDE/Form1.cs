using ScintillaNET;
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
using nodeSCRIPTResources;

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


            Input.StyleResetDefault();
            Input.Styles[Style.Default].BackColor = Color.White;
            Input.Styles[Style.Default].ForeColor = Color.Black;
            Input.Styles[Style.Default].Font = "consolas";
            Input.Styles[Style.Default].Size = 16;
            Input.ScrollWidth = 1;
            Input.ScrollWidthTracking = true;
            Input.StyleClearAll();
            Input.Styles[Style.LineNumber].BackColor = Color.White;
            Input.Styles[Style.LineNumber].ForeColor = Color.Black;
            Input.Styles[Style.LineNumber].Font = "consolas";





            Input.Lexer = ScintillaNET.Lexer.Cpp; // Doesn't matter what group it's in, cpp is empty


            //
            // Comment highlighting
            //
            Input.Margins[0].Width = 16;
            Input.Styles[Style.Cpp.CommentLine].Font = "Consolas";
            Input.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green

            //
            // string highlighting
            //
            Input.Styles[Style.Cpp.String].Font = "Consolas";
            Input.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(13, 14, 250); // Red
            Input.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            Input.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(21, 151, 26); // Green
            Input.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(21, 151, 26); // Green
            Input.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            Input.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            Input.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(7, 8, 148); // group 0
            Input.Styles[Style.Cpp.Word2].ForeColor = Color.DarkCyan;
            Input.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Input.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Input.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Input.Styles[Style.Cpp.StringEol].BackColor = Color.Red;
            Input.Styles[Style.Cpp.Operator].ForeColor = Color.Red;
            Input.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            Input.SetKeywords(0, "main node while for foreach var obj object variable if");
            Input.SetKeywords(1, "string int double char null");

            Input.SetProperty("fold", "1");
            Input.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            Input.Margins[2].Type = MarginType.Symbol;
            Input.Margins[2].Mask = Marker.MaskFolders;
            Input.Margins[2].Sensitive = true;
            Input.Margins[2].Width = 20;
            Input.SetFoldMarginColor(true, Color.White);
            Input.SetFoldMarginHighlightColor(true, Color.White);


            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                Input.Markers[i].SetForeColor(Color.Transparent);
                Input.Markers[i].SetBackColor(Color.FromArgb(189, 192, 198));
            }

            // Configure folding markers with respective symbols
            Input.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            Input.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            Input.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            Input.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            Input.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            Input.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            Input.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            Input.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            menuStrip1.BackColor = Color.White;
            menuStrip1.ForeColor = Color.Black;
            toolStrip1.BackColor = Color.White;
            toolStrip1.ForeColor = Color.Black;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Input.IndentWidth = 4;
            this.Input.TabWidth = 4;
            //menuStrip1.Renderer = new renderer();
        }

        /*
        private class renderer : ToolStripProfessionalRenderer
        {
            public renderer() : base(new cols()) { }
        }

        private class cols : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                // when the menu is selected
                get { return Color.FromArgb(60, 60, 60); }
            }

           

            public override Color MenuBorder  //added for changing the menu border
            {
                get { return Color.FromArgb(37, 37, 38); }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(37, 37, 38); }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(37, 37, 38); }
            }

            public override Color ToolStripDropDownBackground
            {
                get
                {
                    return Color.FromArgb(27, 27, 28);
                }
            }

        }
        */

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Text.Substring(this.Text.Length - 1) == "*")
            {
                DialogResult saveQuery = MessageBox.Show(fileName + " is has not been saved.\nDo you want to save it?", "Unsaved Work", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (saveQuery == DialogResult.Yes)
                {
                    e.Cancel = true;
                    Save();
                    this.Close();
                }
                else if (saveQuery == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }

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
                    /*
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "C:\\Users\\TorinPC\\source\\repos\\nodeSCRIPTProfessional\\nodeSCRIPTProfessional\\bin\\Release\\nodeSCRIPTProfessional.exe",
                            Arguments = "C:\\Users\\TorinPC\\source\\repos\\nodeSCRIPTProfessional\\nodeSCRIPTProfessional\\bin\\Release\\test.txt",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    int count = 0;
                    bool valid = true;
                    while (valid)
                    {
                        if (count == 0)
                        {
                            Console.WriteLine("PROCESS STARTED\n");

                            proc.Start();
                        }
                        string line = proc.StandardOutput.ReadLine();
                        Output.Text += line;
                        if (proc.StandardOutput.EndOfStream)
                        {
                            valid = false;
                        }
                        count += 1;
                    }
                    */

                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Users\TorinPC\source\repos\nodeSCRIPTProfessional\nodeSCRIPTProfessional\bin\Release\nodeSCRIPTProfessional.exe", @"C:\Users\TorinPC\source\repos\nodeSCRIPTProfessional\nodeSCRIPTProfessional\bin\Release\test.txt");
                    startInfo.WorkingDirectory = "C:\\Users\\TorinPC\\source\\repos\\nodeSCRIPTProfessional\\nodeSCRIPTProfessional\\bin\\Release\\";
                    startInfo.RedirectStandardOutput = true;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;

                    Process process = new Process();
                    process.StartInfo = startInfo;
                    process.EnableRaisingEvents = false;
                    process.Start();

                    StreamReader outputReader = process.StandardOutput;

                    Output.Text = outputReader.ReadToEnd();

                    process.WaitForExit();
                    process.Close();


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
                this.Text = "nsIDE - " + fileName;
            }
            else
            {
                SaveAs();
            }
        }
        private void Open()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "NS File (*.ns)|*.ns";
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

        private int maxLineNumberCharLength;
        private void Input_TextChanged(object sender, EventArgs e)
        {
            this.Text = "nsIDE - " + fileName + "*";
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = Input.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            Input.Margins[0].Width = Input.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;

        }

        private void Input_CharAdded(object sender, CharAddedEventArgs e)
        {
            
            // Find the word start
            var currentPos = Input.CurrentPosition;
            var wordStartPos = Input.WordStartPosition(currentPos, true);

            // Display the autocompletion list
            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0)
            {
                if (!Input.AutoCActive)
                    Input.AutoCShow(lenEntered, "main node var obj if while for foreach");
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Input_Click(object sender, EventArgs e)
        {

        }

        private void Output_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
