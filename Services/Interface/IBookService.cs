using LibraryMVC.Models;

namespace LibraryMVC.Services.Interface
{
    public interface IBookService
    {
        Task CreateAsync(Book book);
        Task<bool> DeleteAsync(int Id);
        Task<List<Book>> ListBooksAsync(string genre = null, string author = null);

        Task<Book> GetBookById(int Id);

        Task<Book> EditBookAsync(int Id, Book updatedBook);

    }
}
