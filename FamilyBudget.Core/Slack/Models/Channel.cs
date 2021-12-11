using System.Collections.Generic;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Slack.Models
{

    public class ChannelResponse : GenericResponse
    {
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    public class ChannelList : GenericResponse
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }
    }

    public class Channel
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
