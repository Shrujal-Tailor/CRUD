using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD.Pages.Products
{
    public class EditModel : PageModel
    {
        public IFormFile ProductImage { get; set; }
        public ProductInfo productInfo = new ProductInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM products WHERE id = @id";
                    using (SqlCommand command
                        = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.id = "" + reader.GetInt32(0);
                                productInfo.name = reader.GetString(1);
                                productInfo.category = reader.GetString(2);
                                productInfo.description = reader.GetString(3);
                                productInfo.quantity = reader.GetString(4);
                                productInfo.image = reader.GetString(5);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            productInfo.id = Request.Form["id"];
            productInfo.name = Request.Form["name"];
            productInfo.category = Request.Form["category"];
            productInfo.description = Request.Form["description"];
            productInfo.quantity = Request.Form["quantity"];
            productInfo.image = Request.Form["image"];

            if (productInfo.name.Length == 0 || productInfo.category.Length == 0 || productInfo.description.Length == 0 || productInfo.quantity.Length == 0 || productInfo.image.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE products SET " + "name=@name,category=@category,description=@description,quantity=@quantity,image=@image " + "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", productInfo.id);
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

            Response.Redirect("/Products/Index");
        }
    }
}
