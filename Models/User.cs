using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class User
    {
        public User()
        {

        }

        public User(string FirstName, string LastName, string UserName, string PasswordHash)
        {
            this.UserName = UserName;
            this.PasswordHash = PasswordHash;
        }


        [JsonIgnore]
        public int UserId { get; }
       
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Hash(string Password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes).ToString();

            return savedPasswordHash;
        }        
    }
}
