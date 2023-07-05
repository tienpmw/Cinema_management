using DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Utils
{
	public class Util
	{
		public static void SetAuthenticationToken(HttpClient client, HttpContext httpContext)
		{
			if (httpContext.Session.GetString("info") != null)
			{
				UserSignInResponseDTO userInfo = JsonSerializer.Deserialize<UserSignInResponseDTO>(httpContext.Session.GetString("info") ?? "") ?? new UserSignInResponseDTO();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(userInfo.AccessToken);
			}
		}
	}
}
