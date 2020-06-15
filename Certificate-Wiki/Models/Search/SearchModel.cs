using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Search {

	public class SearchModel {

		[BindProperty(SupportsGet = true)]
		public string search { get; set; }

		public IEnumerable<Certificates> certificatesResult { get; set; }

		public int resultPages { get; set; }
	}
}