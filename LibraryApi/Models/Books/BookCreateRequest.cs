using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.Books
{
	public class BookCreateRequest : IValidatableObject
	{
		[Required]
		[MaxLength(200)]
		public string Title { get; set; }
		[Required]
		[MaxLength(100)]
		public string Author { get; set; }
		[Required]
		public string Genre { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Title.ToLower() == "it" && Author.ToLower().Contains("king"))
			{
				yield return new ValidationResult("That book is too darn long!");
			}
		}
	}

	
}
