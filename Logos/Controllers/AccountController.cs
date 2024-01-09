using Logos.Entities;
using Logos.Models.ViewModel;
using Logos.Repositoryes.AccountRepository;
using Microsoft.AspNetCore.Mvc;

namespace Logos.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (   !String.IsNullOrEmpty(viewModel.Email) 
                && !String.IsNullOrEmpty(viewModel.Password) 
                && !String.IsNullOrEmpty(viewModel.PasswordConfirm) 
                && !String.IsNullOrEmpty(viewModel.LastName) 
                && !String.IsNullOrEmpty(viewModel.FirstName))
            {
                if (viewModel.Password == viewModel.PasswordConfirm)
                {
                    User user = new User(TypeUser.Client)
                    {
                        Email = viewModel.Email,
                        Password = viewModel.Password,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Dishes = new List<Dish>(),
                        Orders = new List<Order>()
                        
                    };
                    await _accountRepository.Add(user);

                    var data = await _accountRepository.Get(viewModel.Email);
					CookieOptions cookie = new CookieOptions();

                    cookie.Expires = DateTime.Now.AddMinutes(15);

                    Response.Cookies.Append("Login", data.GetStringId, cookie); 
					return Redirect("/");
                }
            }
            return View(viewModel);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var confirmUserData = await _accountRepository.Get(viewModel.Email);

            if(confirmUserData == null) {
                return View(viewModel);
            }
            if (confirmUserData.Password == viewModel.Password)
            {
				CookieOptions cookie = new CookieOptions();

				cookie.Expires = DateTime.Now.AddMinutes(15);
                Response.Cookies.Append("Login", confirmUserData.GetStringId, cookie);

                return Redirect("/");
            }
            return View(viewModel);
        }
    }
}
