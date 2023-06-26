using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Repositories
{
	public class SendMailRepository : ISendMailRepository
	{
		private readonly IConfiguration _configuration;
		public SendMailRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			string mail = _configuration["SendMail:Email"];
			string pw = _configuration["SendMail:Password"];
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw),
			};
			MailMessage mm = new MailMessage(from: mail, to: email);
			mm.Subject = subject;
			mm.Body = message;
			mm.IsBodyHtml = true;
			await client.SendMailAsync(mm);
		}
	}
}
