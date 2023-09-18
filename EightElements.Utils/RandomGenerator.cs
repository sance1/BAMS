using System.Security.Cryptography;
using System.Text;



namespace EightElements.Utils
{
    public class RandomGenerator
    {

        public static long GenerateRandomNumbers(int size)
        {            
            const string chars = "0123456789";

            var data = new byte[size];
            var builder = new StringBuilder(size);

            using var provider = new RNGCryptoServiceProvider();            
            provider.GetNonZeroBytes(data);
                        
            foreach (byte b in data)
            {
                builder.Append(chars[b % (chars.Length)]);
            }

            return long.Parse(builder.ToString());
        }
        
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
