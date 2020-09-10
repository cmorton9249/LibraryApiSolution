using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryApi.Controllers
{
	public class StatusController : ControllerBase
	{
		private readonly ISystemTime _clock;

		public StatusController(ISystemTime clock)
		{
			_clock = clock;
		}

		[HttpPost("employees")]
		public ActionResult Hire([FromBody] EmployeeCreateRequest employeeTohire)
		{
			// 1. Validate
			//  Throw error if bad
			// 2. Add it to a database... or whatever
			var response = new EmployeeResponse
			{
				Id = new Random().Next(10, 100000),
				Name = employeeTohire.Name,
				Department = employeeTohire.Department,
				StartingSalary = employeeTohire.StartingSalary,
				HireDate = DateTime.Now
			};

			// 3. return a 201 Created status code
			// 4. Include in teh response a link to the brand new baby resource (location: http://blah/employees/{id}
			// 5. (usually) just send them a copy of what they would get if they went to that location
			return CreatedAtRoute("employees#getanemployee", new { employeeId = response.Id }, response);
		}


		[HttpGet("whoami")]
		public ActionResult WhoAmI([FromHeader(Name = "User-Agent")] string userAgent)
		{
			return Ok($"I see you are running {userAgent}");
		}

		// GET /employees
		// GET /employees?department=DEV
		[HttpGet("employees")]
		public ActionResult GetAllEmployees([FromQuery]string department = "all")
		{
			return Ok($"all the employees (in {department})");
		}

		// GET /employees/{id}
		[HttpGet("employees/{employeeId:int}", Name = "employees#getanemployee")]
		public ActionResult GetAnEmployee(int employeeId)
		{
			var response = new EmployeeResponse
			{
				Name = "Chris Morton",
				Department = "Developer",
				StartingSalary = 250000
			};
			return Ok(response);
		}


		// GET /status
		[HttpGet("/status")]
		public ActionResult GetStatus()
		{
			var status = new StatusResponse
			{
				Message = "Looks good on my end.  Party on.",
				CheckedBy = "Joe Schmidt",
				WhenChecked = _clock.GetCurrent()
			};
			return Ok(status);
		}
	}

	public class StatusResponse
	{
		public string Message { get; set; }
		public string CheckedBy { get; set; }
		public DateTime WhenChecked { get; set; }
	}

	public class EmployeeCreateRequest
	{
		public string Name { get; set; }
		public string Department { get; set; }
		public decimal StartingSalary { get; set; }
	}

	public class EmployeeResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Department { get; set; }
		public decimal StartingSalary { get; set; }
		public DateTime HireDate { get; set; }
	}
}
