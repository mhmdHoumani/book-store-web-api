using Book_Store_Web_Api.Models;
using Book_Store_Web_Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Book_Store_Web_Api.Controllers
{
    [Route("")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository) => _bookRepository = bookRepository;

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById([FromRoute]int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookModel book)
        {
            var bookId = await _bookRepository.AddBookAsync(book);
            book.Id = bookId;
            return CreatedAtAction(nameof(GetBookById),new { id = bookId }, book);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookModel bookModel)
        {
            await _bookRepository.UpdateBookAsync(id, bookModel);

            return AcceptedAtAction(nameof(GetBookById), new { id }, await _bookRepository.GetBookByIdAsync(id));
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateBookPatch([FromRoute] int id, [FromBody] JsonPatchDocument book)
        {
            await _bookRepository.UpdateBookAsyncPatch(id, book);

            return AcceptedAtAction(nameof(GetBookById), new { id }, await _bookRepository.GetBookByIdAsync(id));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            if (await _bookRepository.GetBookByIdAsync(id) == null)
            {
                return NotFound("This book ID doesn't exist in your list.");
            }
            await _bookRepository.DeleteBookAsync(id);
            return Ok($"The Book with the ID: {id} has been removed from your list.");
        }
    }
}
