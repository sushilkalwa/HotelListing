using HotelListing.Models;

namespace HotelListing.Services
{
	public class AuthManager : IAuthManager
	{
		public Task<string> CreateToken()
		{
			throw new NotImplementedException();
		}

		public Task<bool> ValidateUser(LoginUserDTO UserDTO)
		{
			throw new NotImplementedException();
		}
	}
}
