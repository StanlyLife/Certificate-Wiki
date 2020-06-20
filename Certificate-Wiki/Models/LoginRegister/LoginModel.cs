using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.LoginRegister {

	public class LoginModel {

		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}