using Certificate_Wiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface {

	public interface ICertificateHandler {

		Certificates GetById(string id);

		IEnumerable<Certificates> GetByUserId(string id);

		Task<CertificateUser> GetAuthorByIdAsync(string id);

		int GetAmountOfCertificates();

		IEnumerable<Certificates> GetCertificatesPages(int amountPerPage, int pageNumber);

		Certificates Create(Certificates certificate);

		Certificates Update(Certificates certificate);

		bool Delete(string id);

		bool SaveChanges();
	}
}