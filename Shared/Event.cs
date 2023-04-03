using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // one event can have many users attending.
        // however, we shall simply maintain that as a count value.
        // but, in reality, there should be a list of users attending.
        // the count of this collection should give our answer
        public virtual ICollection<User> AttendingUsers { get; set; } = new List<User>();
    }
}
