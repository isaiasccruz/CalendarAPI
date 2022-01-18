using CalendarAPI.Application;
using CalendarAPI.Application.Interfaces;
using CalendarAPI.Controllers;
using CalendarAPI.Domain.DbModels;
using CalendarAPI.Domain.Enums;
using CalendarAPI.Domain.Models;
using CalendarAPI.Domain.Repositories;
using CalendarAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CalendarAPI.Tests.Controllers
{
    public class CalendarControllerTest
    {
        private readonly Mock<ICalendarRepository> _mockCalendarRepository;
        private readonly IUtilRepository _utilRepository;
        private readonly ICalendarApplication _calendarApplication;
        private readonly CalendarController _controller;

        public CalendarControllerTest()
        {
            _mockCalendarRepository = new Mock<ICalendarRepository>();
            _utilRepository = new UtilRepository();

            _calendarApplication = new CalendarApplication(_mockCalendarRepository.Object, _utilRepository);
            _controller = new CalendarController(_calendarApplication);
        }

        [Fact]
        public async Task CreatePersonAvailabilitySuccess()
        {
            List<Person> pplLst = new List<Person>();
            Person candidate = new Person()
            {
                Name = "Isaías",
                Role = "Candidate"
            };
            Person interviewer1 = new Person()
            {
                Name = "Marco",
                Role = "Interviewer"
            };
            Person interviewer2 = new Person()
            {
                Name = "Ramos",
                Role = "Interviewer"
            };
            List<Slot> slots = new List<Slot>()
            {
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 8, 0, 0), DateEnd = new DateTime(2022, 1, 13, 9, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 9, 0, 0), DateEnd = new DateTime(2022, 1, 13, 10, 0, 0)}
            };
            pplLst.Add(candidate);
            pplLst.Add(interviewer1);
            pplLst.Add(interviewer2);
            candidate.Slots = slots;
            interviewer1.Slots = slots;
            interviewer2.Slots = slots;

            IDictionary<string, string> dic = new Dictionary<string, string>
            {
                { $"Slot index: 0, name: {candidate.Name} and role: {EntryType.Candidate}", $"Success. Id: 1" },
                { $"Slot index: 1, name: {candidate.Name} and role: {EntryType.Candidate}", $"Success. Id: 2" },
                { $"Slot index: 2, name: {interviewer1.Name} and role: {EntryType.Interviewer}", $"Success. Id: 1" },
                { $"Slot index: 3, name: {interviewer1.Name} and role: {EntryType.Interviewer}", $"Success. Id: 2" },
                { $"Slot index: 4, name: {interviewer2.Name} and role: {EntryType.Interviewer}", $"Success. Id: 3" },
                { $"Slot index: 5, name: {interviewer2.Name} and role: {EntryType.Interviewer}", $"Success. Id: 4" }
            };

            DefaultResult df = new DefaultResult
            {
                DateTimeOfResult = DateTime.Now,
                ErrorMesage = null,
                Sucess = true,
                SuccessMessage = $"Slot of index: 0, name: {candidate.Name} and role: {EntryType.Candidate} was commited with success;Slot of index: 1, name: {candidate.Name} and role: {EntryType.Candidate} was commited with success;" +
                                 $"Slot of index: 2, name: {interviewer1.Name} and role: {EntryType.Interviewer} was commited with success;Slot of index: 3, name: {interviewer1.Name} and role: {EntryType.Interviewer} was commited with success;" +
                                 $"Slot of index: 4, name: {interviewer2.Name} and role: {EntryType.Interviewer} was commited with success;Slot of index: 5, name: {interviewer2.Name} and role: {EntryType.Interviewer} was commited with success;",
                resultBody = dic
            };

            _mockCalendarRepository.CallBase = true;

            _mockCalendarRepository
                .Setup(m => m.CreatePersonAvailability(It.IsAny<CandidateAvailabilityDbModel>(), EntryType.Candidate))
                .ReturnsAsync(df)
                .Verifiable();
            _mockCalendarRepository
                .Setup(m => m.CreatePersonAvailability(It.IsAny<InterviewerAvailabilityDbModel>(), EntryType.Interviewer))
                .ReturnsAsync(df)
                .Verifiable();

            IActionResult result = await _controller.PostAsync(pplLst);

            _mockCalendarRepository.Verify();

            Assert.IsType<OkObjectResult>(result);

            DefaultResult retorno = (DefaultResult)((OkObjectResult)result).Value;
            Assert.True(retorno.Sucess);
        }

        [Fact]
        public async Task GetAvailabilitySlotsSuccess()
        {

            PersonAvailability personAvailability = new PersonAvailability()
            {
                CandidateName = "Isaías",
                InterviewersQueryParam = new string[] { "Marco", "Ramos" }
            };
            List<Slot> candidateSlots = new List<Slot>()
            {
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 13, 0, 0), DateEnd = new DateTime(2022, 1, 13, 14, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 14, 0, 0), DateEnd = new DateTime(2022, 1, 13, 15, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 15, 0, 0), DateEnd = new DateTime(2022, 1, 13, 16, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 16, 0, 0), DateEnd = new DateTime(2022, 1, 13, 17, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 17, 0, 0), DateEnd = new DateTime(2022, 1, 13, 18, 0, 0)}
            };
            List<Slot> interviewerSlots = new List<Slot>()
            {
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 16, 0, 0), DateEnd = new DateTime(2022, 1, 13, 17, 0, 0)},
                new Slot(){ DateStart = new DateTime(2022, 1, 13, 17, 0, 0), DateEnd = new DateTime(2022, 1, 13, 18, 0, 0)}
            };

            PersonAvailabilitySlots candidate = new PersonAvailabilitySlots();
            candidate.Name = "Isaías";
            candidate.Slots = candidateSlots;

            PersonAvailabilitySlots interviewer = new PersonAvailabilitySlots();
            interviewer.Name = "Marco";
            interviewer.Slots = interviewerSlots;


            _mockCalendarRepository
                .Setup(m => m.GetAvailabilitySlots(It.IsAny<string>()))
                .ReturnsAsync(candidate)
                .Verifiable();
            _mockCalendarRepository
                .Setup(m => m.GetAvailabilitySlots(It.IsAny<string>(), It.IsAny<List<Slot>>()))
                .ReturnsAsync(interviewer)
                .Verifiable();

            IActionResult result = await _controller.GetAsync(personAvailability);

            _mockCalendarRepository.Verify();

            Assert.IsType<OkObjectResult>(result);

            DefaultResult retorno = (DefaultResult)((OkObjectResult)result).Value;
            Assert.True(retorno.Sucess);
        }
    }
}
