using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.Books
{
	public class GetBookResponse : Collection<GetBooksResponseItem>
	{
		public string Genre { get; set; }
		public int Count { get; internal set; }
	}

	public class GetBooksResponseItem
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
