using System.Collections.Generic;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isIncome")]
        public bool IsIncome { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Categories
    {
        [JsonProperty("categories")]
        public List<Category> CategoryList { get; set; }
    }
}
