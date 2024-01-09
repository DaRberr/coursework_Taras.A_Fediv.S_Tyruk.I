using Logos.Entities;
using Logos.Repositoryes.AccountRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Logos.Models.ViewModel.Admin;
using Logos.Repository.Dishes;
using Logos.Repository.Orders;
using System.Diagnostics.Metrics;

namespace Logos.Controllers
{
	public class AdminController : Controller
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IDichRepository _dichRepository;
		private readonly IOrdersRepository _orderRepository;
		public AdminController(IAccountRepository accountRepository, IDichRepository dichRepository, IOrdersRepository ordersRepository)
		{
			_accountRepository = accountRepository;
			_dichRepository = dichRepository;
			_orderRepository = ordersRepository;
		}
		public async Task<IActionResult> Index()
		{
			AdminViewModel model = new AdminViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnAdmin(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.UserName = currentData.FirstName + " " + currentData.LastName;
				model.Users = (await _accountRepository.GetAll()).ToList();

				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		// DELETING!!!!!!
		public async Task<IActionResult> Index(IFormCollection collection)
		{
			AdminViewModel model = new AdminViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnAdmin(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.UserName = currentData.FirstName + " " + currentData.LastName;
				model.Users = (await _accountRepository.GetAll()).ToList();


				var data = collection["userId"];
				await _accountRepository.Delete(await _accountRepository.Get(ObjectId.Parse(data)));

				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> EditUser(IFormCollection collection)
		{
			var data = collection["userId"];
			var userType = collection["userType"];

			var firstName = collection["FirstName"];
			var lastName = collection["LastName"];
			var email = collection["Email"];
			var password = collection["Password"];

			var userForEdit = await _accountRepository.Get(ObjectId.Parse(data));

			if (!String.IsNullOrEmpty(userType))
			{
				switch (userType)
				{
					case "User":
						userForEdit.TypeUser = TypeUser.Client;
						break;
					case "Courier":
						userForEdit.TypeUser = TypeUser.Courier;
						break;
					case "Admin":
						userForEdit.TypeUser = TypeUser.Admin;
						break;
				}

				userForEdit.FirstName = firstName;
				userForEdit.LastName = lastName;
				userForEdit.Email = email;
				userForEdit.Password = password;

				await _accountRepository.Update(userForEdit);
				return RedirectToAction("Index", "Admin");
			}
			return View(userForEdit);
		}

		public async Task<IActionResult> Dishes()
		{
			AdminViewModel model = new AdminViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnAdmin(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.UserName = currentData.FirstName + " " + currentData.LastName;
				model.Dishes = (await _dichRepository.GetAll()).ToList();

				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Dishes(IFormCollection collection)
		{
			AdminViewModel model = new AdminViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnAdmin(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.UserName = currentData.FirstName + " " + currentData.LastName;

				await _dichRepository.Delete(await _dichRepository.Get(ObjectId.Parse(collection["Id"])));
				model.Dishes = (await _dichRepository.GetAll()).ToList();

				return View(model);
			}

			return RedirectToAction("Index", "Home");

		}
		public IActionResult CreateDishes()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateDishes(Dish dish)
		{
			if (ModelState.IsValid)
			{
				await _dichRepository.Add(dish);
			}
            return RedirectToAction("Dishes");
        }

		[HttpPost]
		public async Task<IActionResult> EditDish(IFormCollection collection)
		{
			var data = collection["Id"];

			var title = collection["Title"];
			var description = collection["Description"];
			var price = collection["Price"];
			var namePhoto = collection["UrlOnPhoto"];

			var dishForEdit = await _dichRepository.Get(ObjectId.Parse(data));

			if (!String.IsNullOrEmpty(title))
			{

				dishForEdit.Title = title;
				dishForEdit.Description = description;
				dishForEdit.Price = Decimal.Parse(price);
				dishForEdit.UrlOnPhoto = namePhoto;

				await _dichRepository.Update(dishForEdit);
				return RedirectToAction("Dishes", "Admin");
			}
			return View(dishForEdit);
		}

		public async Task<IActionResult> Orders()
		{
			AdminViewModel model = new AdminViewModel();
			model.Id = Request.Cookies["Login"];

			if (await CheckOnAdmin(model.Id))
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(model.Id));
				model.UserName = currentData.FirstName + " " + currentData.LastName;
				model.Orders = (await _orderRepository.GetAll()).ToList();

				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteOrder(IFormCollection collection)
		{
			var id = Request.Cookies["Login"];

			if (await CheckOnAdmin(id))
			{
				var idOrder = collection["Id"];
				var order = await _orderRepository.Get(ObjectId.Parse(idOrder));

				if (!String.IsNullOrEmpty(order.IdCourier))
				{
					var courier = await _accountRepository.Get(ObjectId.Parse(order.IdCourier));
					for (int i = 0; i < courier.Orders.Count(); i++)
					{
						if (courier.Orders[i]._id == order._id)
						{
							courier.Orders.RemoveAt(i);
						}
					}

					await _accountRepository.Update(courier);
				}

				var user = await _accountRepository.Get(ObjectId.Parse(order.IdUser));
				for (int i = 0; i < user.Orders.Count(); i++)
				{
					if (user.Orders[i]._id == order._id)
					{
						user.Orders.RemoveAt(i);
					}
				}

				await _accountRepository.Update(user);
				await _orderRepository.Delete(order);
			}

			return RedirectToAction("Orders", "Admin");
		}
		public async Task<bool> CheckOnAdmin(string id)
		{
			if (id != null)
			{
				var currentData = await _accountRepository.Get(ObjectId.Parse(id));

				if (currentData.TypeUser == TypeUser.Admin)
				{
					return true;
				}
			}

			return false;
		}
	}
}
