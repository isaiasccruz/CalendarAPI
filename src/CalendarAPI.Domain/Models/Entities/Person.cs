using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class Person
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("slots")]
        public List<Slot> Slots { get; set; }
    }
}