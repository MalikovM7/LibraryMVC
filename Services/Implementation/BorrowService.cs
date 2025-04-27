using LibraryMVC.Data;
using LibraryMVC.Models;
using LibraryMVC.Models.Enums;
using LibraryMVC.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Services.Implementation
{
    public class BorrowService : IBorrowService
    {
        private readonly AppDbContext _context;

        public BorrowService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Borrow> BorrowBookAsync(int bookId, int readerId, DateTime borrowedDate, DateTime returnDueDate)
        {
            var overlappingBorrow = await _context.Borrows
                .Where(b => b.BookId == bookId && b.Status == BorrowStatus.Borrowed)
                .Where(b =>
                    (borrowedDate < b.ReturnDueDate && returnDueDate > b.BorrowedDate)
                )
                .FirstOrDefaultAsync();

            if (overlappingBorrow != null)
            {
                throw new InvalidOperationException($"This book is already borrowed from {overlappingBorrow.BorrowedDate:yyyy-MM-dd} to {overlappingBorrow.ReturnDueDate:yyyy-MM-dd}.");
            }

            var borrow = new Borrow
            {
                BookId = bookId,
                ReaderId = readerId,
                BorrowedDate = borrowedDate,
                ReturnDueDate = returnDueDate,
                Status = BorrowStatus.Borrowed
            };

            _context.Borrows.Add(borrow);

            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.Status = Status.Inactive;
            }

            await _context.SaveChangesAsync();
            return borrow;
        }


        public async Task<bool> ReturnBookAsync(int borrowId)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == borrowId);

            if (borrow == null || borrow.Status == BorrowStatus.Returned)
                return false;

            borrow.ReturnedDate = DateTime.Now;
            borrow.Status = BorrowStatus.Returned;

            
            if (borrow.Book != null)
            {
                borrow.Book.Status = Status.Active;
            }

            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<List<Borrow>> ListCurrentlyBorrowedAsync()
        {
            return await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .Where(b => b.Status == BorrowStatus.Borrowed)
                .ToListAsync();
        }

        
        public async Task<List<Borrow>> ListBorrowHistoryAsync()
        {
            return await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .OrderByDescending(b => b.BorrowedDate)
                .ToListAsync();
        }
    }
}
