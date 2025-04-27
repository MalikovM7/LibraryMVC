using LibraryMVC.Data;
using LibraryMVC.Models;
using LibraryMVC.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Services.Implementation
{
    public class BookService : IBookService
    {

        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Book book)
        {
           await _context.Books.AddAsync(book);
           await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Book> EditBookAsync(int Id, Book updatedBook)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == Id);
            if (existingBook == null) return null;

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Genre = updatedBook.Genre;
            existingBook.PublishDate = updatedBook.PublishDate;
            existingBook.PageCount = updatedBook.PageCount;
            existingBook.Status = updatedBook.Status;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<Book> GetBookById(int Id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == Id);
        }

        public async Task<List<Book>> ListBooksAsync(string genre = null, string author = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre.ToLower().Contains(genre.ToLower()));

            if (!string.IsNullOrEmpty(author))
                query = query.Where(b => b.Author.ToLower().Contains(author.ToLower()));

            return await query.ToListAsync();
        }
    }
}
