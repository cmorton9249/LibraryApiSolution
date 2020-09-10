using LibraryApi;
using LibraryApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LibraryApiIntegrationTests
{
	public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
			builder.ConfigureServices(services =>
			{
				var systemTimeDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISystemTime));

				services.Remove(systemTimeDescriptor);
				services.AddScoped<ISystemTime, FakeTime>();
			});
		}
    }

	public class FakeTime : ISystemTime
	{
		public DateTime GetCurrent()
		{
			return new DateTime(1982, 8, 9, 23, 59, 00);
		}
	}
}
