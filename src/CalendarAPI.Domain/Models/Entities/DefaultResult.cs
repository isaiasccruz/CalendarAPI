using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class DefaultResult
    {
        public bool Sucess { get; set; }
        public string ErrorMesage { get; set; }
        public string SuccessMessage { get; set; }
        public DateTime DateTimeOfResult { get; set; }
        public IDictionary<string, string> resultBody { get; set; }
    }
}