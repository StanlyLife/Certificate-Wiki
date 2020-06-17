using Certificate_Wiki.Data;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface.Implementation {

	public class UserSearch : IUserSearch {
		private readonly CertificateDbContext db;
		private readonly UserManager<CertificateUser> userManager;

		public UserSearch(CertificateDbContext db, UserManager<CertificateUser> userManager) {
			this.db = db;
			this.userManager = userManager;
		}

		public IEnumerable<CertificateUser> SearchUserName(string userName) {
			var query = from entity in db.Users
						where entity.NormalizedUserName.Contains(userName)
						select entity;

			return query;
		}
	}
}