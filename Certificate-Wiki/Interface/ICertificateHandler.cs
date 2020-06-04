using Certificate_Wiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface {

	public interface ICertificateHandler {

		Certificates GetById(int id);

		IEnumerable<Certificates> GetByUserId(string id);

		Task<CertificateUser> GetAuthorByIdAsync(int id);

		int GetAmountOfCertificates();

		IEnumerable<Certificates> GetCertificatesPages(int amountPerPage, int pageNumber);

		Certificates Create(Certificates certificate);

		Certificates Update(Certificates certificate);

		bool Delete(int id);

		bool SaveChanges();
	}
}