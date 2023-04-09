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

        /* TODO:
         * Consider adding a method to the interface that allows you to 
         * retrieve all events that match a given set of criteria. 
         * This can be useful if you want to retrieve all events 
         * that occurred in a particular year, or that have a '
         * certain price range, for example.
         * Would be the GetEvent method, with more properties.
         */
    }
}
