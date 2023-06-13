using EventAPI.Data;
using EventAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace EventAPI.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly DataContext _dataContext;

        public EventRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Checks if an event exists in the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfEventExists(string id)
        {
            return await _dataContext.Events.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets an event from the DB, using the name.
        /// Returns as many events with that name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<Event>> GetEventByNameAsync(string name)
        {
            // TODO adjust to be used for search
            // i.e there are different parameters to search by.
            var eventModel = await _dataContext.Events.Where(e => e.Name.ToLower()
                .Contains(name.ToLower())).ToListAsync();

            if (eventModel != null)
            {
                return eventModel;
            }
            return new List<Event>();
        }
    }
}
