using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Claims;

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
		public async Task<IActionResult> registerAsync(RegisterModel model) {
			if (!ModelState.IsValid) {
				Console.WriteLine("modelstate failed"); //Remove
				return View(model);
			}

			var user = await userManager.FindByEmailAsync(model.Email);
			if (user != null) { ModelState.AddModelError("Email", "User Already exists"); return View(model); }

			//Register user
			user = new CertificateUser {
				Id = Guid.NewGuid().ToString(),
				UserName = model.Email, /*Username cannot be null in Identity*/
				Email = model.Email
			};

			var result = await userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded) {
				foreach (var error in result.Errors.ToList())
					ModelState.AddModelError("All", error.Description.ToString());
				return View(model);
			}

			return View("Success");
			//Email confirmation

			//Send link to email
		}

		[Route("Forgotpassword")]
		public IActionResult forgotpassword() {
			return View();
		}
	}
}