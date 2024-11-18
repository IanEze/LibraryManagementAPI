using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementAPI.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Collection of books borrowed by the borrower
        public ICollection<Book> Books { get; set; }
            }
}
