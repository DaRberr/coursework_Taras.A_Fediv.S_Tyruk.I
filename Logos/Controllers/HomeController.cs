using Logos.Entities;
using Logos.Models;
using Logos.Models.ViewModel.Home;
using Logos.Repository.Dishes;
using Logos.Repository.Orders;
using Logos.Repositoryes.AccountRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Diagnostics;

namespace Logos.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IAccountRepository _accountRepository;
		private readonly IDichRepository _dichRepository;
		private readonly IOrdersRepository _ordersRepository;
		public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository, IDichRepository dichRepository, IOrdersRepository ordersRepository)
		{
			_logger = logger;
			_accountRepository = accountRepository;
			_dichRepository = dichRepository;
			_ordersRepository = ordersRepository;
		}

		public async Task<IActionResult> Index()
		{
			HomeIndexViewModel viewModel = new HomeIndexViewModel();
			viewModel.Id = Request.Cookies["Login"];

			if (viewModel.Id != null)
			{
				CookieOptions options = new CookieOptions();
				options.Expires = DateTime.Now.AddMinutes(15);

				Response.Cookies.Delete("Login");
				Response.Cookies.Append("Login", viewModel.Id, options);

				var data = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));
				viewModel.UserName = data.FirstName + " " + data.LastName;
				viewModel.Dishes = data.Dishes;
			}
			return View(viewModel);
		}

		public async Task<IActionResult> Dishes()
		{
			HomeDishesViewModel viewModel = new HomeDishesViewModel();
			viewModel.Id = Request.Cookies["Login"];


			if (viewModel.Id != null)
			{
				viewModel.User = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));
				viewModel.UserName = $"{viewModel.User.FirstName} {viewModel.User.LastName}";
				viewModel.Dishes = (await _dichRepository.GetAll()).ToList();

				return View(viewModel);
			}
			return RedirectToAction("Register", "Account");
		}
		[HttpPost]
		public async Task<IActionResult> Dishes(IFormCollection collection)
		{
			HomeDishesViewModel viewModel = new HomeDishesViewModel();
			viewModel.Id = Request.Cookies["Login"];

			if (viewModel.Id != null)
			{
				viewModel.User = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));

				var idDish = collection["IdDish"];
				if (!String.IsNullOrEmpty(idDish))
				{
					viewModel.User.Dishes.Add(await _dichRepository.Get(ObjectId.Parse(idDish)));
				}

				_accountRepository.Update(viewModel.User);
				viewModel.UserName = $"{viewModel.User.FirstName} {viewModel.User.LastName}";
				viewModel.Dishes = (await _dichRepository.GetAll()).ToList();

				return RedirectToAction("Dishes", "Home", viewModel);

			}

			return RedirectToAction("Register", "Account");
		}

		[HttpPost]

		public async Task<IActionResult> RemoveDish(IFormCollection collection)
		{
			HomeDishesViewModel viewModel = new HomeDishesViewModel();
			viewModel.Id = Request.Cookies["Login"];

			if (viewModel.Id != null)
			{
				viewModel.User = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));
				viewModel.User.Dishes.RemoveAt(Int32.Parse(collection["Id"]));

				await _accountRepository.Update(viewModel.User);
				viewModel.UserName = $"{viewModel.User.FirstName} {viewModel.User.LastName}";
				viewModel.Dishes = (await _dichRepository.GetAll()).ToList();

				return RedirectToAction("Dishes", "Home", viewModel);
			}

			return RedirectToAction("Register", "Account");
		}

		public async Task<IActionResult> Orders()
		{
			HomeOrderViewModel viewModel = new HomeOrderViewModel();
			var data = Request.Cookies["Login"];
			if (data == null)
			{
				return Redirect("/");
			}


			viewModel.User = await _accountRepository.Get(ObjectId.Parse(data));

			if (viewModel.User.Dishes.Count() == 0)
			{
				return RedirectToAction("Dishes", "Home");
			}
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Orders(IFormCollection collection)
		{
			HomeOrderViewModel viewModel = new HomeOrderViewModel();
			var data = Request.Cookies["Login"];
			if (data == null)
			{
				return Redirect("/");
			}

			viewModel.User = await _accountRepository.Get(ObjectId.Parse(data));

			if (viewModel.User.Dishes.Count() == 0)
			{
				return RedirectToAction("Dishes", "Home");
			}

			Order order = new Order()
			{
				Dishes = viewModel.User.Dishes,
				Location = collection["Location"],
				Number = collection["Number"],
				IdUser = viewModel.User.GetStringId,
				IdCourier = "",
				CourierName = "",
				UserName = $"{viewModel.User.FirstName} {viewModel.User.LastName}"
			};

			await _ordersRepository.Add(order);

			viewModel.User.Orders.Add(order);
			viewModel.User.Dishes.Clear();

			await _accountRepository.Update(viewModel.User);



			return Redirect("/");

		}
		public IActionResult Exit()
		{
			Response.Cookies.Delete("Login");

			return Redirect("/");
		}
		public async Task<IActionResult> Profile()
		{
			HomeProfileViewModel viewModel = new HomeProfileViewModel();
			viewModel.Id = Request.Cookies["Login"];

			if (viewModel.Id == null)
			{
				return View();
			}

			var data = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));

			if (data.TypeUser == TypeUser.Admin)
			{
				return RedirectToAction("Index", "Admin");
			}
			else if (data.TypeUser == TypeUser.Courier)
			{
				return RedirectToAction("Index", "Courier");
			}
			viewModel.UserName = $"{data.FirstName} {data.LastName}";
			viewModel.Orders = data.Orders;

			return View(viewModel);

		}
		public async Task<IActionResult> Settings()
		{
			HomeSettingsViewModel viewModel = new HomeSettingsViewModel();
			viewModel.Id = Request.Cookies["Login"];

			if (viewModel.Id == null)
			{
				return View();
			}

			var data = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));

			if (data.TypeUser == TypeUser.Admin)
			{
				return RedirectToAction("Index", "Admin");
			}
			else if (data.TypeUser == TypeUser.Courier)
			{
				return RedirectToAction("Index", "Courier");
			}
			viewModel.UserName = $"{data.FirstName} {data.LastName}";

			viewModel.FirstName = data.FirstName;
			viewModel.LastName = data.LastName;
			viewModel.Email = data.Email;
			viewModel.Password = data.Password;

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeCurrentData(HomeSettingsViewModel viewModel)
		{
			var currentData = await _accountRepository.Get(ObjectId.Parse(viewModel.Id));

			if (currentData == null)
			{
				return RedirectToAction("Settings", "Home", viewModel);
			}

			currentData.FirstName = viewModel.FirstName;
			currentData.LastName = viewModel.LastName;
			currentData.Password = viewModel.Password;
			await _accountRepository.Update(currentData);


			return RedirectToAction("Profile", "Home");
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}