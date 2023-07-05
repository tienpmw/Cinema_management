using DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInfo.AccessToken) ;
			}
		}

		public static async Task RefreshToken(HttpContext httpContext)
		{
			HttpClient _httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			string UserApi = "http://localhost:5001/api/Users/RefreshToken";
			if (httpContext.Session.GetString("info") != null)
			{
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				UserSignInResponseDTO userInfo = JsonSerializer.Deserialize<UserSignInResponseDTO>(httpContext.Session.GetString("info") ?? "", options) ?? new UserSignInResponseDTO();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(userInfo.AccessToken ?? "");
				RefreshTokenRequestDTO rf = new RefreshTokenRequestDTO
				{
					AccessToken = userInfo.AccessToken,
					RefreshToken = userInfo.RefreshToken
				};
				var content = new StringContent(JsonSerializer.Serialize(rf), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _httpClient.PostAsync(UserApi, content);
				if (response.IsSuccessStatusCode)
				{
					httpContext.Session.SetString("info", await response.Content.ReadAsStringAsync());
				}
			}
		}

		public static async Task<string> GetAccessToken(HttpContext httpContext)
		{
			await RefreshToken(httpContext);
			if (httpContext.Session.GetString("info") != null)
			{
				UserSignInResponseDTO? userInfo = JsonSerializer.Deserialize<UserSignInResponseDTO>(httpContext.Session.GetString("info") ?? "");
				string token = "Bearer " + (userInfo?.AccessToken ?? "");
				return token;
			}
			else
			{
				return "";
			}

		}
	}
}
