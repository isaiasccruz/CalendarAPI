using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.DbModels
{
    [ExcludeFromCodeCoverage]
    [Table("tInterviewerAvailability")]
    public class InterviewerAvailabilityDbModel
    {
        public int Id { get; set; }
        public string InterviewerName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
    }
}