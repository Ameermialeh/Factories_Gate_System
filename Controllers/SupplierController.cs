using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : Controller
    {
       private readonly SupplierRepo _supplierRepo;
        public SupplierController(SupplierRepo supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

      

    }
}
