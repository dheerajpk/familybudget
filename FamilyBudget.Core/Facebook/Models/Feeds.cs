using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Facebook.Models
{
    [DataContract]
    public class Feeds
    {
        [JsonProperty("data")]
        public List<FeedMessage> Messages { get; set; }
    }
}
