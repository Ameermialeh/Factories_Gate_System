using FactoriesGateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime OrderDate { get; set; }

        public string Description { get; set; }

        public int CustomerID { get; set; }
    }
}
