using System.ComponentModel.DataAnnotations;

namespace ErrandPay_Test.Shared.DTOs
{
    public class EventDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
