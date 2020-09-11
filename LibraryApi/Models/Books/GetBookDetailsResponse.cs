using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.Books
{
	public class GetBookDetailsResponse
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(200)]
		public string Title { get; set; }
		[Required]
		[MaxLength(100)]
		public string Author { get; set; }
		public string Genre { get; set; }
	}
}
