using EmployeeManager.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Webapi.Controllers
{
    public record EditEmployeeCmd(
        Guid Guid,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiger Username")]
        string Username,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiger Vorname")]
        string Firstname,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiger Nachname")]
        string Lastname,
        DateTime Birth) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Birth < DateTime.Now.AddYears(-100) || Birth > DateTime.Now.AddYears(-14))
                yield return new ValidationResult("Ungültiges Geburtsdatum", new string[] { nameof(Birth) });
        }
    };
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;

        public EmployeesController(EmployeemanagerContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _db.Employees
                .OrderBy(e => e.Lastname)
                .ThenBy(e => e.Firstname)
                .Select(e => new EditEmployeeCmd(
                    e.Guid, e.Username,
                    e.Firstname, e.Lastname, e.Birth))
                .ToListAsync();
            return Ok(employees);
        }
        [HttpPut]
        public async Task<IActionResult> EditEmployee(EditEmployeeCmd editEmployeeCmd)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Guid == editEmployeeCmd.Guid);
            if (employee is null) return NotFound();
            employee.Username = editEmployeeCmd.Username;
            employee.Firstname = editEmployeeCmd.Firstname;
            employee.Lastname = editEmployeeCmd.Lastname;
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
    }
}
