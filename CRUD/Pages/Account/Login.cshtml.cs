using com.sun.xml.@internal.bind.v2.model.core;
using CRUD.Pages.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;

namespace CRUD.Pages.Access
{
    public class LoginModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        [BindProperty]
        public Credential Credential { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync() 
        {
            string email = Credential.email;
            try
            {
                String connectionString = "Data Source=.\\;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE email = @email";
                    using (SqlCommand command
                        = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.password = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /*errorMessage = ex.Message;*/
            }

            if (!ModelState.IsValid) return Page();

            if (Credential.email == clientInfo.email && Credential.Password == clientInfo.password)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Email, Credential.email),
                    new Claim(ClaimTypes.Name, clientInfo.name)
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
    public class Credential
    {
        [Required]
        public string email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
