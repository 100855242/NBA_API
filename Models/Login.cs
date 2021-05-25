using System;
using System.Security.Cryptography;

namespace DotNetAuthentication.Controllers
{
    public class Login
    {

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public Login()
        {

        }

        public bool CheckPass(string SavedPasswordHash, string Password)
        {

            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(SavedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}