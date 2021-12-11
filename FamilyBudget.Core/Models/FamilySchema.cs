using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Models
{
    public class FamilySchema<T> where T : class
    {
        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonIgnore]
        public string ExternalRefernceId { get; set; }
    }
}
