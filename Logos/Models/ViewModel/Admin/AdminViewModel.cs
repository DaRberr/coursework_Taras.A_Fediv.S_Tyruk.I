using Logos.Entities;

namespace Logos.Models.ViewModel.Admin
{
	public class AdminViewModel
	{
		public string Id { get; set; }
		public string UserName { get; set; }

		public List<User> Users { get; set; }
		public List<Order> Orders { get; set; }
		public List<Dish> Dishes { get; set; }
	}
}
