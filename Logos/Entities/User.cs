using MongoDB.Bson;

namespace Logos.Entities
{
	public class User
	{
		public ObjectId _id { get; set; }

		public string GetStringId
		{
			get
			{
				return _id.ToString();
			}
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public TypeUser TypeUser { get; set; }
		public List<Dish> Dishes { get; set; } 
		public List<Order> Orders { get; set; } 
		public User(TypeUser typeUser)
		{
			TypeUser = typeUser;
		}


		public decimal TotalMoney()
		{
			decimal totalMoney = 0;
			for(int i = 0; i < Dishes.Count(); i++)
			{
				totalMoney += Dishes[i].Price;
			}
			return totalMoney;
		}
	}
	public enum TypeUser
	{
		Client,
		Courier,
		Admin
	}
}
