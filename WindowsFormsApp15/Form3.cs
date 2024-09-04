using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;



namespace WindowsFormsApp15
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private MySqlConnection connection;
        private MySqlCommand command;
        private void label3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            Form3 form3 = new Form3();

            form3.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Get input from text boxes
            string username = textBox1.Text;
            string password = textBox3.Text;
            string email = textBox2.Text;

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Lütfen Tüm Kutucukları Doldurun.");
                return;
            }

            if (username.Length > 16)
            {
                MessageBox.Show("Kullanıcı adı maksimum 16 karakter olmalıdır.");
                return;
            }

            // Hash password
            string hashedPassword = BCryptNet.HashPassword(password, 12);

            try
            {
                // Create connection to database
                connection = new MySqlConnection("server=0000;database=loucraft_users;user=loucraft_admin;password=0000");
                connection.Open();

                // Create command to insert data into database
                command = new MySqlCommand("INSERT INTO users (username, password, email) VALUES (@username, @password, @email)", connection);

                // Add parameters with explicit data types
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                // Execute command
                command.ExecuteNonQuery();

                // Close connection
                connection.Close();

                // Show success message
                MessageBox.Show("Kayıt Başarılıyla Oluşturuldu!");
                Form2 mainForm = new Form2();

                // Show the main form
                mainForm.ShowDialog();

                // Close the login form
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error registering user: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
         

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

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
    }
    }

