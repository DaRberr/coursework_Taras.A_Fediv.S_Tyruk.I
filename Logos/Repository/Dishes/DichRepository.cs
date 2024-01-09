using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Logos.Entities;
using Logos.Settings;

namespace Logos.Repository.Dishes
{
	public class DichRepository : IDichRepository
	{
		private readonly IMongoCollection<Dish> _orderCollection;

		public DichRepository()
		{
			var mongoClient = new MongoClient(DatabaseSettings.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.DatabaseName);
			_orderCollection = mongoDatabase.GetCollection<Dish>(DatabaseSettings.CollectionNameDish);
		}

		public async Task<IEnumerable<Dish>> GetAll()
		{
			return await _orderCollection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task Add(Dish user)
		{
			await _orderCollection.InsertOneAsync(user);
		}

		public async Task Delete(Dish user)
		{
			await _orderCollection.DeleteOneAsync(u => u._id == user._id);
		}

		public async Task<Dish> Get(ObjectId id)
		{
			return await _orderCollection.Find(u => u._id == id).FirstOrDefaultAsync();
		}

		public async Task Update(Dish user)
		{
			await _orderCollection.ReplaceOneAsync(x => x._id == user._id, user);
		}
	}
}
