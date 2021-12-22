using System;
using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly DataBaseContext _context;
		private readonly DbSet<T> _db;

		public GenericRepository(DataBaseContext context)
		{
			_context = context;
			_db=_context.Set<T>();
		}
		public async Task Delete(int Id)
		{
			var entity = await _db.FindAsync(Id);
			_db.Remove(entity);
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			_db.RemoveRange(entities);
		}

		public async Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string> includes = null)
		{
			IQueryable<T> query =_db;
			if(includes!= null)
			{
				foreach(var includeproperty in includes)
				{
					query = query.Include(includeproperty);
				}
			}
			return await query.AsNoTracking().FirstOrDefaultAsync(expression);
		}

		public async Task<IList<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null)
		{
			IQueryable<T> query = _db;
			if (expression != null)
			{
				query= query.Where(expression);
			}
			if (includes != null)
			{
				foreach (var includeproperty in includes)
				{
					query = query.Include(includeproperty);
				}
				
			}
			if(orderby != null)
			{
				query=orderby(query);
			}
			return await query.AsNoTracking().ToArrayAsync();
		}

		public async Task Insert(T entity)
		{
			await _db.AddAsync(entity);
		}

		public async Task InsertRange(IEnumerable<T> entities)
		{
			await _db.AddRangeAsync(entities);
		}

		public void Update(T entity)
		{
			_db.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}
