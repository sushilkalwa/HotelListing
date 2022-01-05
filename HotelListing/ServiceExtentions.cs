using HotelListing.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelListing
{
	public static class ServiceExtentions
	{
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);
			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
			builder.AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();
		}

		public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSetting = configuration.GetSection("jwt");
			var key = Environment.GetEnvironmentVariable("KEY");

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o => {
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSetting.GetSection("Issuer").Value,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
				};
			});
		}
	}
}
