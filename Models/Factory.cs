using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Factory
    {
        [Key]
        public int FactoryId { get; set; }
        [Required]
        public required string Name { get; set; }

        public string? Address { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
