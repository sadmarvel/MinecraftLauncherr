using MySql.Data.MySqlClient;
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
using BCryptNet = BCrypt.Net.BCrypt;
using System.Configuration;

namespace WindowsFormsApp15
{
    public partial class Form2 : Form
    {

        private MySqlConnection connection;
        private MySqlCommand command;
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["isLoggedIn"] = false;
            Properties.Settings.Default.Save(); 
           
            Form3 form3 = new Form3();
            form3.Show();
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                // Kullanıcı zaten giriş yapmış, giriş yapma işlemini atla
                return;
            }

            // Get input from text boxes
            string inputUsername = textBox1.Text;
            string password = textBox2.Text;

            // Validate input
            if (string.IsNullOrEmpty(inputUsername) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                // Create connection to database
                connection = new MySqlConnection("server=00000;database=loucraft_users;user=loucraft_admin;password=0000");
                connection.Open();

                // Create command to select data from database
                command = new MySqlCommand("SELECT password FROM users WHERE username = @username", connection);

                // Add parameter with explicit data type
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = inputUsername;

                // Execute command
                string hashedPassword = command.ExecuteScalar().ToString();

                // Close connection
                connection.Close();

                // Verify password
                if (BCryptNet.Verify(password, hashedPassword))
                {
                    // Login successful
                   
                    MessageBox.Show("Giriş Başarılı!");
                    Properties.Settings.Default["isLoggedIn"] = true;
                    Properties.Settings.Default["Username"] = inputUsername;
                    Properties.Settings.Default.Save();

                    Form1 mainForm = new Form1(inputUsername); // Pass the input username to Form1

                    // Show the main form
                    mainForm.ShowDialog();

                    // Close the login form
                    this.Close();
                }
                else
                {
                    // Login failed
                    MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error logging in: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            bool isLoggedIn = (bool)Properties.Settings.Default["isLoggedIn"];

            if (isLoggedIn)
            {
                // Kullanıcı zaten giriş yapmış, ana forma yönlendir
                string username = Properties.Settings.Default["Username"].ToString();

                Form1 mainForm = new Form1(username);

                mainForm.ShowDialog();
                this.Close();

                // Radyo butonun açılmasını sağlayalım
              
            }
        }
    }
}