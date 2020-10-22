using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Container { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public decimal PricePerLiter
        {
            get => (TotalVolume != -1) ? Price / TotalVolume : -1;
        }

        [JsonIgnore]
        public decimal TotalVolume
        {
            get
            {
                var words = Container?.Split(' ');
                if ((words?.Length > 4) ||
                    int.TryParse(words[0], out int count) ||
                    words[0].Length < 2 ||
                    decimal.TryParse(words[2][0..^1], out decimal volume))
                {
                    return -1;
                }
                return count * volume;
            }
        }
    }
}
