using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Services
{
	public class AuthManager : IAuthManager
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly IConfiguration _configuration;
		private ApiUser _user;

		public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}
		public async Task<string> CreateToken()
		{
			var siginingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var token = GenerateTokenOtions(siginingCredentials, claims);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private JwtSecurityToken GenerateTokenOtions(SigningCredentials siginingCredentials, List<Claim> claims)
		{
			var jwtSetting = _configuration.GetSection("jwt");
			var expiration = DateTime.Now.AddMinutes(Convert.ToInt32(jwtSetting.GetSection("lifetime").Value));
			var token = new JwtSecurityToken(
				issuer: jwtSetting.GetSection("Issuer").Value,
				claims: claims,
				expires:expiration,
				signingCredentials:siginingCredentials);	
			return token;
		}

		private SigningCredentials GetSigningCredentials()
		{
			var key = "ffc632ce-0053-4bab-8077-93a4d14caaad";
			//var key = Environment.GetEnvironmentVariable("KEY");
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name,_user.UserName)
			};
			var roles = await _userManager.GetRolesAsync(_user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			return claims;
		}

		public async Task<bool> ValidateUser(LoginUserDTO UserDTO)
		{
			_user = await _userManager.FindByNameAsync(UserDTO.Email);
			return (_user != null && await _userManager.CheckPasswordAsync(_user, UserDTO.Password));
		}
		
	}
}
