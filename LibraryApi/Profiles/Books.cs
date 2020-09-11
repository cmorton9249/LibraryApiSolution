using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;

namespace LibraryApi.Profiles
{
	public class Books : Profile
	{
		public Books()
		{
			CreateMap<Book, GetBooksResponseItem>();
			CreateMap<Book, GetBookDetailsResponse>();
			CreateMap<BookCreateRequest, Book>()
				.ForMember(dest => dest.RemovedFromInventory, d => d.MapFrom(source => false));
		}
	}
}
