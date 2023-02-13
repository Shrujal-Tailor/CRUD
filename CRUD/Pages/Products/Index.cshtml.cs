using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;

namespace CRUD.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<ProductInfo> listProduct = new List<ProductInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\;Initial Catalog=CRUD;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM products";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductInfo productInfo = new ProductInfo();
                                productInfo.id = "" + reader.GetInt32(0);
                                productInfo.name = reader.GetString(1);
                                productInfo.category = reader.GetString(2);
                                productInfo.description = reader.GetString(3);
                                productInfo.quantity = reader.GetString(4);
                                productInfo.image = reader.GetString(5);
                                productInfo.created_at = reader.GetDateTime(6).ToString();

                                listProduct.Add(productInfo);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.ToString());
            }
        }
    }
}
public class ProductInfo
{
    public String id;
    public String name;
    public String category;
    public String description;
    public String quantity;
    public String image;
    public String created_at;
}
