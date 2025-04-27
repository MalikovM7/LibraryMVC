using LibraryMVC.Models;

namespace LibraryMVC.Services.Interface
{
    public interface IBorrowService
    {
        
        Task<Borrow> BorrowBookAsync(int bookId, int readerId, DateTime borrowDate, DateTime returnDueDate);

        Task<bool> ReturnBookAsync(int borrowId);
        Task<List<Borrow>> ListCurrentlyBorrowedAsync();

        Task<List<Borrow>> ListBorrowHistoryAsync();

    }
}
