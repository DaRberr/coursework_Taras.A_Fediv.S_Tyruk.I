using System.ComponentModel.DataAnnotations;

namespace Logos.Models.ViewModel.Account
{
	public class SettingsViewModel
	{

		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Number { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string PasswordConfirm { get; set; }

		public string StringId { get; set; }
	}
}
