namespace CRUD.Pages.Clients
{
    public class password
    {
        public string Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = System.Text.Encoding.UTF32.GetBytes(password);
                string EncrypedData = Convert.ToBase64String(EncDataByte);
                return EncrypedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro in encription" + ex.Message);
            }
        }
    }
}
