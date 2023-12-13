using EmployeeManager.Application.Infrastructure;
using EmployeeManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;
        private readonly CalendarService _calendarService;

        public CalendarController(EmployeemanagerContext db, CalendarService calendarService)
        {
            _db = db;
            _calendarService = calendarService;
        }

        /// <summary>
        /// GET https://localhost:5001/api/calendar/2022/1
        /// Holt sich einen Monat aus dem CalendarService und fügt bei jedem Tag die gespeicherten
        /// Termine ein. So kann das Frontend einen Monatskalender mit allen Terminen erstellen.
        /// </summary>
        [HttpGet("{year:int}/{month:int}")]
        public IActionResult GetCalendar(int year, int month)
        {
            var calendarDays = _calendarService.GetDaysOfMonthFullWeeks(year, month);
            var firstDay = calendarDays[0].DateTime;
            var lastDay = calendarDays[calendarDays.Length - 1].DateTime.AddDays(1);
            var jobs = _db.Jobs
                .Include(j => j.Customer)
                .Include(j => j.Employee)
                .Where(j => j.DateTime >= firstDay && j.DateTime <= lastDay)
                .ToList();

            var result = calendarDays.GroupJoin(jobs, c => c.DateTime, j => j.DateTime.Date, (day, jobs) => new
            {
                day.JsTimestamp,
                day.DateTime,
                day.Day,
                day.Month,
                day.Year,
                day.IsWorkingDayMoFr,
                day.IsPublicHoliday,
                day.SchoolHolidayName,
                day.PublicHolidayName,
                jobs = jobs.Select(j => new
                {
                    j.Guid,
                    j.DateTime,
                    TimeFormatted = j.DateTime.ToString("HH:mm"),
                    CustomerGuid = j.Customer.Guid,
                    CustomerName = j.Customer.Name,
                    IsAssigned = j.Employee is not null,
                    EmployeeGuid = j.Employee?.Guid,
                    EmployeeFirstname = j.Employee?.Firstname,
                    EmployeeLastname = j.Employee?.Lastname
                })
            })
            .ToList();
            return Ok(result);
        }
    }
}
