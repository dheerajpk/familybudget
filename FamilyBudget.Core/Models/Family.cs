using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Models
{
    public class Family
    {
        [JsonProperty("family")]
        public string FamilyName { get; set; }

        [JsonProperty("familyCode")]
        public string FamilyCode { get; set; }

        [JsonProperty("familyMembers")]
        public List<FamilyMember> FamilyMembers { get; set; }

        [JsonProperty("expenseCycleStartDay")]
        public int ExpenseCycleStartDay { get; set; }
    }
}
