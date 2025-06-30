using System;


namespace BizPilot.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public int Age => DateOfBirth.HasValue ?
            (int)((DateTime.Now - DateOfBirth.Value).TotalDays / 365.25) : 0;
        public string Gender { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public override string ToString()
        {
            return FirstName;
        }
    }
}
