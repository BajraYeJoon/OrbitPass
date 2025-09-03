using OrbitPass.Core.Entities;

namespace OrbitPass.Core.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetEventByIdAsync(int eventId);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(int organizerId);
    Task<Event> CreateEventAsync(Event eventEntity);
    Task<Event> UpdateEventAsync(Event eventEntity);
    Task<bool> DeleteEventAsync(int eventId);
    Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm);

}