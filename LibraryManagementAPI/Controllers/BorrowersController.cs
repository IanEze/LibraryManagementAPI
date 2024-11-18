using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BorrowerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Borrower
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers()
        {
            // Fetch borrowers with their associated books
            return await _context.Borrowers
                                 .Include(b => b.Books)
                                 .ToListAsync();
        }

        // GET: api/Borrower/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Borrower>> GetBorrower(int id)
        {
            // Find borrower with associated books by ID
            var borrower = await _context.Borrowers
                                          .Include(b => b.Books)
                                          .FirstOrDefaultAsync(b => b.Id == id);

            return borrower == null ? NotFound() : borrower;
        }

        // POST: api/Borrower
        [HttpPost]
        public async Task<ActionResult<Borrower>> PostBorrower(Borrower borrower)
        {
            // Add borrower and save changes
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBorrower), new { id = borrower.Id }, borrower);
        }

        // PUT: api/Borrower/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrower(int id, Borrower borrower)
        {
            if (id != borrower.Id)
                return BadRequest("Borrower ID mismatch.");

            _context.Entry(borrower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowerExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Borrower/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);
            if (borrower == null)
                return NotFound();

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if a borrower exists
        private bool BorrowerExists(int id) => _context.Borrowers.Any(b => b.Id == id);
    }
}
