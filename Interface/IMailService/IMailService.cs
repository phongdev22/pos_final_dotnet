using pos.Helpers.Mailer;

namespace pos.Interface.IMailService
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
