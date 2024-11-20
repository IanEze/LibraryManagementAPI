using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryManagementAPI.Models
{
    public class Borrower
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Collection of books borrowed by the borrower

        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }
            }
}
