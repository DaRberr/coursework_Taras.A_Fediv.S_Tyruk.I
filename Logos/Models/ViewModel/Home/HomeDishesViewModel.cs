using Logos.Entities;

namespace Logos.Models.ViewModel.Home
{
	public class HomeDishesViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }
		public User User { get; set; }
		public List<Dish> Dishes { get; set; }
	}
}
