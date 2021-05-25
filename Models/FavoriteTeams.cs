using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class FavoriteTeam
    {
        public string Token { get; set; }

        public string TeamNames { get; set; }

        public bool IsFav { get; set; }
    }
}
