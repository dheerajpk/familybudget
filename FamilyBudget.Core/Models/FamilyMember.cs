using Newtonsoft.Json;

namespace FamilyBudget.Core.Models
{
    public  class FamilyMember
    {
        [JsonProperty("memberId")]
        public string MemberId { get; set; }

        [JsonProperty("memberName")]
        public string MemberName { get; set; }

        [JsonProperty("isParent")]
        public bool IsParent { get; set; }
    }
}