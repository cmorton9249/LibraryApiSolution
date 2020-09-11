using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
	public class BooksController : ControllerBase
	{
		private readonly LibraryDataContext _context;
		private readonly IMapper _mapper;
		private readonly MapperConfiguration _config;

		public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
		{
			_context = context;
			_mapper = mapper;
			_config = config;
		}

		// GET /books - Returns a collection of all our books.  Can filter by genre
		[HttpGet("books")]
		[Produces("application/json")]
		public async Task<ActionResult<GetBookResponse>> GetAllBooks([FromQuery] string genre = "all")
		{
			var result = _context.Books
				.Where(x => x.RemovedFromInventory == false);

			if(genre != "all")
			{
				result = result.Where(b => b.Genre == genre);
			}

			var response = new GetBookResponse
			{
				Genre = genre,
				Data = await result.ProjectTo<GetBooksResponseItem>(_config).ToListAsync(),
				Count = result.Count()
			};

			return Ok(response);
		}

		//Get /books/{id}

		/// <summary>
		/// Retrieve a single book
		/// </summary>
		/// <param name="bookId">The id of the book you wish to retrieve</param>
		/// <returns>A book or a 404</returns>
		[HttpGet("books/{bookId:int}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int bookId)
		{
			var response = await _context.Books
				.Where(b => !b.RemovedFromInventory && b.Id == bookId)
				.ProjectTo<GetBookDetailsResponse>(_config)
				.SingleOrDefaultAsync();

			if (response != null)
			{
				return Ok(response);
			}
			return NotFound();
		}

		[HttpDelete("books/{bookId:int}")]
		public async Task<ActionResult> RemoveBooksFromInventory(int bookId)
		{
			var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId && !b.RemovedFromInventory);

			if (book != null)
			{
				book.RemovedFromInventory = true;
				await _context.SaveChangesAsync();
			}

			return NoContent();
		}
	}
}
