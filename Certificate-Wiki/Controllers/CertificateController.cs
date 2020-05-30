using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class CertificateController : Controller {

		public IActionResult Edit() {
			return View();
		}

		public IActionResult Upload() {
			return View();
		}
		public IActionResult user() {
			return View();
		}
	}
}