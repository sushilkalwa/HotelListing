﻿using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
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
		[HttpGet("{id:int}")]
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
	}
}