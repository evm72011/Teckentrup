using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Xml.Schema;

namespace Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Container { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public decimal? PricePerLiter
        {
            get
            {
                if ((TotalVolume is null) || (TotalVolume == 0))
                {
                    return null;
                }
                else
                {
                    return Math.Round((Price / TotalVolume).Value, 2);
                }
            }
        }

        [JsonIgnore]
        public decimal? TotalVolume
        {
            get
            {
                if (string.IsNullOrEmpty(Container)) 
                    return null;

                var words = Container.Split(' ');
                if (words?.Length != 4) 
                    return null;

                if (!int.TryParse(words[0], out int count))
                    return null;

                if (words[2].Length < 2)
                    return null;

                if (words[2][^1..] != "L")
                    return null;

                if (!decimal.TryParse(words[2][0..^1].Replace(",","."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal volume))
                    return null;

                return count * volume;
            }
        }
    }
}
