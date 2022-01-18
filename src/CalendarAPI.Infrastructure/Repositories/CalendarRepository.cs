using CalendarAPI.Domain.DbModels;
using CalendarAPI.Domain.Enums;
using CalendarAPI.Domain.Models;
using CalendarAPI.Domain.Repositories;
using CalendarAPI.Infrastructure.Scripts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalendarAPI.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CalendarRepository : ICalendarRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IDatabase _database;

        public CalendarRepository(HttpClient httpClient, IDatabase database)
        {
            _httpClient = httpClient;
            _database = database;
        }

        public async Task<DefaultResult> CreatePersonAvailability<T>(T personDbModel, EntryType entryType)
        {
            DefaultResult response = new DefaultResult();
            try
            {
                if (entryType == EntryType.Candidate)
                {
                    int? answr = await CreateCandidate((CandidateAvailabilityDbModel)(object)personDbModel);
                    response.SuccessMessage = $"Id: {answr}";
                }
                else if (entryType == EntryType.Interviewer)
                {
                    int? answr = await CreateInterviewer((InterviewerAvailabilityDbModel)(object)personDbModel);
                    response.SuccessMessage = $"Id: {answr}";
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}");
                throw ex;
            }
            response.Sucess = true;
            response.ErrorMesage = null;
            response.DateTimeOfResult = DateTime.Now;
            response.resultBody = null;

            return response;
        }

        public async Task<PersonAvailabilitySlots> GetAvailabilitySlots(string CandidateName)
        {
            PersonAvailabilitySlots response = new PersonAvailabilitySlots();
            response.Name = CandidateName;

            try
            {
                List<CandidateAvailabilityDbModel> candidate = (List<CandidateAvailabilityDbModel>)await _database.QueryWithRetry<CandidateAvailabilityDbModel>(string.Format(CalendarScripts.GetSlotsFromCandidate, CandidateName), DataBase.defaultDB);

                if (candidate.Any())
                {
                    response.Slots = new List<Slot>();

                    foreach (var item in candidate)
                    {
                        Slot slot = new Slot();
                        slot.DateStart = item.DateStart;
                        slot.DateEnd = item.DateEnd;

                        response.Slots.Add(slot);
                    }
                }
                else
                {
                    response = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}");
                throw ex;
            }
            return response;
        }

        public async Task<PersonAvailabilitySlots> GetAvailabilitySlots(string InterviewerName, List<Slot> CandidateSlots)
        {
            PersonAvailabilitySlots response = new PersonAvailabilitySlots();
            response.Name = InterviewerName;
            try
            {
                response.Slots = new List<Slot>();

                foreach (var candidateSlot in CandidateSlots)
                {
                    List<InterviewerAvailabilityDbModel> interviewerSlotDb = (List<InterviewerAvailabilityDbModel>)await _database.QueryWithRetry<InterviewerAvailabilityDbModel>(string.Format(CalendarScripts.GetSlotsFromInterviewer, InterviewerName, candidateSlot.DateStart.ToString("yyyy-MM-dd HH:mm:ss"), candidateSlot.DateEnd.ToString("yyyy-MM-dd HH:mm:ss")), DataBase.defaultDB);

                    if (interviewerSlotDb.Any())
                    {
                        foreach (var item in interviewerSlotDb)
                        {
                            Slot slot = new Slot();
                            slot.DateStart = item.DateStart;
                            slot.DateEnd = item.DateEnd;

                            response.Slots.Add(slot);
                        }                        
                    }                
                }

                if (!response.Slots.Any())
                {
                    response = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}");
                throw ex;
            }
            return response;
        }

        #region CandidateAvailability
        private async Task<int?> CreateCandidate(CandidateAvailabilityDbModel obj)
        {
            try
            {
                return await _database.Insert(obj, DataBase.defaultDB);
            }
            catch (Exception ex)
            {
                Log.Error("Error inserting the CandidateAvailabilityDbModel: " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region InterviewerAvailability       
        private async Task<int?> CreateInterviewer(InterviewerAvailabilityDbModel obj)
        {
            try
            {
                return await _database.Insert(obj, DataBase.defaultDB);
            }
            catch (Exception ex)
            {
                Log.Error("Error inserting the InterviewerAvailabilityDbModel: " + ex.Message);
                throw ex;
            }
        }
        #endregion

    }
}
