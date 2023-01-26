using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static Project1_ASP.Pages.PrivacyModel;

namespace Project1_ASP.Pages
{
    public class PrivacyModel : PageModel
    {
        public List<AssignmentInfo> listAssignments = new List<AssignmentInfo>();

        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            string id = Request.Query["id"];

            if (id == null) {
                System.Diagnostics.Debug.WriteLine("Null Query");
            } else
            {
                System.Diagnostics.Debug.WriteLine("id is: " + id);
            }

            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=Project1;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Assignments", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AssignmentInfo assignmentInfo = new AssignmentInfo();
                                assignmentInfo.AssignmentID = reader.GetInt32(0);
                                assignmentInfo.DateAssigned = reader.GetDateTime(1);
                                assignmentInfo.OwnerFirst = reader.GetString(2);
                                assignmentInfo.OwnerLast = reader.GetString(3);
                                assignmentInfo.Address = reader.GetString(4);
                                assignmentInfo.Phone = reader.GetString(5);
                                assignmentInfo.ProblemDesc = reader.GetString(6);
                                assignmentInfo.Completed = (bool)reader.GetSqlBoolean(7);
                                assignmentInfo.EmployeeID = reader.GetInt32(8);


                                listAssignments.Add(assignmentInfo);

                            }
                        }
                    }
                    foreach (var item in listAssignments)
                    {
                        using (SqlCommand command = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID=@id", connection))
                        {
                            command.Parameters.AddWithValue("@id", item.EmployeeID);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    item.EmployeeFirstName = reader.GetString(1);
                                    item.EmployeeLastName = reader.GetString(2);
                                }       

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


        public class AssignmentInfo
        {
            public int AssignmentID;
            public DateTime DateAssigned;
            public string OwnerFirst;
            public string OwnerLast;
            public string Address;
            public string Phone;
            public string ProblemDesc;
            public bool Completed;
            public int EmployeeID;
            public string EmployeeFirstName;
            public string EmployeeLastName;
        }
    }
}