using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Certificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Certificate_Wiki.Interface.Implementation {

	public class FavoriteHandler : IFavoriteHandler {
		private readonly CertificateDbContext db;
		private readonly UserManager<CertificateUser> userManager;

		public FavoriteHandler(CertificateDbContext db, UserManager<CertificateUser> userManager) {
			this.db = db;
			this.userManager = userManager;
		}

		public void AddUserFavorite(FavoriteCertificate entity) {
			var result = db.FavoriteCertificates.Add(entity);
			SaveChanges();
		}

		public bool CheckUserFavortite(string userId, int CertificateId) {
			var query = from entity in db.FavoriteCertificates
						where entity.UserId == userId && entity.certificateId == CertificateId
						select entity;
			if (query.First() == null) {
				return false;
			}
			return true;
		}

		public bool DeleteAllUserFavorites(string userId) {
			var query = from entity in db.FavoriteCertificates
						where entity.UserId == userId
						select entity;

			if (query.ToList().Count() < 1) { return false; }

			foreach (var favorite in query) {
				db.FavoriteCertificates.Remove(favorite);
			}
			SaveChanges();
			return true;
		}

		public bool DeleteUserFavorite(string userId, int certificateId) {
			var query = from entity in db.FavoriteCertificates
						where entity.UserId == userId && entity.certificateId == certificateId
						select entity;
			if (query.ToList().Count > 0) {
				db.FavoriteCertificates.Remove(query.First());
				SaveChanges();
				return true;
			}
			return false;
		}

		public List<int> GetFavoritesByUser(string userId) {
			var query = from entity in db.FavoriteCertificates
						where entity.UserId == userId
						select entity;

			List<int> list = new List<int>();
			foreach (var favorite in query) {
				list.Add(favorite.certificateId);
			}
			return list;
		}

		public bool SaveChanges() {
			if (db.SaveChanges() > 0) { return true; }
			return false;
		}

		public void ToggleFavorite(FavoriteCertificate entity) {
			if (CheckUserFavortite(entity.UserId, entity.certificateId)) {
				//remove
				DeleteUserFavorite(entity.UserId, entity.certificateId);
			} else {
				//add
				AddUserFavorite(entity);
			}
		}
	}
}