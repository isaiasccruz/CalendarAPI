using CalendarAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarAPI.Application.Interfaces
{
    public interface ICalendarApplication
    {
        Task<DefaultResult> CreatePersonAvailability(List<Person> obj);
        Task<DefaultResult> GetAvailabilitySlots(PersonAvailability obj);
    }
}
