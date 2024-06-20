using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Webapi.Controllers
{
    public record EditCustomerCmd(Guid Guid, string Name, string Zip, string City, string Street);
    public record NewCustomerCmd(string Name, string Zip, string City, string Street);

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;

        public CustomersController(EmployeemanagerContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _db.Customers
                .OrderBy(c => c.Name)
                .Select(c => new EditCustomerCmd(c.Guid, c.Name, c.Zip, c.City, c.Street))
                .ToListAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] NewCustomerCmd newCustomerCmd)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCustomer = new Customer(newCustomerCmd.Name, newCustomerCmd.Zip, newCustomerCmd.City, newCustomerCmd.Street)
            {
                Guid = Guid.NewGuid()
            };

            _db.Customers.Add(newCustomer);
            await _db.SaveChangesAsync();

            return Ok(new { newCustomer.Guid });
        }

        [HttpPut]
        public async Task<IActionResult> EditCustomer(EditCustomerCmd editCustomerCmd)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var customer = await _db.Customers.FirstOrDefaultAsync(e => e.Guid == editCustomerCmd.Guid);
            if (customer is null) return NotFound();
            customer.Name = editCustomerCmd.Name;
            customer.Zip = editCustomerCmd.Zip;
            customer.City = editCustomerCmd.City;
            customer.Street = editCustomerCmd.Street;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }
            return NoContent();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid guid)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Guid == guid);
            if (customer == null)
                return NotFound();

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}