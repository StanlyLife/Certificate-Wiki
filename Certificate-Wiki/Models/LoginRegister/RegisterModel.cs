using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.LoginRegister {

	public class RegisterModel {
		//These variables does not respect the IdentityUser options
		[Required]
		[MinLength(3, ErrorMessage = "Email must be entered")]
		[MaxLength(64, ErrorMessage = "Email must be less than 64 characters")]
		public string Email { get; set; }

		[MinLength(6, ErrorMessage = "Password must excede 5 characters")]
		[MaxLength(64, ErrorMessage = "Password must be less than 64 characters")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Compare("Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}