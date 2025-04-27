using LibraryMVC.Models;

namespace LibraryMVC.Services.Interface
{
    public interface IReaderService
    {

        Task CreateAsync(Reader reader);
        Task<bool> DeleteAsync(int Id);
        Task<List<Reader>> GetAllAsync();

        Task<List<Reader>> GetAllActiveAsync();

        Task<Reader> GetReaderById (int Id);

        Task<Reader> EditReader(int Id, Reader updatedReader);

    }
}
