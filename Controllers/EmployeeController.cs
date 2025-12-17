using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        public EmployeeController(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        //[HttpGet("CreateEmployee")]
        //public IActionResult CreateEmployee([FromBody] EmployeeDTO dto ) {

            //var employee = _employeeRepo.CreateEmployee(dto);


        //}

    }
}
