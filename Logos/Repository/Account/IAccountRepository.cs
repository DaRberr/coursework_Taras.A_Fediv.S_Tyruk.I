using Logos.Entities;
using MongoDB.Bson;

namespace Logos.Repositoryes.AccountRepository
{
	public interface IAccountRepository
	{
		Task<IEnumerable<User>> GetAll();
		Task Add(User user);
		Task Delete(User user);
		Task<User> Get(ObjectId id);
		Task Update(User user);
		Task<User> Get(string email);
	}
}
