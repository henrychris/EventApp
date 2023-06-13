using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class Event
    {
        public static Event NotFound = new() { };

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }

    public class EventRegistrationDetails
    {
        // setup automapper
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string EventId { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string UserEmail { get; set; } = string.Empty;
    }
    public class EventUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
    }

    public class CreateEventDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
