using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SocialDB.Services.Slack.Models
{
    internal abstract class GenericResponse
    {

        [JsonProperty("ok")]
        public bool IsSuccess { get; set; }

        [JsonProperty("error")]
        public string ErrorMessage { get; set; }


    }

    internal class MessageList : GenericResponse
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }

    internal class Message
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

    internal class ChannelResponse : GenericResponse
    {
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    internal class ChannelList : GenericResponse
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }
    }

    internal class Channel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_channel")]
        public bool IsChannel { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_general")]
        public bool IsGeneral { get; set; }

        [JsonProperty("unlinked")]
        public long Unlinked { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("name_normalized")]
        public string NameNormalized { get; set; }

        [JsonProperty("is_shared")]
        public bool IsShared { get; set; }

        [JsonProperty("is_org_shared")]
        public bool IsOrgShared { get; set; }

        [JsonProperty("is_member")]
        public bool IsMember { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_mpim")]
        public bool IsMpim { get; set; }

        [JsonProperty("last_read")]
        public string LastRead { get; set; }

        [JsonProperty("latest")]
        public object Latest { get; set; }

        [JsonProperty("unread_count")]
        public long UnreadCount { get; set; }

        [JsonProperty("unread_count_display")]
        public long UnreadCountDisplay { get; set; }

        [JsonProperty("members")]
        public string[] Members { get; set; }

        [JsonProperty("previous_names")]
        public object[] PreviousNames { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }
    }
}
