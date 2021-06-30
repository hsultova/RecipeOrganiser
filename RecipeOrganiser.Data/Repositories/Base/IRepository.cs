using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	/// <summary>
	/// Represents a repository with the basic CRUD operations.
	/// </summary>
	/// <typeparam name="T">Type of the used model</typeparam>
	public interface IRepository<T> where T : BaseModel, new()
	{
		/// <summary>
		/// Gets a record from the table.
		/// </summary>
		/// <param name="id">Record identificator</param>
		/// <returns>Single record if found, otherwise null.</returns>
		T Get(int id);

		/// <summary>
		/// Gets a record from the table.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition</param>
		/// <param name="includeProperties">Properties to be included</param>
		/// <returns>Single record if found, otherwise null.</returns>
		T Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

		/// <summary>
		/// Gets all records in the table
		/// </summary>
		/// <returns>List of records</returns>
		IList<T> GetAll();

		/// <summary>
		/// Gets all records in the table
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition</param>
		/// <param name="includeProperties">>Properties to be included</param>
		/// <returns>List of records</returns>
		IList<T> GetAll(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

		/// <summary>
		/// Creates a new record in the table
		/// </summary>
		/// <param name="entity">Entity to be created</param>
		void Create(T entity);

		/// <summary>
		/// Updates a record from the table
		/// </summary>
		/// <param name="entity">Entity to be updated</param>
		void Update(T entity);

		/// <summary>
		/// Deletes a record from the table
		/// </summary>
		/// <param name="id">Record identificator</param>
		void Delete(int id);

		/// <summary>
		/// Saves all changes to the database
		/// </summary>
		void SaveChanges();

		/// <summary>
		/// Reloads the entity from the database overwriting any property values with values from the database.
		/// </summary>
		/// <param name="entity">Entity to be updated</param>
		void Reload(T entity);
	}
}
