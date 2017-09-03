namespace WeddingsPlanner.Import.DTOs
{
    using Models.Enums;
    using System;

    public class PersonDto
    {
        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}