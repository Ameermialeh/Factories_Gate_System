using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public required string ProductName { get; set; }

        public string? ProductDescription { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        public bool Hide { get; set; } = false;

        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
