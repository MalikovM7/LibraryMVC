using LibraryMVC.Models.Enums;

namespace LibraryMVC.Models
{
    public class Borrow
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int ReaderId { get; set; }
        public Reader Reader { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public BorrowStatus Status { get; set; }

    }
}
