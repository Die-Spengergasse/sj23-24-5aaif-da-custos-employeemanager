using System;

namespace EmployeeManager.Application.Model
{
    public class Job : IEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Job() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Job(DateTime dateTime, Customer customer, Employee? employee)
        {
            DateTime = dateTime;
            Customer = customer;
            Employee = employee;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime DateTime { get; set; }
        public Customer Customer { get; set; }
        public Employee? Employee { get; set; }
    }
}
