using Domain.Entities.Dictionaries;

namespace Domain.Models;

public class BaseBooking
{
    public TicketingProvider Provider { get; set; }
    public string BookingId { get; set; }
}
