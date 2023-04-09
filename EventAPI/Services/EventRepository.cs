using EventAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTO;

namespace EventAPI.Services
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _dataContext;

        public EventRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Adds a new event to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> AddEventAsync(Event model)
        {
            if (model != null)
            {
                await _dataContext.Events.AddAsync(model);
                await _dataContext.SaveChangesAsync();
                return new ServiceResponse<Event> { Message = "Event Added.", Success = true, Data = model };
            }
            else if (model != null && await CheckIfEventExists(model))
            {
                return new ServiceResponse<Event> { Message = "Event already exists.", Success = false, Data = Event.NotFound };
            }
            return new ServiceResponse<Event> { Message = "Unable to add event.", Success = false, Data = Event.NotFound };
        }

        /// <summary>
        /// Checks if an event exists in the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfEventExists(Event model)
        {
            if (model != null)
            {
                return await _dataContext.Events.AnyAsync(e => e.Id == model.Id || e.Name == model.Name);
            }
            // if its null, we ain't finding that event.
            return false;
        }

        /// <summary>
        /// Deletes an event from the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> DeleteEventAsync(Event model)
        {
            var modelToDelete = await _dataContext.Events.FirstOrDefaultAsync(e => e.Id == model.Id);
            if (modelToDelete != null)
            {
                _dataContext.Events.Remove(modelToDelete);
                await _dataContext.SaveChangesAsync();
                return new ServiceResponse<Event> { Message = "Event Deleted.", Success = true, Data = model };
            }
            return new ServiceResponse<Event> { Message = "Unable to delete event.", Success = false, Data = Event.NotFound };
        }

        /// <summary>
        /// Gets all events from the DB.
        /// </summary>
        /// <returns>A list of all events in the system</returns>
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _dataContext.Events.ToListAsync();
        }

        /// <summary>
        /// Get an event from the DB. Using either Id or Name.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Event> GetEvent(int id, string name = "")
        {
            var eventModel = await _dataContext.Events.FirstOrDefaultAsync(e => e.Id == id || e.Name == name);
            if (eventModel != null)
            {
                return eventModel;
            }
            return Event.NotFound;
        }

        /// <summary>
        /// Gets an event from the DB, using the event ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventModel = await _dataContext.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (eventModel != null)
            {
                return eventModel;
            }
            return Event.NotFound;
        }

        /// <summary>
        /// Gets an event from the DB, using the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Event> GetEventByNameAsync(string name)
        {
            // TODO adjust to be used for search
            var eventModel = await _dataContext.Events.FirstOrDefaultAsync(e => e.Name == name);
            if (eventModel != null)
            {
                return eventModel;
            }
            return Event.NotFound;
        }

        /// <summary>
        /// Update an event in the DB.
        /// Care must be taken to avoid wrongly updating price and date.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> UpdateEventAsync(EventDTO model)
        {
            var eventModel = await GetEvent(model.Id, model.Name);

            if (eventModel == Event.NotFound || eventModel == null)
            {
                return new ServiceResponse<Event> { Message = "Event not found.", Success = false, Data = Event.NotFound };
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                eventModel.Name = model.Name;
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                eventModel.Description = model.Description;
            }
            // care must be taken when using this method
            eventModel.Price = model.Price;
            eventModel.Date = model.Date;

            // Save changes to the database
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<Event> { Message = "Event updated successfully.", Success = true, Data = eventModel };

        }
    }
}
