using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookController> _logger;

        public BookController(AppDbContext context, ILogger<BookController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                var books = await _context.Books.Include(b => b.Author).ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching books: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching book with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book) // Fixed missing method signature error
        {
            try
            {
                // Ensure the associated author exists
                if (book.AuthorId != 0 && !await _context.Authors.AnyAsync(a => a.Id == book.AuthorId))
                {
                    return BadRequest("The specified author does not exist.");
                }

                // Add the new book
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating book: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] Book book) // Added missing [FromBody] for consistency
        {
            if (id != book.Id)
            {
                return BadRequest("Book ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the associated author exists
                if (book.AuthorId != 0 && !await _context.Authors.AnyAsync(a => a.Id == book.AuthorId))
                {
                    return BadRequest("Author does not exist.");
                }

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(b => b.Id == id))
                {
                    return NotFound();
                }

                _logger.LogWarning($"Concurrency conflict occurred while updating book with ID {id}");
                return Conflict("The book was updated by another user.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating book with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting book with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
