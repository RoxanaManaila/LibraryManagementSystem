﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Data;
using System.Data.SqlClient;


namespace LibraryManagementSystem
{
    internal class DataAddBooks
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Roxi\Documents\library.mdf;Integrated Security=True;Connect Timeout=30");
        public int ID{ set; get;}

        public string BookTitle { set; get; }

        public string Author { set; get; }

        public string Published { set; get; }

        public string Status { set; get; }

        public List<DataAddBooks> addBooksData()
        {
            List<DataAddBooks> listData= new List<DataAddBooks>();

            if(connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM books WHERE date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataAddBooks dab = new DataAddBooks();

                        while (reader.Read())
                        {
                            dab.ID = (int)reader["id"];
                            dab.BookTitle = reader["book_title"].ToString();
                            dab.Author = reader["author"].ToString();
                            dab.Published = reader["published_date"].ToString();
                            dab.Status = reader["status"].ToString();

                            listData.Add(dab);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error connecting to DataBase: " + ex);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;

        }
    }
}
