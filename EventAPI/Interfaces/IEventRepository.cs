using Shared;
using Shared.Repository;

namespace EventAPI.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<List<Event>> GetEventByNameAsync(string name);
        Task<bool> CheckIfEventExists(string id);

        /* TODO:
         * Consider adding a method to the interface that allows you to 
         * retrieve all events that match a given set of criteria. 
         * This can be useful if you want to retrieve all events 
         * that occurred in a particular year, or that have a '
         * certain price range, for example.
         * Would be the GetEventByName method, with more properties.
         */
    }
}
