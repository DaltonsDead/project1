using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static Project1_ASP.Pages.PrivacyModel;


namespace Project1_ASP.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();
        public AssignmentInfo assignmentInfo = new AssignmentInfo();
        public string error = "";
        public string success = "";

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
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.EmployeeID = reader.GetInt32(0);
                                employeeInfo.EmployeeFirstName = reader.GetString(1);
                                employeeInfo.EmployeeLastName = reader.GetString(2);

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

        public void OnPost()
        {
            try { assignmentInfo.EmployeeID = Int32.Parse(Request.Form["employee"]); } catch (Exception ex) {
                error = ex.Message;
                return;
            }
            assignmentInfo.OwnerFirst = Request.Form["OwnerFirst"];
            assignmentInfo.OwnerLast = Request.Form["OwnerLast"];
            assignmentInfo.Address = Request.Form["Address"];
            assignmentInfo.Phone = Request.Form["Phone"];
            assignmentInfo.ProblemDesc = Request.Form["ProblemDesc"];

            if (assignmentInfo.OwnerFirst.Length == 0 || assignmentInfo.OwnerLast.Length == 0 || assignmentInfo.Address.Length == 0 || assignmentInfo.Phone.Length == 0 || assignmentInfo.ProblemDesc.Length == 0)
            {
                error = "All fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=Project1;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Assignments " +
                                 "(DateAssigned, OwnerFirstName, OwnerLastName, Address, Phone, ProblemDescription, Completed, EmployeeID) VALUES" +
                                 "(GETDATE(), @OFN, @OLN, @Address, @Phone, @PD, 0,  @id);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@OFN", assignmentInfo.OwnerFirst);
                        command.Parameters.AddWithValue("@OLN", assignmentInfo.OwnerLast);
                        command.Parameters.AddWithValue("@Address", assignmentInfo.Address);
                        command.Parameters.AddWithValue("@Phone", assignmentInfo.Phone);
                        command.Parameters.AddWithValue("@PD", assignmentInfo.ProblemDesc);
                        command.Parameters.AddWithValue("@id", assignmentInfo.EmployeeID);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;
            }

            success = "Assignment added successfully";
        }
        
    }
}
