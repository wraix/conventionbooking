using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

namespace ConventionBooking
{
    public class Startup
    {

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

    	public void ConfigureServices(IServiceCollection services)
    	{
			var issuer = "https://dev-z1zwwud554psgmmw.eu.auth0.com/";
			var audience = "https://conventionbooking.local";
			var origin = "http://localhost:4040";

			services.AddCors(options => {
				options.AddDefaultPolicy(
					builder => { builder.WithOrigins(origin).WithHeaders("Authorization");
				});
			});

			services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
			.AddJwtBearer(options => {
				options.Authority = issuer;
				options.Audience = audience;
				options.IncludeErrorDetails = true;
			});

			services.AddAuthorization(options => {
				options.AddPolicy("Participant", policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "permissions" && c.Value == "participant") && c.Issuer == issuer)));
				options.AddPolicy("Talker", policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "permissions" && c.Value == "talker") && c.Issuer == issuer)));
				options.AddPolicy("Convention Administrator", policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "permissions" && c.Value == "convention-administrator") && c.Issuer == issuer)));
			});

			services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title= "Convention Booking API", Version = "v1" });
					// using System.Reflection;
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				app.UseSwagger();
				app.UseSwaggerUI(c => {
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Convention Booking API V1");
					c.RoutePrefix = string.Empty;
				});
			}

			app.UseRouting();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

    }
}
