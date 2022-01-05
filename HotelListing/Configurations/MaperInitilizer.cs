using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;

namespace HotelListing.Configurations
{
	public class MaperInitilizer: Profile
	{
		public MaperInitilizer()
		{
			CreateMap<Country, CountryDTO>().ReverseMap();
			CreateMap<Country, CreateCountryDTO>().ReverseMap();

			CreateMap<Hotel, HotelDTO>().ReverseMap();
			CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

			CreateMap<ApiUser, UserDTO>().ReverseMap();
		}
	}
}
