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
    public partial class RegisterForm : Form
    {
        //create Sql connection to the database
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Roxi\Documents\library.mdf;Integrated Security=True;Connect Timeout=30");
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void signIn_btn_Click(object sender, EventArgs e)
        {
            LoginForm lForm = new LoginForm();
            lForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            //first, we neede to check if all text fields have been filled in
            if(register_email.Text == "" || register_username.Text == "" || register_password.Text == "")
            {
                MessageBox.Show("It is mandatory to fill in all blank fields.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //we check to see if the connection with the Database has been established
                //if not, we try to establish connection
                if(connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        String checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username";

                        using (SqlCommand checkCMD = new SqlCommand(checkUsername, connect))
                        {
                            checkCMD.Parameters.AddWithValue("@username", register_username.Text.Trim());

                            int count = (int)checkCMD.ExecuteScalar();
                            if(count > 0)
                            {
                                MessageBox.Show(register_username.Text.Trim()
                                    + "is not available", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                //to get the date today
                                DateTime day = DateTime.Today;

                                String insertData = "INSERT INTO users (email, username, password, date_regiser)" +
                                    "VALUES(@email, @username, @password, @date)";

                                using (SqlCommand insertCMD = new SqlCommand(insertData, connect))
                                {
                                    insertCMD.Parameters.AddWithValue("@email", register_email.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@username", register_username.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@password", register_password.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@date", day.ToString());

                                    insertCMD.ExecuteNonQuery();

                                    MessageBox.Show("Your account has been successfully registered", "Information Message"
                                        , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    LoginForm lForm = new LoginForm();
                                    lForm.ShowDialog();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Fail to connect to DataBase: " +ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void register_showPassword_CheckedChanged(object sender, EventArgs e)
        {
            //mask the entered characters in the text box
            register_password.PasswordChar = register_showPassword.Checked ? '\0' : '*';
        }
    }
}
