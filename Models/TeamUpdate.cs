using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class TeamUpdate
    {
        public string Token { get; set; }
        public string TeamName { get; set; }

        public TeamUpdate(string token, string teamName)
        {
            Token = token;
            TeamName = teamName;
        }
    }
}
