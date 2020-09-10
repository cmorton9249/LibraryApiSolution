using LibraryApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
	public class GettingStatus : IClassFixture<WebTestFixture>
	{
		private HttpClient _client;

		public GettingStatus(WebTestFixture factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetsAnOkayStatusCode()
		{
			var response = await _client.GetAsync("/status");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task SerializeAsJson()
		{
			var response = await _client.GetAsync("/status");
			var mediaType = response.Content.Headers.ContentType.MediaType;

			Assert.Equal("application/json", mediaType);
		}
		
		[Fact]
		public async Task HasCorrectData()
		{
			var response = await _client.GetAsync("/status");
			var content = await response.Content.ReadAsAsync<StatusResponse>();

			Assert.Equal("Looks good on my end.  Party on.", content.Message);
			Assert.Equal("Joe Schmidt", content.CheckedBy);
			Assert.Equal(new DateTime(1982, 8, 9, 23, 59, 00), content.WhenChecked);
		}
	}
}
