using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages.Recharge
{
    public class PaymentModel : PageModel
    {
        [BindProperty]
        public long Amount { get; set; }
        [BindProperty]
        public string Code { get; set; }

        private readonly HttpClient client = null;

        public PaymentModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> OnGet(long id, string rawAmount)
        {
            string[] raws = rawAmount.Split(',');
            long amount = long.Parse(string.Join("", raws));
            RechargeRequestDTO data = new RechargeRequestDTO()
            {
                UserId = id,
                Amount= amount
            };

            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var request = await client.PostAsync("http://localhost:5001/api/RechargeRequests", content);

            var respone = await request.Content.ReadAsStringAsync();

            Amount = amount;
            Code = respone.Replace("\"","").Replace("\"","");
            return Page();
        }
    }
}
