using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class ProfileController : Controller {

		[Route("Profile")]
		public IActionResult Index() {
			return View();
		}

		[Route("Profile/edit")]
		public IActionResult Edit() {
			return View();
		}
	}
}