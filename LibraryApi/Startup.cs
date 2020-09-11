using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Profiles;
using LibraryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddScoped<ISystemTime, SystemTime>();
			services.AddDbContext<LibraryDataContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("LibraryDatabase"));
			});

			var mappingConfig = new MapperConfiguration( mc => 
			{
				mc.AddProfile(new Books());
			});
			var mapper = mappingConfig.CreateMapper();

			services.AddSingleton<IMapper>(mapper);
			services.AddSingleton<MapperConfiguration>(mappingConfig);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "Library API",
					Version = "1.0",
					Contact = new Microsoft.OpenApi.Models.OpenApiContact
					{
						Name = "Chris Morton",
						Email = "christopher_r_morton@progressive.com"
					},
					Description = "An API for the BES 100 Class"
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthorization();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API");
				c.RoutePrefix = "";
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
