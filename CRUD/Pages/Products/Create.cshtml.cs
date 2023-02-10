using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD.Pages.Products
{
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            productInfo.name = Request.Form["name"];
            productInfo.category = Request.Form["category"];
            productInfo.description = Request.Form["description"];
            productInfo.quantity = Request.Form["quantity"];
            productInfo.image = Request.Form["image"];

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
}
