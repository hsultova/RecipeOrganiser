using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	///<inheritdoc/>
	public class Repository<T> : IRepository<T>
		where T : BaseModel, new()
	{
		private readonly RecipeOrganiserDbContext _dbContext;

		public Repository(RecipeOrganiserDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected DbSet<T> DbSet => _dbContext.Set<T>();

		///<inheritdoc/>
		public T Get(int id)
		{
			return DbSet.Find(id);
		}

		///<inheritdoc/>
		public T Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = DbSet;
			foreach(var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}

			if(predicate != null)
			{
				query = query.Where(predicate);
			}

			return query.FirstOrDefault();
		}

		///<inheritdoc/>
		public IList<T> GetAll()
		{
			return DbSet.ToList();
		}

		///<inheritdoc/>
		public IList<T> GetAll(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = DbSet;
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return query.ToList();
		}

		///<inheritdoc/>
		public void Create(T entity)
		{
			DbSet.Add(entity);
		}

		///<inheritdoc/>
		public void Update(T entity)
		{
			DbSet.Attach(entity);
		}

		///<inheritdoc/>
		public void Delete(int id)
		{
			T entityToDelete = DbSet.Find(id);
			EntityEntry entityEntry = _dbContext.Entry(entityToDelete);
			if (entityEntry.State == EntityState.Detached)
			{
				DbSet.Attach(entityToDelete);
			}

			DbSet.Remove(entityToDelete);
		}

		///<inheritdoc/>
		public void SaveChanges()
		{
			_dbContext.SaveChanges();
		}

		///<inheritdoc/>
		public void Reload(T entity) 
		{
			_dbContext.Entry(entity).Reload();
		}
	}
}
