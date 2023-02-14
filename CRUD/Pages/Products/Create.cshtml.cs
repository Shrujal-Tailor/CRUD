using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace CRUD.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public String errorMessage = "";
        public String successMessage = "";
        [BindProperty]
        public Details Details { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            productInfo.name = Details.id;
            productInfo.category = Details.category;
            productInfo.description = Details.description;
            productInfo.quantity = Details.quantity;
            productInfo.image = Details.image;

            if (productInfo.name.Length == 0 || productInfo.category.Length == 0 || productInfo.description.Length == 0 || productInfo.quantity.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the new data intu database

            try
            {
                String connectionString = "Data Source=.;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO products " + "(name, category, description, quantity, image) VALUES " + "(@name, @category, @description, @quantity, @image);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productInfo.name);
                        command.Parameters.AddWithValue("@category", productInfo.category);
                        command.Parameters.AddWithValue("@description", productInfo.description);
                        command.Parameters.AddWithValue("@quantity", productInfo.quantity);
                        command.Parameters.AddWithValue("@image", productInfo.image);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            productInfo.name = "";
            productInfo.category = "";
            productInfo.description = "";
            productInfo.quantity = "";
            productInfo.image = "";
            successMessage = "New Product Added Correctly";
            Response.Redirect("/Products/Index");
        }
    }
    public class Details
    {
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string category { get;set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string quantity { get; set; }
        public string image { get; set; }
    }
}
