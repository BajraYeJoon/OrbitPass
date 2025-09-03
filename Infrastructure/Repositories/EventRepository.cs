using Microsoft.EntityFrameworkCore;
using OrbitPass.Core.Entities;
using OrbitPass.Core.Interfaces;
using OrbitPass.Infrastructure.Data;

namespace OrbitPass.Infrastructure.Repositories;

public class EventRepository(ApplicationDbContext dbContext) : IEventRepository
{
    public async Task<Event?> GetEventByIdAsync(int eventId)
    {
        return await dbContext.Events.FindAsync(eventId);
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await dbContext.Events
                .OrderBy(e => e.EventDate)
                .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(int organizerId)
    {
        return await dbContext.Events
            .Where(e => e.OrganizerId == organizerId)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<Event> CreateEventAsync(Event eventEntity)
    {
        dbContext.Events.Add(eventEntity);
        await dbContext.SaveChangesAsync();
        return eventEntity;
    }

    public async Task<Event> UpdateEventAsync(Event eventEntity)
    {
        eventEntity.UpdatedAt = DateTime.UtcNow;
        dbContext.Events.Update(eventEntity);
        await dbContext.SaveChangesAsync();
        return eventEntity;
    }

    public async Task<bool> DeleteEventAsync(int eventId)
    {
        var eventEntity = await GetEventByIdAsync(eventId);
        if (eventEntity == null) return false;

        dbContext.Events.Remove(eventEntity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm)
    {
        return await dbContext.Events
            .Where(e => e.Title.Contains(searchTerm) || e.Location.Contains(searchTerm))
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }


}

