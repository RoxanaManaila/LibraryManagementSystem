using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace LibraryManagementSystem
{
    public partial class LoginForm : Form
    {
        //create Sql connection to the database
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Roxi\Documents\library.mdf;Integrated Security=True;Connect Timeout=30");
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void signupBtn_Click(object sender, EventArgs e)
        {
            RegisterForm rForm = new RegisterForm();
            rForm.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //mask the entered characters in the text box
            logIn_password.PasswordChar = logIn_showPassword.Checked ? '\0' : '*';
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (logIn_username.Text == "" || logIn_password.Text == "")
            {
                MessageBox.Show("It is mandatory to fill in all blank fields.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //we check to see if the connection with the Database has been established
                //if not, we try to establish connection
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        String selectData = "SELECT *FROM users WHERE username = @username AND password =@password";

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {
                            cmd.Parameters.AddWithValue("@username", logIn_username.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", logIn_password.Text.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Login successfully", "Information Message"
                                       , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                MainForm mForm = new MainForm();
                                mForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password", "Information Message"
                                       , MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fail to connect to DataBase: " + ex, "Error Message"
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }
    }
}
