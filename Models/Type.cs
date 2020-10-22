using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace Models
{
    public class Type
    {
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string TypeName { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
