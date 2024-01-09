using MongoDB.Bson;

namespace Logos.Entities
{
	public class Dish
	{
		public ObjectId _id {  get; set; }
		public string GetStringId
		{
			get { return _id.ToString(); }
		}
		public string Title {  get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string UrlOnPhoto { get; set; }

	}
}
