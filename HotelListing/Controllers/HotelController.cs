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
	public class HotelController : ControllerBase
	{
		private readonly IUnitOfWork _unitofwork;
		private readonly ILogger<HotelController> _logger;
		private readonly IMapper _mapper;

		public HotelController(IUnitOfWork unitofwork, ILogger<HotelController> logger, IMapper mapper)
		{
			_unitofwork = unitofwork;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetHotels()
		{
			try
			{
				var Hotels = await _unitofwork.Hotels.GetAll();
				var results = _mapper.Map<List<HotelDTO>>(Hotels);
				return Ok(results);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.GetHotels)}");
				return StatusCode(500, "Please try again!");
			}
		}

		[HttpGet("{id:int}", Name = "GetHotel")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetHotel(int id)
		{
			try
			{
				var Hotel = await _unitofwork.Hotels.Get(q=>q.Id==id,new List<string> { "Country" });
				var result = _mapper.Map<List<HotelDTO>>(Hotel);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.GetHotels)}");
				return StatusCode(500, "Please try again!");
			}
		}

		[Authorize(Roles ="Admin")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hoteldto)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid hotel record{nameof(CreateHotel)}");
				return BadRequest(ModelState);
			}

			try
			{
				var hotel = _mapper.Map<Hotel>(hoteldto);
				await _unitofwork.Hotels.Insert(hotel);
				await _unitofwork.Save();
				return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.CreateHotel)}");
				return StatusCode(500, "Please try again!");
			}
		}
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateHotel(int id, [FromHeader] UpdateHotelDTO updateHotelDTO)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid hotel record{nameof(UpdateHotel)}");
				return BadRequest(ModelState);
			}

			try
			{
				var hotel = await _unitofwork.Hotels.Get(q => q.Id == id);
				if (hotel == null || id < 1)
				{
					_logger.LogError($"Invalid hotel record{nameof(UpdateHotel)}");
					return BadRequest(ModelState);
				}
				_mapper.Map(updateHotelDTO, hotel);
				_unitofwork.Hotels.Update(hotel);
				await _unitofwork.Save();
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.UpdateHotel)}");
				return StatusCode(500, "Please try again!");
			}
		}

		[Authorize]
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteHotel(int id)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
				return BadRequest(ModelState);
			}
			try
			{
				var hotel = await _unitofwork.Hotels.Get(q => q.Id == id);
				if (hotel == null)
				{
					_logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
					return BadRequest("Submitted Data is Invalid!");
				}
				await _unitofwork.Hotels.Delete(id);
				await _unitofwork.Save();
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Somethin went wrong in {nameof(HotelController.DeleteHotel)}");
				return StatusCode(500, "Please try again!");
			}
		}
	}
}
