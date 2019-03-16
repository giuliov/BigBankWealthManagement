using Newtonsoft.Json;
using System.Collections.Generic;

namespace bbwm
{
    public class CustomerPortfolio
    {
        [JsonProperty(PropertyName = "id")]
        public string CustomerId { get; set; }
        public List<string> Symbols { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
