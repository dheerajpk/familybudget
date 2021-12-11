using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SocialDB.Services.Facebook.Models
{
    [DataContract]
    internal class Feeds
    {
        [JsonProperty("data")]
        public List<FeedMessage> Messages { get; set; }
    }
}
