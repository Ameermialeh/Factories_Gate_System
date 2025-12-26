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

        public int ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public Manager? Manager { get; set; }
    }
}
