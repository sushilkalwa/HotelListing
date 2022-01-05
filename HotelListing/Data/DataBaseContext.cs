using HotelListing.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
	public class DataBaseContext : IdentityDbContext<ApiUser>
	{
		public DataBaseContext(DbContextOptions options):base(options)
		{}
		public DbSet<Country> Countries { get; set; }
		public DbSet<Hotel> Hotels { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new RolesConfiguration());
		}
	}
}
