namespace Evsell.Business.Email.MailBusiness
{
    public class CompanyEmail
    {
        public void SendRegisterCompany(string mailTo, string companyName, string htmlPage)
        {
            using (Email email = new Email())
            {
                htmlPage = htmlPage.Replace("{CompanyName}", $"{companyName} İyi Satışlar");

                email.Send(mailTo, htmlPage);
            }
        }
    }
}