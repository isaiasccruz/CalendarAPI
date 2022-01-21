using CalendarAPI.Application.Interfaces;
using CalendarAPI.Domain.DbModels;
using CalendarAPI.Domain.Enums;
using CalendarAPI.Domain.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Application
{
    public class CalendarApplication : ICalendarApplication
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUtilRepository _utilRepository;

        public CalendarApplication(ICalendarRepository calendarRepository, IUtilRepository utilRepository)
        {
            _calendarRepository = calendarRepository;
            _utilRepository = utilRepository;
        }

        public async Task<DefaultResult> CreatePersonAvailability(List<Person> obj)
        {
            Log.Information($"{DateTime.Now:HH:mm:ss} - Processing Slot Creation Request");

            DefaultResult response = new DefaultResult();
            IDictionary<string, string> dic = new Dictionary<string, string>();

            response.Sucess = false;
            response.SuccessMessage = null;

            try
            {
                foreach (Person person in obj)
                {
                    List<Slot> slots = person.Slots.ToList();

                    if (!string.IsNullOrEmpty(person.Name) && !string.IsNullOrEmpty(person.Role) && (slots != null && slots.Count > 0))
                    {
                        for (int i = 0; i < slots.Count; i++)
                        {                            

                            if ((slots[i].DateStart >= slots[i].DateEnd))
                            {
                                response.ErrorMesage += $"Slot of index: {i}, name: {person.Name} with Invalid DateStart/DateEnd (DateEnd must be at least one hour older than DateStart);";
                                response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", "Invalid DateStart/DateEnd");
                                Log.Error($"Invalid Data on slot of index {i}.");
                            }
                            else if ((slots[i].DateStart.Minute != 00 && slots[i].DateStart.Second != 00) && (slots[i].DateEnd.Minute != 00 && slots[i].DateEnd.Second != 00))
                            {
                                response.ErrorMesage += $"Slot of index: {i}, name: {person.Name} with Invalid DateStart/DateEnd (An hour slot can't have broken minutes/seconds within an hour. Valid example: 13:00:00 - 14:00:00);";
                                response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", "Invalid DateStart/DateEnd");
                                Log.Error($"Invalid Data on slot of index {i}.");
                            }
                            else if (slots[i].DateStart.AddHours(1) != slots[i].DateEnd)
                            {
                                response.ErrorMesage += $"Slot of index: {i}, name: {person.Name} with Invalid DateStart/DateEnd (An interview slot is a 1-hour period of time that spreads from the beginning of any hour until the beginning of the next hour);";
                                response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", "Invalid DateStart/DateEnd");
                                Log.Error($"Invalid Data on slot of index {i}.");
                            }
                            else
                            {
                                if (Enum.TryParse(person.Role, out EntryType entryTypeC) && entryTypeC == EntryType.Candidate)
                                {

                                    CandidateAvailabilityDbModel candidate = new CandidateAvailabilityDbModel()
                                    {
                                        CandidateName = person.Name,
                                        DateStart = slots[i].DateStart,
                                        DateEnd = slots[i].DateEnd,
                                    };

                                    try
                                    {
                                        DefaultResult answr = await _calendarRepository.CreatePersonAvailability(candidate, entryTypeC);

                                        response.Sucess = true;
                                        response.SuccessMessage += $"Candidate Slot of index: {i}, name: {candidate.CandidateName} and role: {entryTypeC} was commited with success;";
                                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {candidate.CandidateName} and role: {entryTypeC}", $"Success. {answr.SuccessMessage}");
                                    }
                                    catch (Exception ex)
                                    {
                                        response.ErrorMesage += $"Candidate Slot of index: {i}, name: {person.Name} returned error trying to insert the slot: {ex.Message};";
                                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", $"Error trying to insert the slot: {ex.Message}");
                                        Log.Error($"Candidate Slot of index: {i}, name: {person.Name} returned error trying to insert the slot: {ex.Message}");
                                    }
                                }
                                else if (Enum.TryParse(person.Role, out EntryType entryTypeI) && entryTypeI == EntryType.Interviewer)
                                {
                                    InterviewerAvailabilityDbModel interviewer = new InterviewerAvailabilityDbModel()
                                    {
                                        InterviewerName = person.Name,
                                        DateStart = slots[i].DateStart,
                                        DateEnd = slots[i].DateEnd,
                                    };

                                    try
                                    {
                                        DefaultResult answr = await _calendarRepository.CreatePersonAvailability(interviewer, entryTypeI);

                                        response.Sucess = true;
                                        response.SuccessMessage += $"Interviewer Slot of index: {i}, name: {interviewer.InterviewerName} and role: {entryTypeI} was commited with success;";
                                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {interviewer.InterviewerName} and role: {entryTypeI}", $"Success. {answr.SuccessMessage}");
                                    }
                                    catch (Exception ex)
                                    {
                                        response.ErrorMesage += $"Interviewer Slot of index: {i}, name: {person.Name} returned error trying to insert the slot: {ex.Message};";
                                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", $"Error trying to insert the slot: {ex.Message}");
                                        Log.Error($"InterviewerSlot of index: {i}, name: {person.Name} returned error trying to insert the slot: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    response.ErrorMesage += $"Slot of index: {i}, name: {person.Name} with Invalid Role;";
                                    response.resultBody = _utilRepository.UpsertDictionary(dic, $"Slot index: {i}, name: {person.Name}", "Invalid Role");
                                    Log.Error($"Invalid Role Data on slot of index {i}.");
                                }
                            }
                        }
                    }
                    else
                    {
                        response.Sucess = false;
                        response.SuccessMessage = null;
                        response.ErrorMesage = $"Invalid Data on Request Body.";
                        response.resultBody = null;
                        Log.Error($"Invalid Data on Request Body.");

                    }
                }

                response.DateTimeOfResult = DateTime.Now;
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}");
                response.Sucess = false;
                response.SuccessMessage = null;
                response.ErrorMesage = $"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}";
                response.resultBody = null;
            }

            Log.Information($"{DateTime.Now:HH:mm:ss} - Slot Creation Request Processed");

            return response;

        }
        public async Task<DefaultResult> GetAvailabilitySlots(PersonAvailability obj)
        {
            Log.Information($"{DateTime.Now:HH:mm:ss} - Processing Slot Retrieval Request");

            DefaultResult response = new DefaultResult();
            IDictionary<string, string> dic = new Dictionary<string, string>();
            PersonAvailabilitySlots candidateSlots = null;
            List<PersonAvailabilitySlots> interviewerSlots = new List<PersonAvailabilitySlots>();

            response.Sucess = false;
            response.SuccessMessage = null;

            try
            {
                if (string.IsNullOrEmpty(obj.CandidateName))
                {
                    response.ErrorMesage = $"Invalid Candidate. Candidate can't be white space or null";
                    response.resultBody = null;
                    Log.Error($"Invalid Candidate. Candidate cant be white space or null");
                }
                else
                {
                    try
                    {
                        candidateSlots = await _calendarRepository.GetAvailabilitySlots(obj.CandidateName);
                    }
                    catch (Exception ex)
                    {
                        response.Sucess = false;
                        response.ErrorMesage += $"Candidate: {obj.CandidateName} returned error trying to get the slots: {ex.Message};";
                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Candidate: {obj.CandidateName}", $"returned error trying to get the slots: {ex.Message}");
                        Log.Error($"Candidate: {obj.CandidateName} returned error trying to get the slots: {ex.Message}");
                    }

                    if (candidateSlots != null && candidateSlots.Slots.Any())
                    {
                        response.Sucess = true;
                        response.SuccessMessage += $"Candidate: {candidateSlots.Name}, Available Slots: {JsonConvert.SerializeObject(candidateSlots.Slots)};";
                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Candidate: {candidateSlots.Name}", $"Available Slots: {JsonConvert.SerializeObject(candidateSlots.Slots)}");

                        if (obj.InterviewersQueryParam != null || obj.InterviewersQueryParam.Length > 0)
                        {                            
                            for (int i = 0; i < obj.InterviewersQueryParam.Length; i++)
                            {
                                try
                                {
                                    var answr = await _calendarRepository.GetAvailabilitySlots(obj.InterviewersQueryParam[i], candidateSlots.Slots.ToList());

                                    if (answr != null)
                                    {
                                        interviewerSlots.Add(answr);

                                        foreach (var interviewerSlot in answr.Slots)
                                        {
                                            response.SuccessMessage += $"Interviewer: {obj.InterviewersQueryParam[i]}, Available Slots: {JsonConvert.SerializeObject(interviewerSlot)};";
                                            response.resultBody = _utilRepository.UpsertDictionary(dic, $"Interviewer: {obj.InterviewersQueryParam[i]}", $"Available Slots: {JsonConvert.SerializeObject(interviewerSlot)}");
                                        }                                        
                                    }
                                    else
                                    {
                                        response.ErrorMesage += $"Interviewer: {obj.InterviewersQueryParam[i]} does not exist or does not have an available slot for the candidate;";
                                        response.resultBody = _utilRepository.UpsertDictionary(dic, $"Interviewer: {obj.InterviewersQueryParam[i]}", $"does not exist or does not have an available slot for the candidate");
                                    }                                    
                                }
                                catch (Exception ex)
                                {
                                    response.ErrorMesage += $"Interviewer: {obj.InterviewersQueryParam[i]} returned error trying to get the slots: {ex.Message};";
                                    response.resultBody = _utilRepository.UpsertDictionary(dic, $"Interviewer: {obj.InterviewersQueryParam[i]}", $"returned error trying to get the slots: {ex.Message}");
                                    Log.Error($"Interviewer: {obj.InterviewersQueryParam[i]} returned error trying to get the slots: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            response.ErrorMesage += $"Invalid Interviewer data.";
                            response.resultBody = _utilRepository.UpsertDictionary(dic, $"Interviewer", $"Invalid Interviewer data;");
                            Log.Error($"Invalid Interviewer data.");
                        }
                    }
                    else
                    {
                        response.ErrorMesage += $"Invalid Candidate. Candidate does not exists in the database;";
                        response.resultBody = null;
                        Log.Error($"Invalid Candidate. Candidate does not exists in the database");
                    }
                }

                response.DateTimeOfResult = DateTime.Now;
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}");
                response.Sucess = false;
                response.SuccessMessage = null;
                response.ErrorMesage = $"Error: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack: {ex.StackTrace}";
                response.resultBody = null;
            }

            Log.Information($"{DateTime.Now:HH:mm:ss} - Slot Retrieval Request Processed");

            return response;
        }
    }
}
