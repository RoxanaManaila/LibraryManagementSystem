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
using System.IO;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace LibraryManagementSystem
{
    public partial class AddBooks : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Roxi\Documents\library.mdf;Integrated Security=True;Connect Timeout=30");
        public AddBooks()
        {
            InitializeComponent();
        }

        //import pictures 
        private void addBooks_ImportBtn_Click(object sender, EventArgs e)
        {
            String imagePath = "";

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    addBooks_picture.ImageLocation = imagePath;

                    //check for special characters
                    /*string filePath = addBooks_picture.ImageLocation;
                    if (ContainsSpecialCharacters(filePath))
                    {
                        Console.WriteLine("File path contains special characters.");
                    }
                    else
                    {
                        Console.WriteLine("File path does not contain special characters.");
                    }*/

                    //check image path
                    //MessageBox.Show("Image Path: " + addBooks_picture.ImageLocation, "Image Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex,"Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Function to check for special characters in a string
        /*public bool ContainsSpecialCharacters(string input)
        {
            // Regular expression pattern to match any character that is not alphanumeric or whitespace
            string pattern = @"[^a-zA-Z0-9\s]";

            // Create a regular expression object
            Regex regex = new Regex(pattern);

            // Check if the input string contains any matches to the pattern
            return regex.IsMatch(input);
        }*/

        //addBooks_picture.Image , ".jpg"

        private void addBooks_addBtn_Click(object sender, EventArgs e)
        {
            if( addBooks_picture.Image == null
                || addBooks_bookTitle.Text == ""
                || addBooks_author.Text == ""
                || addBooks_published.Value == null
                || addBooks_status.Text == "")
                {
                MessageBox.Show("Please fill in all the blanck fields", "Error Message"
                                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(connect.State == ConnectionState.Closed)
                {
                    try
                    {
                        DateTime today = DateTime.Today;
                        connect.Open();

                        string insertData = "INSERT INTO books" +
                            "(book_title, author, published_date, image, status, date_insert)" +
                            "VALUES (@bookTitle, @author, @publishedDate, @image, @status, @dateInsert)";

                        string path = Path.Combine(@"C:\Users\Roxi\Desktop\projects\LibraryManagementSystem\Books_Directory\" +
                           addBooks_bookTitle.Text.Trim() + "_" + addBooks_author.Text.Trim() + "_" + today.ToString() + "_" + ".jpg");

                        string directoryPath = Path.GetDirectoryName(path);

                        // Get the file path
                        /*string filePath = addBooks_picture.ImageLocation;

                        // Check if the file path is not empty or whitespace
                        if (!string.IsNullOrWhiteSpace(filePath))
                        {
                            // Sanitize the file path (replace special characters with underscores)
                            string sanitizedFilePath = Regex.Replace(filePath, @"[^\w\s]", "_");

                            // Get the directory path for storing the image file
                            string directoryPath = Path.GetDirectoryName(sanitizedFilePath);

                            // Check if the directory path is not empty or whitespace
                            if (!string.IsNullOrWhiteSpace(directoryPath))
                            {
                                // Ensure that the directory exists before attempting to create it
                                if (!Directory.Exists(directoryPath))
                                {
                                    // Create the directory
                                    Directory.CreateDirectory(directoryPath);
                                }

                                // Copy the image file to the sanitized file path
                                File.Copy(filePath, sanitizedFilePath, true);
                            }
                            else
                            {
                                // Handle case where directory path is empty
                                MessageBox.Show("Unable to extract directory path from the file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Handle case where file path is empty
                            MessageBox.Show("File path is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }*/



                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }


                        File.Copy(addBooks_picture.ImageLocation, path, true);




                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@bookTitle", addBooks_bookTitle.Text.Trim());
                            cmd.Parameters.AddWithValue("@author", addBooks_author.Text.Trim());
                            cmd.Parameters.AddWithValue("@publishedDate", addBooks_published.Text.Trim());
                            cmd.Parameters.AddWithValue("@status", addBooks_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@image", addBooks_picture.ImageLocation);
                            cmd.Parameters.AddWithValue("@dateInsert", today.ToString());

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Added Successfully", "Information Message"
                                     , MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error" +ex, "Error Message"
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
