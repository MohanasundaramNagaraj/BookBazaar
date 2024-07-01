using AutoMapper;
using BookBazaar.Entities;
using BookBazaar.Profiles.Dtos;
using BookBazaar.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBazaar.Areas.Admin.Controllers.Api
{
#pragma warning disable

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork<Book> _Book;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public BooksController(IUnitOfWork<Book> Book, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _Book = Book;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        // GET: / api / Books
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetBooks()
        {
            var Books = await _Book.Entity.GetAllAsync(filter: null, c=>c.Category,
                //a=>a.Author,
                v=>v.Cover);
            return Ok(Books);
        }

        //GET: / api / Books / 5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {

            var Book = await _Book.Entity.GetFirstOrDefaultAsync(p => p.Id == id, includeProperties: "Category,Author,Cover");

            if (Book == null)
                return NotFound();

            return Ok(_mapper.Map<Book, BookDto>(Book));
        }


        // PUT /api/Books/1
        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Upsert(int id, BookDto BookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var BookInDb = await _Book.Entity.GetFirstOrDefaultAsync(p => p.Id == id, includeProperties: "Category,Author,Cover");

            if (BookInDb == null)
                return NotFound();

            var _mappedBook = _mapper.Map(BookDto, BookInDb);

            await _Book.Entity.UpdateAsync(_mappedBook);
            await _Book.CompleteAsync();

            return Ok();
        }


        //GET: / api / Books/ id
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            Book BookInDb = await _Book.Entity.GetFirstOrDefaultAsync(p => p.Id == id);

            if (BookInDb == null)
                return NotFound();

            if (BookInDb.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, BookInDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            await _Book.Entity.DeleteAsync(BookInDb.Id);
            await _Book.CompleteAsync();

            return Ok(BookInDb);
        }
    }
}