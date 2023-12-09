using System;

namespace EmployeeManager.Application.Model
{
    public class Employee : IEntity
    {
        public Employee(string firstname, string lastname, DateTime birth)
        {
            Firstname = firstname;
            Lastname = lastname;
            Birth = birth;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birth { get; set; }
    }
}
