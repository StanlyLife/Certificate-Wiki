using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class CertificateController : Controller {
		private ICertificateHandler CertificateHandler;
		private readonly UserManager<CertificateUser> userManager;

		public CertificateController(ICertificateHandler certificateHandler, UserManager<CertificateUser> userManager) {
			this.CertificateHandler = certificateHandler;
			this.userManager = userManager;
		}

		public IActionResult Edit() {
			return View();
		}

		[HttpGet]
		public IActionResult Upload() {
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult Upload(Certificates model) {
			//TODO
			//Change subject to a <select> stored in database
			//Check for file or url
			if (!ModelState.IsValid) { return View(model); }

			model.UserFk = userManager.GetUserId(User);
			CertificateHandler.Create(model);

			return RedirectToAction("user");
		}

		public IActionResult user() {
			return View();
		}
	}
}