using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> UserAsync(CertificateViewModel model) {
			CertificateViewModel certificates = new CertificateViewModel();
			CertificateUser user = await userManager.FindByNameAsync(User.Identity.Name);
			certificates.certificateList = CertificateHandler.GetByUserId(user.Id);
			return View(certificates);
		}

		[HttpGet]
		public IActionResult Upload() {
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult Upload(Certificates model, [FromForm(Name = "file")] IFormFile file) {
			//TODO
			//Change subject to a <select> stored in database
			//Add modelvalidators
			if (file != null) {
				//file upload
				model.CertificateUrl = null;
				model.CertificateFile = FileToBytes(file);
			} else if (model.CertificateUrl != null) {
				//url upload
				model.CertificateFile = null;
			}

			if (model.CertificateUrl == null && model.CertificateFile == null) {
				ModelState.AddModelError("All", "You must upload or link to a certificate image");
			}

			if (model.CertificateUrl != null && model.CertificateFile != null) {
				ModelState.AddModelError("All", "You cannot upload certificate and link a certificate");
			}

			if (!ModelState.IsValid) { return View(model); }

			model.UserFk = userManager.GetUserId(User);
			CertificateHandler.Create(model);

			return RedirectToAction("user");
		}

		public byte[] FileToBytes(IFormFile file) {
			MemoryStream ms = new MemoryStream();
			file.CopyTo(ms);
			var memoryStream = ms.ToArray();
			ms.Close();
			ms.Dispose();

			return memoryStream;
		}
	}
}