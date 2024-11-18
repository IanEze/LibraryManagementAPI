namespace LibraryManagementAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Foreign key for Borrower
        public Author Author { get; set; }
        public int? BorrowerId { get; set; } // Nullable in case no borrower is assigned
        public Borrower Borrower { get; set; } // Navigation property
        public int AuthorId { get; internal set; }
    }
}
