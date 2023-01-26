using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Project1_ASP.Pages.Employee
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string error = "";
        public string success = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
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
                    string sql = "INSERT INTO Employees " +
                                 "(EmployeeFirstName, EmployeeLastName) VALUES" +
                                 "(@fName, @lName);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@fName", employeeInfo.EmployeeFirstName);
                        command.Parameters.AddWithValue("@lName", employeeInfo.EmployeeLastName);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch  (Exception ex)
            {
                error= ex.Message;
                return;
            }


            employeeInfo.EmployeeFirstName = ""; employeeInfo.EmployeeLastName = "";
            success = "Employee added succesfully";

  
        }
    }
}
