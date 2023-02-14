using System.Text;

namespace CRUD.Pages.Clients
{
    public class password
    {
        public string Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = Encoding.UTF32.GetBytes(password);
                string EncrypedData = Convert.ToBase64String(EncDataByte);
                return EncrypedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in encription" + ex.Message);
            }
        }
        public string Decode(string password)
        {
            try
            {
                byte[] DecDataByte = new byte[password.Length];
                var DecodeData = Convert.FromBase64String(password);
                var DecodedData =  Encoding.UTF32.GetString(DecodeData);
                return DecodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Decoding" + ex.Message);
            }
        }
    }
}
