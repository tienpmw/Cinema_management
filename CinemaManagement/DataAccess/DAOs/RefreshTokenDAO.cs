using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class RefreshTokenDAO
	{
		private static RefreshTokenDAO instance = null;
		private static readonly object instanceLock = new object();

		public static RefreshTokenDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new RefreshTokenDAO();
					}
				}
				return instance;
			}
		}

		public RefreshToken? GetRefreshToken(string refreshToken)
		{
			return new CinemaContext().RefreshToken.FirstOrDefault(x => x.TokenRefresh == refreshToken);
		}

		public void UpdateRefreshToken(RefreshToken refreshToken)
		{
			using (var context = new CinemaContext())
			{
				try
				{
					refreshToken.IsUsed = true;
					context.Entry(refreshToken).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					context.SaveChanges();
				}
				catch (Exception ex)
				{

					throw new Exception(ex.Message);
				}

			}
		}

		public void AddRefreshToken(RefreshToken refreshTokenModel)
		{
			using (var context = new CinemaContext())
			{
				try
				{
					context.RefreshToken.Add(refreshTokenModel);
					context.SaveChanges();
				}
				catch (Exception ex)
				{

					throw new Exception(ex.Message);
				}

			}
		}

		public void UpdateRevokeOldToken(long userId)
		{
			using (var context = new CinemaContext())
			{
				try
				{
					List<RefreshToken> refreshTokens = context.RefreshToken.Where(x => x.UserId == userId && x.IsUsed != true && x.IsRevoked == false).ToList();
					foreach (var item in refreshTokens)
					{
						item.IsRevoked = true;
					}
					context.UpdateRange(refreshTokens);
					context.SaveChanges();
				}
				catch (Exception ex)
				{

					throw new Exception(ex.Message);
				}

			}
		}
	}
}
