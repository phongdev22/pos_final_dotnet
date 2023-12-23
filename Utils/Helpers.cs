using System.Globalization;
using System.Net.Mail;
using System.Net;

namespace pos.Utils
{
	public class Helpers
	{
		public static Task SendEmail(IConfiguration _configuration, string toEmail, string subject, string content)
		{
			var host = _configuration["MailSettings:Server"];
			var port = Convert.ToInt32(_configuration["MailSettings:Port"]);
			var fromEmail = _configuration["MailSettings:UserName"];
			var password = _configuration["MailSettings:Password"];
			var senderName = _configuration["MailSettings:SenderName"];

			var client = new SmtpClient(host, port)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(fromEmail, password)
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(fromEmail, senderName),
				Subject = subject,
				Body = content,
				IsBodyHtml = true,
				Priority = MailPriority.High
			};

			mailMessage.To.Add(toEmail);

			return client.SendMailAsync(mailMessage);
		}

		public static void ProcessUpload(IFormFile file, string fileName, string savePath)
		{
			if (file == null || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(savePath))
			{
				throw new ArgumentNullException("file, fileName, and savePath cannot be null or empty");
			}
			var filePath = Path.Combine(savePath, fileName);

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}
		}
	}
}
