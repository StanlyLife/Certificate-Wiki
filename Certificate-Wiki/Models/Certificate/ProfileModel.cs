using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Certificate {

	public class ProfileModel {

		public ProfileModel() {
			facm = new FavoriteAndCertificateModel();
		}
		public FavoriteAndCertificateModel facm { get; set; }

		public CertificateUser profile { get; set; }
		public string profilePicture { get; set; }
	}
}