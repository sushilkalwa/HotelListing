using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Models
{
	public class CreateHotelDTO
	{
		[Required]
		[StringLength(maximumLength: 50, ErrorMessage = "Hotel Name is too long!")]
		public string Name { get; set; }

		[Required]
		[StringLength(maximumLength: 100, ErrorMessage = "Address is too long!")]
		public string Address { get; set; }

		[Required]
		[Range(1, 5)]
		public double Rating { get; set; }

		[Required]
		public int CountryId { get; set; }
	}
	public class HotelDTO:CreateHotelDTO
	{
		public int Id { get; set; }
		public CountryDTO Country { get; set; }
	}
	
}
