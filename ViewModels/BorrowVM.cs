using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.ViewModels
{
    public class BorrowVM
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDueDate { get; set; }

        public string BookTitle { get; set; }
        public string ReaderName { get; set; }
        public bool IsReturned { get; set; }
    }
}
