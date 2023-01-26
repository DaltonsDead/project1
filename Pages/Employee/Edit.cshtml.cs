using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Project1_ASP.Pages.Employee
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string error = "";
        public string success = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=Project1;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Employees WHERE EmployeeID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employeeInfo.EmployeeID = reader.GetInt32(0);
                                employeeInfo.EmployeeFirstName = reader.GetString(1);
                                employeeInfo.EmployeeLastName = reader.GetString(2);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;
            }

        }


        public void OnPost()
        {
            string id = Request.Query["id"];

            employeeInfo.EmployeeFirstName = Request.Form["fName"];
            employeeInfo.EmployeeLastName = Request.Form["lName"];

            if (employeeInfo.EmployeeFirstName.Length == 0 || employeeInfo.EmployeeLastName.Length == 0)
            {
                error = "All the fields are required";
                return;
            }

            //save data to database
            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=Project1;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Employees " +
                                 "SET EmployeeFirstName=@fName, EmployeeLastName=@lName " + 
                                 "WHERE EmployeeID=@id;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@fName", employeeInfo.EmployeeFirstName);
                        command.Parameters.AddWithValue("@lName", employeeInfo.EmployeeLastName);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;
            }


            employeeInfo.EmployeeFirstName = ""; employeeInfo.EmployeeLastName = "";
            success = "Employee Updated succesfully";
        }

    }
}
