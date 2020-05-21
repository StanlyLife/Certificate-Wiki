using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class AuthController : Controller {

		public AuthController() {
		}

		[Route("login")]
		public IActionResult login() {
			return View();
		}

		[Route("Register")]
		public IActionResult register() {
			return View();
		}

		[Route("Forgotpassword")]
		public IActionResult forgotpassword() {
			return View();
		}
	}
}