using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<CountryController> _logger;
		private readonly IMapper _mapper;

		public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCountries()
		{
			try
			{
				var countries = await _unitOfWork.Countries.GetAll();
				var results = _mapper.Map<List<CountryDTO>>(countries);
				return Ok(results);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message.ToString(),$"Something is wrong in the {nameof(CountryController.GetCountries)}");
				return StatusCode(500, "Internal Server Error, Please try again.");
			}
		}

		[HttpGet("{id:int}", Name = "GetCountry")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCountry(int id)
		{
			try
			{
				var country = await _unitOfWork.Countries.Get(q=>q.Id==id,new List<string> { "Hotels"});
				var result = _mapper.Map<CountryDTO>(country);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message.ToString(), $"Something is wrong in the {nameof(CountryController.GetCountries)}");
				return StatusCode(500, "Internal Server Error, Please try again.");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid hotel record{nameof(CreateCountry)}");
				return BadRequest(ModelState);
			}
			try
			{
				var country = _mapper.Map<Country>(countryDTO);
				await _unitOfWork.Countries.Insert(country);
				await _unitOfWork.Save();
				return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.CreateHotel)}");
				return StatusCode(500, "Please try again!");
			}
		}
	}
}
