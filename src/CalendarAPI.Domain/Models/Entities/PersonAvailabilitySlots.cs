using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class PersonAvailabilitySlots
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slots")]
        public IList<Slot> Slots { get; set; }
    }
}