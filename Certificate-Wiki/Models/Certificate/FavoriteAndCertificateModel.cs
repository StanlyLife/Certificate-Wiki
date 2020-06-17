using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Certificate {

	public class FavoriteAndCertificateModel {

		public FavoriteAndCertificateModel() {
			counter = 0;
			isFavorite = new List<bool>();
		}

		public IEnumerable<Certificates> certificate { get; set; }

		public List<bool> isFavorite { get; set; }

		public int counter { get; set; }
	}
}