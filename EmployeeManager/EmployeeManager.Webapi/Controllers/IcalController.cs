using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Webapi.Controllers
{
    // Beispiel: https://localhost:5001/ical/ce439b43-500f-baf7-578d-beac1028d66c
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class IcalController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;
        private readonly CalendarService _calendarService;

        public IcalController(EmployeemanagerContext db, CalendarService calendarService)
        {
            _db = db;
            _calendarService = calendarService;
        }

        [HttpGet("{employeeGuid}")]
        public async Task<IActionResult> GetIcalData(Guid employeeGuid)
        {
            var jobs = await _db.Jobs.Select(j => j.Customer.Name).ToListAsync();
            var data = string.Join(", ", jobs);
            return new ContentResult()
            { 
                Content = data,
                ContentType = "text/plain; charset=UTF-8" 
            };
        }
    }
}
