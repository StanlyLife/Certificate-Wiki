using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class ProfileController : Controller {
		private readonly UserManager<CertificateUser> userManager;
		private readonly SignInManager<CertificateUser> signInManager;

		public ProfileController(UserManager<CertificateUser> userManager, SignInManager<CertificateUser> signInManager) {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[Route("Profile")]
		public IActionResult Index() {
			return View();
		}

		[HttpGet]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync() {
			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			if (Profile == null) { return View(); }
			return View(Profile);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync([FromForm]CertificateUser model) {
			if (!ModelState.IsValid) { return View(model); }

			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			if (Profile == null) { return View(); }
			//Update database
			Profile.FirstName = model.FirstName;
			Profile.LastName = model.LastName;
			Profile.Description = model.Description;
			Profile.Country = model.Country;
			Profile.Occupation = model.Occupation;
			Profile.Website = model.Website;
			Profile.isPrivate = model.isPrivate;
			await userManager.UpdateAsync(Profile);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAccountAsync() {
			var user = await userManager.FindByNameAsync(User.Identity.Name);
			await userManager.DeleteAsync(user);
			await signInManager.SignOutAsync();
			return RedirectToAction("index", "home");
			//TODO
			//Delete certificates
		}
	}
}