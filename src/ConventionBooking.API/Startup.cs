using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
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
			var origin = "http://localhost:3000"; // SPA

			// Setup API to allow cors from your SPA origin for Authorization header and Content-Type header.
			services.AddCors(options => {
				options.AddDefaultPolicy(
					builder => { builder.WithOrigins(origin).WithHeaders("Authorization").WithHeaders("Content-Type");
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
			services.AddSwaggerGen(options => {
				options.SwaggerDoc("v1", new OpenApiInfo { Title= "Convention Booking API", Version = "v1" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    			{
    			    In = ParameterLocation.Header,
    			    Description = "Please enter a valid token",
    			    Name = "Authorization",
    			    Type = SecuritySchemeType.Http,
    			    BearerFormat = "JWT",
    			    Scheme = JwtBearerDefaults.AuthenticationScheme
    			});
    			options.AddSecurityRequirement(new OpenApiSecurityRequirement
    			{
    			    {
    			        new OpenApiSecurityScheme
    			        {
    			            Reference = new OpenApiReference
    			            {
    			                Type=ReferenceType.SecurityScheme,
    			                Id=JwtBearerDefaults.AuthenticationScheme
    			            }
    			        },
    			        new string[]{}
    			    }
    			});

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
