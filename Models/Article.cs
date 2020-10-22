using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace Gateway.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Container { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
