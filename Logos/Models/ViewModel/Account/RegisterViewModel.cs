using System.ComponentModel.DataAnnotations;

namespace Logos.Models.ViewModel
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "Ім'я")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Прізвище")]
		public string LastName { get; set; }
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }


	}
}
