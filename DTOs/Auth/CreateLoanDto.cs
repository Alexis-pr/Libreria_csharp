using System.ComponentModel.DataAnnotations;

namespace AdminLibrary.DTOs.Loan;

public class CreateLoanDto
{
    [Required]
    public int BookId { get; set; }

    // Solo lo usa el admin
    public int? UserId { get; set; }
}