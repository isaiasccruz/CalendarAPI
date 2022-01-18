using Flunt.Notifications;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Domain.Models.Entities
{
    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {
        public List<string> Erros { get; } = new List<string>();

        public ErrorModel(string erro)
        {
            Erros.Add(erro);
        }

        public ErrorModel(IEnumerable<string> erros)
        {
            Erros.AddRange(erros);
        }

        public ErrorModel(IReadOnlyCollection<Notification> notifications)
        {
            foreach (Notification notification in notifications)
            {
                Erros.Add(notification.Message);
            }
        }
    }
}
