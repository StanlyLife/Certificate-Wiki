using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class AuthController : Controller {
		private readonly UserManager<CertificateUser> userManager;

		public AuthController(UserManager<CertificateUser> UserManager) {
			userManager = UserManager;
		}

		[Route("login")]
		public IActionResult login() {
			return View();
		}

		[HttpGet]
		[Route("Register")]
		public IActionResult register() {
			return View();
		}

		[HttpPost]
		[Route("Register")]
		public IActionResult register(RegisterModel model) {
			if (!ModelState.IsValid) {
				Console.WriteLine("modelstate failed"); //Remove
				return View(model);
			}

			var user = userManager.FindByEmailAsync(model.Email);
			if (user != null) { ModelState.AddModelError("Email", "User Already exists"); return View(model); }

			return View();
		}

		[Route("Forgotpassword")]
		public IActionResult forgotpassword() {
			return View();
		}
	}
}