using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Webapi.Controllers
{
    public record AllEmployeeDto(Guid Guid, string Username, string Firstname, string Lastname, DateTime Birth, string Longname);
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

    public record NewEditEmployeeCmd(
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
                .Select(e => new AllEmployeeDto(
                    e.Guid, e.Username,
                    e.Firstname, e.Lastname, e.Birth, $"{e.Lastname} {e.Firstname}"))
                .ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] NewEditEmployeeCmd createEmployeeCmd)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newEmployee = new Employee(createEmployeeCmd.Username, createEmployeeCmd.Firstname, createEmployeeCmd.Lastname, createEmployeeCmd.Birth)
            {
                Guid = Guid.NewGuid()
            };

            _db.Employees.Add(newEmployee);
            await _db.SaveChangesAsync();

            return Ok(new { newEmployee.Guid });
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
            employee.Birth = editEmployeeCmd.Birth;
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
        public async Task<IActionResult> DeleteEmployee(Guid guid)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Guid == guid);
            if (employee == null)
                return NotFound();

            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}


