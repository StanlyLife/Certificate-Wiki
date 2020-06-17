using Certificate_Wiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface {

	public interface IUserSearch {

		public IEnumerable<CertificateUser> SearchUserName(string userName);
	}
}