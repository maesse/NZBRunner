using System;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace NZBRunner
{
    
    class Program : Form
    {
        private TextBox textBox1;
        private FolderBrowserDialog folderBrowserDialog;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private Label label1;

        private static string Destination;
        private static bool AutoRename;

        [STAThread]
        static void Main(string[] args)
        {
            Destination = Properties.Settings.Default.DestinationFolder;
            AutoRename = Properties.Settings.Default.AutoRename;
            try
            {
                // Show GUI
                if (args.Length.Equals(0))
                {
                    Program program = new Program();
                    program.InitializeComponent();
                    program.Show();
                    Application.Run(program);
                }
                // Handle file
                else if (args.Length.Equals(1))
                {
                    if (Destination.Equals("") || Destination == null || !Directory.Exists(Destination))
                    {
                        // Problem with destination, starting gui..
                        Program program = new Program();
                        program.InitializeComponent();
                        program.Show();

                        MessageBox.Show(program, "Please fix Destination folder, then try again");
                        Application.Run(program);
                    }
                    else
                    {
                        string filename = Path.GetFileName(args[0]);
                        string finalDestination = Path.Combine(Destination, filename);

                        if (File.Exists(finalDestination))
                        {
                            if (AutoRename)
                            {
                                // Rename source/file.ext to source/file1231.ext
                                string temppath = Path.GetFileNameWithoutExtension(finalDestination);
                                string tempext = Path.GetExtension(finalDestination);
                                finalDestination = temppath + new Random().Next(0, 9999) + tempext;
                            }
                            else
                            {
                                // Error.. file exists
                                MessageBox.Show("Error: Destination file already exists: " + finalDestination + "\n\nTip: Turn on automatic renaming of files");
                                return;
                            }
                        }

                        System.Console.WriteLine("Source: {0}\n" +
                                                 "Destination: {1}", args[0], Destination);
                        File.Move(args[0], finalDestination);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("I think I'm going to puke!1111\n\n\n    ARRGGHHHH \n\n" + ex.ToString(), "NZBRunner error :(");
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(113, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(306, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Destination Folder:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(286, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Apply";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 37);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(250, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Rename files if they already exists at destination";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Program
            // 
            this.AcceptButton = this.button2;
            this.ClientSize = new System.Drawing.Size(393, 66);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Program";
            this.Text = "NZBRunner";
            this.Load += new System.EventHandler(this.Program_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Program_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.DestinationFolder;
            checkBox1.Checked = Properties.Settings.Default.AutoRename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }
        }

        // Apply
        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DestinationFolder = textBox1.Text;
            Properties.Settings.Default.AutoRename = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
