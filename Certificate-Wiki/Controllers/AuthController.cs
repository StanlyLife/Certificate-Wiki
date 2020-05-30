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
using System.Threading;

namespace Certificate_Wiki.Controllers {

	public class AuthController : Controller {
		private readonly UserManager<CertificateUser> userManager;
		private readonly IUserClaimsPrincipalFactory<CertificateUser> claimsPrincipalFactory;
		private readonly SignInManager<CertificateUser> signInManager;

		public AuthController(UserManager<CertificateUser> UserManager, IUserClaimsPrincipalFactory<CertificateUser> ClaimsPrincipalFactory, SignInManager<CertificateUser> signInManager) {
			userManager = UserManager;
			claimsPrincipalFactory = ClaimsPrincipalFactory;
			this.signInManager = signInManager;
		}

		[HttpGet]
		[Route("login")]
		public IActionResult login() {
			return View();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> loginAsync(LoginModel model) {
			if (!ModelState.IsValid) { Console.WriteLine("Modelstate invalid"); return View(model); }

			var user = await userManager.FindByEmailAsync(model.Email);

			if (user == null) {
				ModelState.AddModelError("All", "No user with that email exists");
				return View(model);
			}

			var result = await userManager.CheckPasswordAsync(user, model.Password);
			if (!result) {
				ModelState.AddModelError("All", "Password did not match email");
				return View(model);
			}

			//2FA

			//Add claims
			var claims = new List<Claim> {
			new Claim(ClaimTypes.NameIdentifier, user.Id),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Name, user.UserName)
			};
			var Identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
			await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(Identity));
			//Login

			// ADD lockout
			//var principal = await claimsPrincipalFactory.CreateAsync(user);
			//await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

			Console.WriteLine("Logged in!");
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

			Console.WriteLine("registered!");
			return View();
			//Email confirmation

			//Send link to email
		}

		[Route("Forgotpassword")]
		public IActionResult forgotpassword() {
			return View();
		}
	}
}