using Logos.Entities;
using System.ComponentModel.DataAnnotations;

namespace Logos.Models.ViewModel.Home
{
	public class HomeOrderViewModel
	{
		public string Location { get; set; }
		public string Number { get; set; }
		public User User { get; set; }

		public List<Dish> Dishes = new List<Dish>();
	}
}
