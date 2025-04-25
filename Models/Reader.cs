using LibraryMVC.Models.Enums;

namespace LibraryMVC.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Status Status { get; set; }
    }
}
