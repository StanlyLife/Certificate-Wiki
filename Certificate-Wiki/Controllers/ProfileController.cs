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

		public ProfileController(UserManager<CertificateUser> userManager) {
			this.userManager = userManager;
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
			//TODO
			//Remove CW from single-line if
			if (!ModelState.IsValid) { Console.WriteLine("Modelstate invalid"); return View(model); }

			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			if (Profile == null) { return View(); }
			//Update database
			Profile.FirstName = model.FirstName;
			Profile.LastName = model.LastName;
			Profile.Description = model.Description;
			Profile.Country = model.Country;
			Profile.Occupation = model.Occupation;
			Profile.Website = model.Website;
			await userManager.UpdateAsync(Profile);

			Console.WriteLine("Update success");

			return RedirectToAction("Index");
		}
	}
}