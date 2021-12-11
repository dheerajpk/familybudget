using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Slack.Models
{

    public class MessageList : GenericResponse
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("bot_id")]
        public string BotId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subtype")]
        public string Subtype { get; set; }

        [JsonProperty("ts")]
        public string Ts { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }
}
