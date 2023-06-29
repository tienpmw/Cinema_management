using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		public void AddRefreshToken(RefreshToken refreshTokenModel) => RefreshTokenDAO.Instance.AddRefreshToken(refreshTokenModel);

		public RefreshToken? GetRefreshToken(string refreshToken) => RefreshTokenDAO.Instance.GetRefreshToken(refreshToken);

		public void UpdateRefreshToken(RefreshToken refreshToken) => RefreshTokenDAO.Instance.UpdateRefreshToken(refreshToken);

		public void UpdateRevokeOldToken(long userId) => RefreshTokenDAO.Instance.UpdateRevokeOldToken(userId);
	}
}
