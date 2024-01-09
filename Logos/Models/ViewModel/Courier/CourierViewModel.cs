using Logos.Entities;

namespace Logos.Models.ViewModel.Courier
{
    public class CourierViewModel
    {
        public string Id { get; set; }
        public string CourierName { get; set; }
        public List<Order> Orders { get; set; }
    }
}
