using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class Data
    {
        public string ReadData(string location)
        {
            string[] rows, columns;
            string data = "";
            string success = "False";

            //Get Text in file
            string text = System.IO.File.ReadAllText(location);
            //Create Array of values
            rows = text.Split(Environment.NewLine);
            //noRows = rows.Count();
           // data = new string[noRows, 8];

            foreach (string row in rows)
            {
                columns = row.Split(",");

                foreach (string column in columns)
                {
                    data = data + "'" + column + "',";
                }
                data = data.Remove(data.Length -1 ,1);
                success = SaveData(data);
                data = "";
            }

            return success;
        }



        public string SaveData(string data)
        {

            string success;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=(localdb)\Interview;Integrated Security=true;";
            cnn = new SqlConnection(connetionString);

            string insert = "INSERT INTO Employee (Title,FirstName,LastName,Gender,DateOfBirth,Email,JobRole,Salary) VALUES(" + data +")";
            Console.WriteLine(insert);
            SqlCommand cmd = new SqlCommand(insert, cnn);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                success = "True";
            }
            catch(SqlException e)
            {
                success = "Fail" + e.ToString();
            }
            finally
            {
                cnn.Close();
            }




            return success;
        }


    }
}
