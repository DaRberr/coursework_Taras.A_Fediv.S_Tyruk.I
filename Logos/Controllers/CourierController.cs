using Logos.Entities;
using Logos.Models.ViewModel.Courier;
using Logos.Repository.Orders;
using Logos.Repositoryes.AccountRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Logos.Controllers
{
	public class CourierController : Controller
	{
		private IAccountRepository _accountRepository { get; set; }
		private IOrdersRepository _ordersRepository { get; set; }
		public CourierController(IAccountRepository accountRepository, IOrdersRepository ordersRepository)
        {
            _accountRepository = accountRepository;
			_ordersRepository = ordersRepository;
        }
        public async Task<IActionResult> Index()
		{
			CourierViewModel model = new CourierViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnCourier(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.CourierName = currentData.FirstName + " " + currentData.LastName;
				model.Orders = (await _ordersRepository.GetAll()).ToList();

				return View(model);
			}
			return Redirect("/");
		}

		public async Task<IActionResult> MyOrders()
		{
			CourierViewModel model = new CourierViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnCourier(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.CourierName = currentData.FirstName + " " + currentData.LastName;
				model.Orders = currentData.Orders.ToList();

				return View(model);
			}
			return Redirect("/");
		}

		public async Task<IActionResult> TakeOrder(IFormCollection collection)
		{
			CourierViewModel model = new CourierViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnCourier(model.Id))
			{
				var id = collection["Id"];

				var courier = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.CourierName = courier.FirstName + " " + courier.LastName;
				model.Orders = (await _ordersRepository.GetAll()).ToList();

				var order = await _ordersRepository.Get(ObjectId.Parse(id));
				order.CourierName = $"{courier.FirstName} {courier.LastName}";
				order.IdCourier = courier.GetStringId;
				courier.Orders.Add(order);

				var user = await _accountRepository.Get(ObjectId.Parse(order.IdUser));
				
				for(int i = 0; i < user.Orders.Count(); i++)
				{
					if (user.Orders[i]._id == ObjectId.Parse(id))
					{
						user.Orders[i].IdCourier = courier.GetStringId;
						user.Orders[i].CourierName = courier.FirstName + " " + courier.LastName;
					}
				}

				await _accountRepository.Update(user);
				await _accountRepository.Update(courier);
				await _ordersRepository.Update(order);
				return RedirectToAction("Index", "Courier", model);
			}
			return Redirect("/");
		
		}
		public async Task<IActionResult> FinishOrder(IFormCollection collection)
		{
			CourierViewModel model = new CourierViewModel();
			model.Id = Request.Cookies["Login"];

			if (!await CheckOnCourier(model.Id))
			{
				return Redirect("/");
			}

			var id = collection["Id"];

			var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
			var order = await _ordersRepository.Get(ObjectId.Parse(id));
			var user = await _accountRepository.Get(ObjectId.Parse(order.IdUser));


			for (int i = 0; i < user.Orders.Count(); i++)
			{
				if (user.Orders[i]._id == order._id)
				{
					user.Orders.Remove(user.Orders[i]);

					await _ordersRepository.Delete(order);
				}
			}
			for (int i = 0;i < currentData.Orders.Count(); i++)
			{
				if (currentData.Orders[i]._id == order._id)
				{
					currentData.Orders.Remove(currentData.Orders[i]);
				}
			}
			await _accountRepository.Update(currentData);
			await _accountRepository.Update(user);

			model.Orders = (await _ordersRepository.GetAll()).ToList();
			model.CourierName = currentData.FirstName + " " + currentData.LastName;

			return RedirectToAction("Index", "Courier", model);
		}
		public async Task<bool> CheckOnCourier(string courierId)
		{
			var currentData = await _accountRepository.Get(ObjectId.Parse(courierId));
			if(currentData.TypeUser == TypeUser.Courier) {
				return true;
			}
			return false;
		}
	}
}
