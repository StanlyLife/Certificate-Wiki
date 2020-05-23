using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.LoginRegister {

	public class LoginModel {

		[NotNull]
		public string Email { get; set; }

		[NotNull]
		public string Password { get; set; }
	}
}