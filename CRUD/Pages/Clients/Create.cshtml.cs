using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sun.security.util;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Serialization;

namespace CRUD.Pages.Clients
{
    [Authorize]
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        [BindProperty]
        public Details Details { get; set; }
        password EncryptData = new password();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Details.name;
            clientInfo.email = Details.email;
            clientInfo.password = Details.password;
            clientInfo.phone = Details.phone;
            clientInfo.address = Details.address;

            /*if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.password.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }*/

            //save the new data intu database

            try
            {
                String connectionString = "Data Source=.\\;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients" + "(name, email, password, phone, address) VALUES" + "(@name, @email, @password, @phone, @address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@password", EncryptData.Encode(clientInfo.password));
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.password = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New Client Added Correctly";
            Response.Redirect("/Clients/Index");
        }
    }
    public class Details
    {
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [Required]
        public string address { get; set; }
    }
}
