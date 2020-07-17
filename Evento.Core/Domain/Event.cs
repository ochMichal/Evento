using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evento.Core.Domain
{
    public class Event : Entity
    {
        private ISet<Ticket> _tickets = new HashSet<Ticket>();
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public IEnumerable<Ticket> Tickets => _tickets;

        public IEnumerable<Ticket> PurchasedTickets => _tickets.Where(x => x.Purchased);
        public IEnumerable<Ticket> AvaliableTickets => Tickets.Except(PurchasedTickets);
        //public IEnumerable<Ticket> AvaliableTickets => Tickets.Where(x => !x.Purchased);

        protected Event()
        {

        }

        public Event(Guid id, string name, string description, DateTime startDate, DateTime endDate)
        {
            Id = id;
            SetName(name);
            SetDescription(description);
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"Event with id: '{Id}' can not have empty name.");
            }

            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception($"Event with id: '{Id}' can not have empty description.");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTickets(int amout, decimal price)
        {
            var seating = _tickets.Count + 1;
            for (int i = 0; i < amout; i++)
            {
                _tickets.Add(new Ticket(this, seating, price));
                seating++;
            }
        }
    }
}
