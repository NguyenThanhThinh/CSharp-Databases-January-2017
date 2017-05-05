namespace _2.AdvancedMapping.DTOs
{
    using Models;
    using System.Collections.Generic;

    class ManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int SubordinatesCount { get; set; }

        public ICollection<EmployeeDTO> Subordinates { get; set; }
    }
}
