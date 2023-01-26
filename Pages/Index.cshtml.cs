using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Project1_ASP.Pages
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=Project1;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.EmployeeID = reader.GetInt32(0);
                                employeeInfo.EmployeeFirstName= reader.GetString(1);
                                employeeInfo.EmployeeLastName= reader.GetString(2);

                                listEmployees.Add(employeeInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class EmployeeInfo
    {
        public int EmployeeID;
        public string EmployeeFirstName;
        public string EmployeeLastName;
    }
}