using CalendarAPI.Domain.Enums;
using CalendarAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarAPI.Application.Interfaces
{
    public interface ICalendarRepository
    {
        Task<DefaultResult> CreatePersonAvailability<T>(T personDbModel, EntryType entryType);
        Task<PersonAvailabilitySlots> GetAvailabilitySlots(string CandidateName);
        Task<PersonAvailabilitySlots> GetAvailabilitySlots(string InterviewerName, List<Slot> CandidateSlots);
    }
}
