using Shared.ResponseModels;
using Shared;
using EventAPI.Interfaces;
using AutoMapper;

namespace EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new event to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> AddEventAsync(CreateEventDTO model)
        {
            if (model != null)
            {
                // map to Event model
                var eventModel = _mapper.Map<Event>(model);
                await _unitOfWork.Events.AddAsync(eventModel);
                await _unitOfWork.CompleteAsync();
                return new ServiceResponse<Event> { Message = "Event Added.", Success = true, Data = eventModel };
            }
            return new ServiceResponse<Event> { Message = "Unable to add event.", Success = false, Data = Event.NotFound };
        }

        /// <summary>
        /// Update an event in the DB.
        /// Care must be taken to avoid wrongly updating price and date.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> UpdateEventAsync(EventUpdateDTO model, string eventId)
        {
            var eventModel = await _unitOfWork.Events.GetByIdAsync(eventId);

            if (eventModel == null)
                return new ServiceResponse<Event> { Message = "Event not found.", Success = false, Data = Event.NotFound };

            eventModel.Name = model.Name ?? eventModel.Name;
            eventModel.Description = model.Description ?? eventModel.Description;
            eventModel.Price = model.Price ?? eventModel.Price;
            eventModel.Date = model.Date ?? eventModel.Date;

            await _unitOfWork.CompleteAsync();

            return new ServiceResponse<Event> { Message = "Event updated successfully.", Success = true, Data = eventModel };
        }

        /// <summary>
        /// Deletes an event from the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<Event>> DeleteEventAsync(Event model)
        {
            var modelToDelete = await _unitOfWork.Events.GetByIdAsync(model.Id);
            if (modelToDelete != null)
            {
                await _unitOfWork.Events.RemoveAsync(modelToDelete);
                await _unitOfWork.CompleteAsync();
                return new ServiceResponse<Event> { Message = "Event Deleted.", Success = true, Data = model };
            }
            return new ServiceResponse<Event> { Message = "Event not found.", Success = false, Data = Event.NotFound };
        }
    }
}
