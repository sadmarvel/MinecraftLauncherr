using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using MySql.Data.MySqlClient;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.IO;


namespace WindowsFormsApp15
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get input from text boxes
            string username = textBox1.Text;
            string oldPassword = textBox2.Text;
            string newPassword = textBox3.Text;
            string confirmPassword = textBox4.Text;

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            try
            {
                // Create connection to database
                MySqlConnection connection = new MySqlConnection("server=0000;database=loucraft_users;user=loucraft_admin;password=000");
                connection.Open();

                // Create command to select data from database
                MySqlCommand command = new MySqlCommand("SELECT email FROM users WHERE username = @username", connection);

                // Add parameter with explicit data type
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

                // Execute command
                string email = command.ExecuteScalar().ToString();

                // Generate a random password reset code
                string resetCode = GenerateResetCode();

                // Send the password reset code to the user's email
                SendResetCodeEmail(email, resetCode);

                // Store the reset code in the database
                command = new MySqlCommand("UPDATE users SET reset_code = @resetCode WHERE username = @username", connection);
                command.Parameters.Add("@resetCode", MySqlDbType.VarChar).Value = resetCode;
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                command.ExecuteNonQuery();

                MessageBox.Show("Şifre Sıfırlama Kodu Mailinize Gönderildi!");
                button2.Visible = true;
                textBox5.Visible = true;
           
                // Close connection
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error resetting password: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private string GenerateResetCode()
        {
            // Generate a random 6-digit code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendResetCodeEmail(string email, string resetCode)
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("account@loucraft.net");
            message.To.Add(email);
            message.Subject = "Password Reset Code";
            message.Body = "Your password reset code is: " + resetCode;

            // Create a new SMTP client
            SmtpClient client = new SmtpClient();
            client.Host = "cp.hostimux.com"; 
            client.Port =587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("account@loucraft.net", "gXULG2mPUsqnHgD");

            // Send the email
            client.Send(message);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            Form4 form4 = new Form4();

            form4.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string _selectedRam;

        public string SelectedRam
        {
            get { return _selectedRam; }
            set { _selectedRam = value; }
        }
        private void Form4_Load(object sender, EventArgs e)
        {
      
            button2.Visible = false;
            textBox5.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void Form4_Click(object sender, EventArgs e)
        {
    
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            // Get the reset code from textBox5
            string resetCode = textBox5.Text;

            // Get the username from textBox1
            string username = textBox1.Text;

            try
            {
                // Create connection to database
                MySqlConnection connection = new MySqlConnection("server=185.17.139.227;database=loucraft_users;user=loucraft_admin;password=ksNn6y3bw7ok;");
                connection.Open();

                // Create command to select data from database
                MySqlCommand command = new MySqlCommand("SELECT reset_code FROM users WHERE username = @username", connection);

                // Add parameter with explicit data type
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

                // Execute command
                string storedResetCode = command.ExecuteScalar().ToString();

                // Check if the reset code matches
                if (resetCode == storedResetCode)
                {
                    // Get the new password from textBox3
                    string newPassword = textBox3.Text;

                    // Hash the new password
                    string hashedPassword = BCryptNet.HashPassword(newPassword);

                    // Update the user's password
                    command = new MySqlCommand("UPDATE users SET password = @newPassword WHERE username = @username", connection);
                    command.Parameters.Add("@newPassword", MySqlDbType.VarChar).Value = hashedPassword;
                    command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                    command.ExecuteNonQuery();

                    MessageBox.Show("Şifreniz başarıyla sıfırlandı!");
                    Form2 form2 = new Form2();
                    form2.Show();
                    Form4 form4 = new Form4();
                    form4.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Yanlış Doğrulama Kodu! Lütfen Tekrar Deneyiniz.");
                }

                // Close connection
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error resetting password: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            bool IsLoggedIn = (bool)Properties.Settings.Default["isLoggedIn"];
            if (IsLoggedIn)
            {
                groupBox1.Visible = false;
                groupBox2.Visible = true;
            }
            else
            {
                MessageBox.Show("Lütfen İlk Giriş Yapınız!");
                radioButton2.Checked = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            Form4 form4 = new Form4();

            form4.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Get the selected RAM amount from the RadioButton group
            string ramAmount = "";
            if (radioButton1.Checked)
            {
                ramAmount = "512MB";
            }
            else if (radioButton2.Checked)
            {
                ramAmount = "1GB";
            }
            else if (radioButton3.Checked)
            {
                ramAmount = "2GB";
            }
            else if (radioButton4.Checked)
            {
                ramAmount = "3GB";
            }
            else if (radioButton5.Checked)
            {
                ramAmount = "4GB";
            }

            Properties.Settings.Default["SelectedRam"] = ramAmount;
            Properties.Settings.Default.Save();

            // Run the batch script with the RAM amount as an argument
            RunBatchScript(ramAmount);
        }
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
            process.StartInfo.CreateNoWindow = true; // Add this line to hide the console window
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Add this line to hide the console window

            process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
            process.Start();
            process.BeginOutputReadLine();

            MessageBox.Show("Başarıyla Uygulandı. Lütfen Bekleyiniz!");

            // Wait for the process to finish
            process.WaitForExit();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                RunBatchScript("512");
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                RunBatchScript("1024");
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                RunBatchScript("2048");
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                RunBatchScript("3072");
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                RunBatchScript("4096");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}