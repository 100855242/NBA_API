using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAuthentication.Models
{
    public class ColumnHeaders
    {
        [JsonProperty("COLUMN_NAME")]
        public string COLUMN_NAME { get; set; }

        public ColumnHeaders()
        {

        }

        public ColumnHeaders(string cOLUMN_NAME)
        {
            this.COLUMN_NAME = cOLUMN_NAME;
        }
    }
}
