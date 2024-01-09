using Logos.Entities;

namespace Logos.Models.ViewModel.Home
{
	public class HomeProfileViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }
		public List<Order> Orders { get; set; }
	}
}
