using AutoMapper;
using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly SignInManager<ApiUser> _signInManager;	
		private readonly ILogger<AccountController> _logger;
		private readonly IMapper _mapper;
		public AccountController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager,
								 ILogger<AccountController> logger, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_mapper = mapper;
		}
	}
}
