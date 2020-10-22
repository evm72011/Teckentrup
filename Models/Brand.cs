using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text.Json.Serialization;


namespace Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<Type> Types { get; set; }
    }
}
