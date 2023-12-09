using System;

namespace EmployeeManager.Application.Model
{
    public class Customer : IEntity
    {
        public Customer(string name, string zip, string city, string street)
        {
            Name = name;
            Zip = zip;
            City = city;
            Street = street;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
