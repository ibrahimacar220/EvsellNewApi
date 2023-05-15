using System.Net.Mail;

namespace Evsell.Business.Email.MailBusiness
{
    public class Email : IDisposable
    {
        MailMessage mailMessage = null;
        SmtpClient smtp = null;
        public void Send(string mailTo, string htmlPage)
        {

            try
            {
                mailMessage = new MailMessage();
                mailMessage.To.Add(mailTo);
                mailMessage.From = new MailAddress("evsellcom@gmail.com");
                mailMessage.Subject = "Evsell";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlPage;

                smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("evsellcom@gmail.com", "xsiberpeiexeropu");
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }

        }

        public void Dispose()
        {
            if (mailMessage != null)
            {
                mailMessage.Dispose();
            }

            if (smtp != null)
            {
                smtp.Dispose();
            }
        }
    }
}
