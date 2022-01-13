using AutoMapper;
using Book_Store_Web_Api.Data;
using Book_Store_Web_Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_Web_Api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;

        public BookRepository(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            var books = await _db.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(books);
        }
        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            //WITHOUT AUTO MAPPER (Not good solution when having 12420423 records!!)
            /*var book = await _db.Books.Where(x => x.Id == bookId).Select(x => new BookModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).FirstOrDefaultAsync();
            return book;*/
            var book = await _db.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book);
        }
        public async Task<int> AddBookAsync(BookModel book)
        {
            var newBook = new Books
            {
                Title = book.Title,
                Description = book.Description
            };
            await _db.Books.AddAsync(newBook);
            await _db.SaveChangesAsync();
            return newBook.Id;
        }
        public async Task UpdateBookAsync(int id, BookModel bookModel)
        {
            /*
             * METHOD 1 (Two times access to the database)
            var book = await _db.Books.FindAsync(id);
            if (book != null)
            {
                book.Title = bookModel.Title;
                book.Description = bookModel.Description;
                await _db.SaveChangesAsync();
            }*/

            var book = new Books
            {
                Id = id,
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateBookAsyncPatch(int id, JsonPatchDocument book)
        {
            var b = await _db.Books.FindAsync(id);
            if(b != null)
            {
                book.ApplyTo(b);
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteBookAsync(int id)
        {
            var b = await _db.Books.FindAsync(id);
            //var b = new Books { Id = id };
            if (b != null)
            {
                _db.Books.Remove(b);
                await _db.SaveChangesAsync();
            }
        }
    }
}
