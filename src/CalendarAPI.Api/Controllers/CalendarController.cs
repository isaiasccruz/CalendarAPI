using CalendarAPI.Application.Interfaces;
using CalendarAPI.Domain.Models;
using CalendarAPI.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarAPI.Controllers
{
    [Route("api/Calendar")]
    [ApiController]
    public class CalendarController : ApiBaseController
    {

        private readonly ICalendarApplication _calendarApplication;
        public CalendarController(ICalendarApplication calendarApplication)
        {
            _calendarApplication = calendarApplication;
        }

        [HttpPost("calendar/availability")]
        [ProducesResponseType(typeof(DefaultResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] List<Person> obj)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _calendarApplication.CreatePersonAvailability(obj));
                }
                else
                {
                    return BadRequest(new DefaultResult() { DateTimeOfResult = DateTime.Now, ErrorMesage = "Invalid Model", SuccessMessage = null, Sucess = false, resultBody = null });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DefaultResult() { DateTimeOfResult = DateTime.Now, ErrorMesage = ex.Message, SuccessMessage = null, Sucess = false, resultBody = null });
            }
        }

        [HttpGet("calendar/availability")]
        [ProducesResponseType(typeof(DefaultResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync([FromQuery] PersonAvailability obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _calendarApplication.GetAvailabilitySlots(obj));
                }
                else
                {
                    return BadRequest(new DefaultResult() { DateTimeOfResult = DateTime.Now, ErrorMesage = "Invalid Model", SuccessMessage = null, Sucess = false, resultBody = null });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DefaultResult() { DateTimeOfResult = DateTime.Now, ErrorMesage = ex.Message, SuccessMessage = null, Sucess = false, resultBody = null });
            }
        }
    }
}