using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class DevicesDto
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
    }
}
