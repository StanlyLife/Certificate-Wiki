using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class SearchController : Controller {

		[Route("Search")]
		public IActionResult Index() {
			return View();
		}
	}
}