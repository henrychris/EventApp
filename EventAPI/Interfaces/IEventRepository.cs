using Shared;
using Shared.DTO;

namespace EventAPI.Interfaces
{
    public interface IEventRepository
    {
        Task<ServiceResponse<Event>> AddEventAsync(Event model);
        Task<ServiceResponse<Event>> UpdateEventAsync(EventDTO model);
        Task<ServiceResponse<Event>> DeleteEventAsync(Event model);
        Task<Event> GetEvent(int id, string name = "");
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByNameAsync(string name);
        Task<bool> CheckIfEventExists(Event model);
        Task<List<Event>> GetAllEventsAsync();
    }
}
