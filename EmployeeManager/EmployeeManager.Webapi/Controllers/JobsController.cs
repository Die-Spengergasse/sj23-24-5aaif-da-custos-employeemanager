using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;

        public JobsController(EmployeemanagerContext db)
        {
            _db = db;
        }

        // GET: api/Jobs
        [HttpGet]
        public IActionResult GetJobs()
        {
            var jobs = _db.Jobs
                .Include(j => j.Customer)
                .Include(j => j.Employee)
                .ToList();
            return Ok(jobs);
        }

        // POST: api/Jobs
        [HttpPost]
        public IActionResult CreateJob([FromBody] JobDto jobDto)
        {
            if (jobDto == null)
            {
                return BadRequest("JobDto ist null");
            }

            var customer = _db.Customers.Find(jobDto.CustomerId);
            var employee = _db.Employees.Find(jobDto.EmployeeId);

            if (customer == null)
            {
                return BadRequest("Ungültige Kunden-ID");
            }

            if (employee == null)
            {
                return BadRequest("Ungültige Mitarbeiter-ID");
            }

            var job = new Job(jobDto.DateTime, customer, employee);

            _db.Jobs.Add(job);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetJobs), new { id = job.Id }, job);
        }
    }

    public class JobDto
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
