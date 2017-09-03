namespace _2.AdvancedMapping.DTOs
{
    using System.Collections.Generic;

    internal class ManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int SubordinatesCount { get; set; }

        public ICollection<EmployeeDTO> Subordinates { get; set; }
    }
}