using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Facebook.Models
{
   public class FeedMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

    }
}
