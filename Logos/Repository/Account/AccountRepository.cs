using Logos.Entities;
using Logos.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Logos.Repositoryes.AccountRepository
{
	public class AccountRepository : IAccountRepository
	{
		private IMongoCollection<User> _userCollection;

		public AccountRepository()
		{
			var mongoClient = new MongoClient(DatabaseSettings.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.DatabaseName);
			_userCollection = mongoDatabase.GetCollection<User>(DatabaseSettings.CollectionNameUser);
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			// Add async modifier to the method
			return await _userCollection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task Add(User user)
		{
			await _userCollection.InsertOneAsync(user);
		}

		public async Task Delete(User user)
		{
			await _userCollection.DeleteOneAsync(u => u._id == user._id);
		}

		public async Task<User> Get(ObjectId id)
		{
			return await _userCollection.Find(u => u._id == id).FirstOrDefaultAsync();
		}

		public async Task Update(User user)
		{
			await _userCollection.ReplaceOneAsync(x => x._id == user._id, user);
		}

		public async Task<User> Get(string email)
		{
			return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
		}
	}
}
