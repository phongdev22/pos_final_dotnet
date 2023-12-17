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

		public static bool IsValidTimestamp(string timestamp, int allowedMinutes = 1)
		{
			DateTime inputDateTime;

			// Try to parse the input timestamp
			if (DateTime.TryParseExact(timestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDateTime))
			{
				// Get the current time
				DateTime currentTime = DateTime.UtcNow;

				// Calculate the difference in time
				TimeSpan difference = currentTime - inputDateTime;

				// Check if the difference is within the allowed time range
				return Math.Abs(difference.TotalMinutes) <= allowedMinutes;
			}

			// Return false if the timestamp couldn't be parsed
			return false;
		}
	}
}
