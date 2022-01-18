using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class Slot
    {
        [JsonProperty("dateStart")]
        public DateTime DateStart { get; set; }

        [JsonProperty("dateEnd")]
        public DateTime DateEnd { get; set; }
    }
}