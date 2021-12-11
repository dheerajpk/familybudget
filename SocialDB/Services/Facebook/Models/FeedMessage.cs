using System;
using System.Text;
using Newtonsoft.Json;

namespace SocialDB.Services.Facebook.Models
{
    internal class FeedMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

    }
}
