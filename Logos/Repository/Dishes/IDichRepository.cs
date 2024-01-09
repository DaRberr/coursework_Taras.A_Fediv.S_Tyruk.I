using Logos.Entities;
using MongoDB.Bson;

namespace Logos.Repository.Dishes
{
	public interface IDichRepository
	{
		Task<IEnumerable<Dish>> GetAll();
		Task Add(Dish dish);
		Task Delete(Dish dish);
		Task<Dish> Get(ObjectId id);
		Task Update(Dish dish);
	}
}
