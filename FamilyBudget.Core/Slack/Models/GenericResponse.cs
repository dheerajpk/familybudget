using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Slack.Models
{
    public abstract class GenericResponse
    {

        [JsonProperty("ok")]
        public bool IsSuccess { get; set; }

        [JsonProperty("error")]
        public string ErrorMessage { get; set; }

       
    }

}
