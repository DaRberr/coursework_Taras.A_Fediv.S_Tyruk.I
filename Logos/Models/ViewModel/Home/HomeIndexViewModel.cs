using Logos.Entities;

namespace Logos.Models.ViewModel.Home
{
	public class HomeIndexViewModel
	{
		public string UserName { get; set; }
		public string Id {  get; set; }
		public List<Dish> Dishes { get; set; }
	}
}
