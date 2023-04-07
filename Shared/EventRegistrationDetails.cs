using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class EventRegistrationDetails
    {
        // setup automapper
        [Required]
        public string UserId { get; set; }
        [Required]
        public string EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string UserEmail { get; set; } = string.Empty;
    }
}
