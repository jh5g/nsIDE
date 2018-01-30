namespace nsIDE
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.DisplayPath = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // DisplayPath
            // 
            this.DisplayPath.Enabled = false;
            this.DisplayPath.Location = new System.Drawing.Point(47, 26);
            this.DisplayPath.Name = "DisplayPath";
            this.DisplayPath.Size = new System.Drawing.Size(529, 26);
            this.DisplayPath.TabIndex = 0;
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(601, 22);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(84, 35);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(571, 156);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(114, 48);
            this.Ok.TabIndex = 2;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 226);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.DisplayPath);
            this.Name = "Form2";
            this.Text = "nodeSCRIPT Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox DisplayPath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Ok;
    }
}