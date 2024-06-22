using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Model; // Vergewissere dich, dass dies der richtige Namespace für die Job-Klasse ist
using EmployeeManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class IcalController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;
        private readonly CalendarService _calendarService;

        public IcalController(EmployeemanagerContext db, CalendarService calendarService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        }

        [HttpGet("{employeeGuid}")]
        public async Task<IActionResult> GetIcalData(Guid employeeGuid)
        {
            // Hole die zugewiesenen Jobs des Mitarbeiters
            var jobs = await _db.Jobs
                .Where(j => j.Employee != null && j.Employee.Guid == employeeGuid)
                .Select(j => new {
                    
                    j.DateTime,
                    CustomerName = j.Customer.Name
                })
                .ToListAsync();

            // Erzeuge iCal Daten
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Your Company//Your Product//EN");

            foreach (var job in jobs)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine($"SUMMARY:{job.CustomerName}"); // Ersetze den Ereignisnamen durch den Kundennamen
                sb.AppendLine($"DTSTART:{job.DateTime:yyyyMMddTHHmmssZ}");
                sb.AppendLine($"DTEND:{job.DateTime.AddHours(1):yyyyMMddTHHmmssZ}"); // Angenommen, jedes Ereignis dauert 1 Stunde
                sb.AppendLine($"DESCRIPTION:Job Title: {job.CustomerName}");
                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");

            var icalData = sb.ToString();

            return new ContentResult()
            { 
                Content = icalData,
                ContentType = "text/calendar; charset=UTF-8" 
            };
        }
    }
}
