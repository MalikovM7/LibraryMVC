using LibraryMVC.Data;
using LibraryMVC.Models;
using LibraryMVC.Models.Enums;
using LibraryMVC.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Services.Implementation
{
    public class ReaderService : IReaderService
    {
        private readonly AppDbContext _context;

        public ReaderService(AppDbContext context)
        {
            _context=context;
        }
        public async Task CreateAsync(Reader reader)
        {
            await _context.AddAsync(reader);
            await _context.SaveChangesAsync();
            
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == Id);
            if (reader == null)
                return false;
            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Reader> EditReader(int Id, Reader updatedReader)
        {
            var existingreader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == Id);
            if (existingreader == null) return null;
            existingreader.FirstName = updatedReader.FirstName;
            existingreader.LastName = updatedReader.LastName;
            existingreader.BirthDate = updatedReader.BirthDate;
            existingreader.RegistrationDate = updatedReader.RegistrationDate;
            existingreader.Status = updatedReader.Status;

            await _context.SaveChangesAsync();
            return existingreader;

        }

        public async Task<List<Reader>> GetAllActiveAsync()
        {
            return await _context.Readers
                .Where(r => r.Status == Status.Active)
                .ToListAsync();
        }

        public async Task<List<Reader>> GetAllAsync()
        {
            return await _context.Readers.ToListAsync();
        }

        public async Task<Reader> GetReaderById(int Id)
        {
           return await _context.Readers.FirstOrDefaultAsync(r =>r.Id == Id);
        }
    }
}
