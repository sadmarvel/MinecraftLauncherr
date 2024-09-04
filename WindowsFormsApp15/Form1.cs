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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static WindowsFormsApp15.Form2;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
  





        public Form1(string username)
        {
            InitializeComponent();
            label5.Text = username;

        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the file explorer
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Display the image in the picture box
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Save the image
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the image
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Exit the application
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        //...
        private void RunBatchScript(string ramAmount)
        {
            string batchScriptPath = Path.Combine(Application.StartupPath, "client", "Baslat.bat");
            bool fileExists = File.Exists(batchScriptPath);
            if (!fileExists)
            {
                MessageBox.Show("File not found: " + batchScriptPath);
                return;
            }

            string username = Properties.Settings.Default["Username"].ToString();

            Process process = new Process();
            process.StartInfo.FileName = batchScriptPath;
            process.StartInfo.Arguments = $"{ramAmount} {username}";
            process.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "client");
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
            process.Start();
            process.BeginOutputReadLine();

            MessageBox.Show("Başarıyla Uygulandı.");

            // Wait for the process to finish
            process.WaitForExit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            comboBox1.Items.Add("Ayarlar");
            comboBox1.Items.Add("Çıkış Yap");

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string selectedRam = Properties.Settings.Default["SelectedRam"] as string;
            if (string.IsNullOrEmpty(selectedRam))
            {
                MessageBox.Show("İlk Önce Ram Seçiniz!");
                Form4 ramForm = new Form4();
                ramForm.Show();
            }
            else
            {
                RunBatchScript(selectedRam);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Ayarlar")
            {
                Form4 ayarlarForm = new Form4();
                ayarlarForm.ShowDialog();
            }
            else if (comboBox1.SelectedItem.ToString() == "Çıkış Yap")
            {
                Form2 girisForm = new Form2();

                // Update the properties settings
                Properties.Settings.Default["isLoggedIn"] = false;
                Properties.Settings.Default["Username"] = string.Empty;
                Properties.Settings.Default.Save();


                girisForm.ShowDialog();
                this.Close();

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}