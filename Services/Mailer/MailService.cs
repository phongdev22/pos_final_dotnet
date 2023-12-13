﻿using Microsoft.Extensions.Options;
using MimeKit;
using pos.Helpers.Mailer;
using pos.Interface.IMailService;
using System.Net;
using System.Net.Mail;

namespace pos.Services.Mailer
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient(_mailSettings.Server, _mailSettings.Port)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password)
                    })
                    
                }

                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
