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
		public string Title { get; set; }
		public string Author { get; set; }
		public string Genre { get; set; }
	}
}
