using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EcommerceAppAPI.Utility
{
    public static class clsUtil
    {
        public static bool ValidateEmail(string Source)
        {
            return new EmailAddressAttribute().IsValid(Source);
        }
        public static bool ValidateLettersOnly(string Source)
        {
            if (!Regex.IsMatch(Source, @"^[\p{L}]+$") || string.IsNullOrEmpty(Source))
                return false;
            else
                return true;
        }
        public static bool ValidatePhoneNumber(string Source)
        {
            if (!Regex.IsMatch(Source, "^(?:\\+1)?\\s?\\(?\\d{3}\\)?[-.\\s]?\\d{3}[-.\\s]?\\d{4}$") || string.IsNullOrEmpty(Source))
                return false;
            else
                return true;
        }
        public static string ComputeHash(string Source)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Source));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
