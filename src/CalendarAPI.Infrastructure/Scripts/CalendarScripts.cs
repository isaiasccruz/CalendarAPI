using System;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Infrastructure.Scripts
{
    [ExcludeFromCodeCoverage]
    public static class CalendarScripts
    {
        public static readonly string GetSlotsFromCandidate = @"select * from [dbo].[tCandidateAvailability] where CandidateName = '{0}'";

        public static readonly string GetSlotsFromInterviewer = @"select * from [dbo].[tInterviewerAvailability] where InterviewerName = '{0}' AND DateStart = '{1}' AND DateEnd = '{2}'";
    }
}
