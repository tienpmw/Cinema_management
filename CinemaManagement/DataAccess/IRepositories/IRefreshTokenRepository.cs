using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
	public interface IRefreshTokenRepository
	{
		void AddRefreshToken(RefreshToken refreshTokenModel);
		RefreshToken? GetRefreshToken(string refreshToken);
		void UpdateRefreshToken(RefreshToken refreshToken);
	}
}
