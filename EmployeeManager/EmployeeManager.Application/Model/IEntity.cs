using System;

namespace EmployeeManager.Application.Model
{
    public interface IEntity
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
    }
}
