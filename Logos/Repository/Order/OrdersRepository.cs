using System.Collections.Generic;
using System.Threading.Tasks;
using Logos.Entities;
using Logos.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Logos.Repository.Orders
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly IMongoCollection<Order> _orderCollection;

		public OrdersRepository()
		{
			var mongoClient = new MongoClient(DatabaseSettings.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.DatabaseName);
			_orderCollection = mongoDatabase.GetCollection<Order>(DatabaseSettings.CollectionNameOrders);
		}

		public async Task<IEnumerable<Order>> GetAll()
		{
			return await _orderCollection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task Add(Order order)
		{
			await _orderCollection.InsertOneAsync(order);
		}

		public async Task Delete(Order order)
		{
			await _orderCollection.DeleteOneAsync(u => u._id == order._id);
		}

		public async Task<Order> Get(ObjectId id)
		{
			return await _orderCollection.Find(u => u._id == id).FirstOrDefaultAsync();
		}

		public async Task Update(Order order)
		{
			await _orderCollection.ReplaceOneAsync(x => x._id == order._id, order);
		}
	}
}

