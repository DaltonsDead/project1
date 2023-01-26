using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static Project1_ASP.Pages.PrivacyModel;

namespace Project1_ASP.Pages.Assignments
{
    public class EditModel : PageModel
    {
        public AssignmentInfo assignmentInfo = new AssignmentInfo();
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
                    String sql = "SELECT * FROM Assignments WHERE AssignmentID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                assignmentInfo.AssignmentID = reader.GetInt32(0);
                                assignmentInfo.DateAssigned = reader.GetDateTime(1);
                                assignmentInfo.OwnerFirst = reader.GetString(2);
                                assignmentInfo.OwnerLast = reader.GetString(3);
                                assignmentInfo.Address = reader.GetString(4);
                                assignmentInfo.Phone = reader.GetString(5);
                                assignmentInfo.ProblemDesc = reader.GetString(6);
                                assignmentInfo.Completed = (bool)reader.GetSqlBoolean(7);
                                assignmentInfo.EmployeeID = reader.GetInt32(8);
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
            assignmentInfo.OwnerFirst = Request.Form["OwnerFirst"];
            assignmentInfo.OwnerLast = Request.Form["OwnerLast"];
            assignmentInfo.Address = Request.Form["Address"];
            assignmentInfo.Phone = Request.Form["Phone"];
            assignmentInfo.ProblemDesc = Request.Form["ProblemDesc"];
            int Completed = Int32.Parse(Request.Form["Completed"]);

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
                    string sql = "UPDATE Assignments " +
                                 "SET OwnerFirstName=@OFN, OwnerLastName=@OLN, Address=@Address, Phone=@Phone, ProblemDescription=@PD, Completed=@Completed " +
                                 "Where AssignmentID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@OFN", assignmentInfo.OwnerFirst);
                        command.Parameters.AddWithValue("@OLN", assignmentInfo.OwnerLast);
                        command.Parameters.AddWithValue("@Address", assignmentInfo.Address);
                        command.Parameters.AddWithValue("@Phone", assignmentInfo.Phone);
                        command.Parameters.AddWithValue("@PD", assignmentInfo.ProblemDesc);
                        if (Completed == 1)
                        {
                            command.Parameters.AddWithValue("@Completed", 1);
                        } else
                        {
                            command.Parameters.AddWithValue("@Completed", 0);
                        }
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

            success = "Assignment added successfully";

        }

    }
}
