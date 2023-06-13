using Shared.ResponseModels;
using Shared;

namespace EventAPI.Interfaces
{
    public interface IEventService
    {
        Task<ServiceResponse<Event>> UpdateEventAsync(EventUpdateDTO model, string eventId);
        Task<ServiceResponse<Event>> DeleteEventAsync(Event model);
    }
}
