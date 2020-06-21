using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.LoginRegister {

	public class ResetPasswordModel {

		[Required(ErrorMessage = "Something went wrong! Cannot find email.")]
		public string email { get; set; }

		[Required(ErrorMessage = "Something went wrong! Cannot find token.")]
		public string token { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("confirmPassword", ErrorMessage = "Passwords does not match")]
		public string password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string confirmPassword { get; set; }
	}
}