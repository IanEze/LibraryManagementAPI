using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        // Foreign key for Borrower
       
        public int? BorrowerId { get; set; }

        // Nullable in case no borrower is assigned
        // Navigation property
        public int? AuthorId { get; set; }

        [JsonIgnore]
        public Author? Author { get; set; }

        [JsonIgnore]
        public Borrower? Borrower { get; set; }
    }
}
