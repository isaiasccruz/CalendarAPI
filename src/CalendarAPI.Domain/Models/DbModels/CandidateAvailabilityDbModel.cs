using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.DbModels
{
    [ExcludeFromCodeCoverage]
    [Table("tCandidateAvailability")]
    public class CandidateAvailabilityDbModel
    {
        public int Id { get; set; }
        public string CandidateName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }

    }
}