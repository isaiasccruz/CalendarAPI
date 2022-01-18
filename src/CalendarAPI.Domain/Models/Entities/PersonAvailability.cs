using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class PersonAvailability
    {
        [JsonProperty("candidateName")]
        public string CandidateName { get; set; }
        [JsonProperty("interviewers")]
        public string[] InterviewersQueryParam { get; set; }
    }
}