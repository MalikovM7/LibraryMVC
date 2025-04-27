using LibraryMVC.Models.Enums;

namespace LibraryMVC.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime PublishDate { get; set; }
        public int PageCount { get; set; }
        public Status Status { get; set; }

        public string SuccessMessage { get; set; }

    }
}
