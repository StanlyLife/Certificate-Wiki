using Certificate_Wiki.Data;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface.Implementation {

	public class CertificateHandler : ICertificateHandler {
		public readonly CertificateDbContext db;
		public readonly UserManager<CertificateUser> userManager;

		public CertificateHandler(CertificateDbContext db, UserManager<CertificateUser> userManager) {
			this.db = db;
			this.userManager = userManager;
		}

		public Certificates Create(Certificates certificate) {
			db.CertificateContext.Add(certificate);
			SaveChanges();
			return certificate;
		}

		public bool Delete(int id) {
			var status = db.CertificateContext.Remove(GetById(id));
			return SaveChanges();
		}

		public int GetAmountOfCertificates() {
			return db.CertificateContext.Where(certificate => certificate.UserFk != null).Count();
		}

		public async Task<CertificateUser> GetAuthorByIdAsync(int id) {
			var query = from entity in db.CertificateContext
						where entity.CertificateId == id
						select entity.UserFk;

			Console.WriteLine("Query: ");
			Console.WriteLine(query.FirstOrDefault());
			CertificateUser user = await userManager.FindByIdAsync(query.FirstOrDefault());
			return user;
		}

		public Certificates GetById(int id) {
			return db.CertificateContext.Find(id);
		}

		public IEnumerable<Certificates> GetByUserId(string id) {
			var query = from entity in db.CertificateContext
						where entity.UserFk.Equals(id)
						orderby entity.CertificateId
						select entity;
			return query;
		}

		public IEnumerable<Certificates> GetCertificatesPages(int amountPerPage, int pageNumber) {
			if (pageNumber == 1) {
				var PageOne = from entity in db.CertificateContext
							  where entity.CertificateId < amountPerPage
							  select entity;
				return PageOne;
			}
			//The series are:
			//((PageNum * AmountPerPage) - AmountPerPage) < certificate < (PageNum * AmountPerPage)
			var query = from entity in db.CertificateContext
						where entity.CertificateId > ((pageNumber * amountPerPage) - pageNumber) && entity.CertificateId < (pageNumber * pageNumber)
						orderby entity.CertificateId
						select entity;
			return query;
		}

		public bool SaveChanges() {
			if (db.SaveChanges() > 0) { return true; }
			return false;
		}

		public Certificates Update(Certificates certificate) {
			var entity = db.CertificateContext.Attach(certificate);
			entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			SaveChanges();

			return certificate;
		}
	}
}