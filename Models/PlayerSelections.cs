using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class PlayerSelections
    {
        public string Token { get; set; }
        public string TeamName { get; set; }
        
        public int[] PlayerKeys { get; set; }
        public PlayerSelections()
        {
        }

        //int userId,
        //UserId = userId;
        public PlayerSelections(string token, string teamName, int[] playerKeys)
        {
            Token = token;
            TeamName = teamName;            
            PlayerKeys = playerKeys;
        }
    }
}
