using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Certificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Interface {

	public interface IFavoriteHandler {

		public List<int> GetFavoritesByUser(string userId);

		public void ToggleFavorite(FavoriteCertificate entity);
		public void AddUserFavorite(FavoriteCertificate entity);

		public bool CheckUserFavortite(string userId, int CertificateId);

		public bool DeleteUserFavorite(string userId, int certificateId);

		public bool DeleteAllUserFavorites(string userId);

		public bool SaveChanges();
	}
}