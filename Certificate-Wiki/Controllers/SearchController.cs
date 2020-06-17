using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class SearchController : Controller {
		private readonly UserManager<CertificateUser> userManager;
		private readonly ICertificateHandler certificateHandler;
		private readonly IUserSearch userSearch;

		public SearchController(
			UserManager<CertificateUser> userManager,
			ICertificateHandler certificateHandler,
			IUserSearch userSearch) {
			this.userManager = userManager;
			this.certificateHandler = certificateHandler;
			this.userSearch = userSearch;
		}

		//TODO
		//Fix url search bug
		private int resultsPerPage = 2;

		[Route("Search/{page:int?}/{search?}")]
		public IActionResult Index(string search, int? page) {
			SearchModel searchModel = new SearchModel { search = search };
			if (String.IsNullOrWhiteSpace(search)) {
				searchModel = SearchDefaultResult(searchModel, page);
				return View(searchModel);
			}

			searchModel = GetCertificates(searchModel, page);
			return View(searchModel);
		}

		public SearchModel SearchDefaultResult(SearchModel searchModel, int? currentPage) {
			var searchResult = certificateHandler.GetCertificatesPagesResultAmount();
			searchModel.resultPages = (int)Math.Ceiling(Decimal.Divide(searchResult, resultsPerPage));

			if (currentPage.HasValue) {
				searchModel.certificatesResult = certificateHandler.GetCertificatesPages(resultsPerPage, currentPage.Value);
			} else {
				searchModel.certificatesResult = certificateHandler.GetCertificatesPages(resultsPerPage, 1);
			}
			return searchModel;
		}

		public SearchModel GetCertificates(SearchModel searchModel, int? page) {
			int currentPage = 1;
			if (page.HasValue) {
				currentPage = page.Value;
			}

			if (searchModel.search.Contains("@")) {
				//TODO
				//Add functionality
				searchModel.certificateUsers = userSearch.SearchUserName(searchModel.search);
				return searchModel;
			}

			int searchResult = certificateHandler.GetCertificatesPagesResultAmount(searchModel.search);
			searchModel.resultPages = (int)Math.Ceiling(Decimal.Divide(searchResult, resultsPerPage));

			searchModel.certificatesResult = certificateHandler.GetCertificatesPages(resultsPerPage, currentPage, searchModel.search);
			return searchModel;
		}
	}
}