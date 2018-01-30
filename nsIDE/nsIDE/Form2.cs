using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsIDE
{
    public partial class Form2 : Form
    {
        public string fileName { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                this.fileName = openFileDialog1.FileName;
                DisplayPath.Text = fileName;
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Path = this.fileName;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
