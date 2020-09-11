using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
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
		[HttpGet("books/{bookId:int}", Name = "books#getbookbyid")]
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

		[HttpPost("books")]
		public async Task<ActionResult> AddBook([FromBody] BookCreateRequest bookToAdd)
		{
			// 1. Validate the incoming entity
			if (!ModelState.IsValid)
			{
				// -- if it fails, send back a 400 with or without details.
				return BadRequest(ModelState);
			}

			// 2. change the domain
			// -- add the book to the Database (BookCreateRequest -> Book)
			var book = _mapper.Map<Book>(bookToAdd);
			_context.Books.Add(book);
			await _context.SaveChangesAsync();
			var response = _mapper.Map<GetBookDetailsResponse>(book);
			// 3. Return 
			// -- 201 Created
			// -- Location Header (what the name of the new book is (name being the URI)
			// -- a copy of the book

			return CreatedAtRoute("books#getbookbyid", new { bookId = response.Id }, response);
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

		[HttpPut("books/{bookId:int}/genre")]
		public async Task<ActionResult> UpdateGere(int bookId, [FromBody] string newGenre)
		{
			var book = await _context.Books.Where(b => b.Id == bookId).SingleOrDefaultAsync();

			if (book == null)
			{
				return NotFound();
			}

			book.Genre = newGenre;
			_context.Update(book);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
