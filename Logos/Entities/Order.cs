using MongoDB.Bson;

namespace Logos.Entities
{
	public class Order
	{
		public ObjectId _id { get; set; }
		public string IdCourier { get; set; }
		public string Location {  get; set; }
		public string Number {  get; set; }
		public string IdUser { get; set; }
		public string CourierName { get; set; }
		public string UserName { get; set; }

		public List<Dish> Dishes { get; set; }
	}
}
