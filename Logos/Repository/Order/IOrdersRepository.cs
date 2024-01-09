using Logos.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Logos.Repository.Orders
{
	public interface IOrdersRepository 
	{
		Task<IEnumerable<Order>> GetAll();
		Task Add(Order order);
		Task Delete(Order order);
		Task<Order> Get(ObjectId id);
		Task Update(Order order);
	}
}
