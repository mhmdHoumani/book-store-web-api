using Book_Store_Web_Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book_Store_Web_Api.Repositories
{
    public interface IBookRepository
    {
        public Task<List<BookModel>> GetAllBooksAsync();
        Task<BookModel> GetBookByIdAsync(int bookId);
        Task<int> AddBookAsync(BookModel book);
        Task UpdateBookAsync(int id, BookModel bookModel);
        Task UpdateBookAsyncPatch(int id, JsonPatchDocument book);
        Task DeleteBookAsync(int id);
    }
}
