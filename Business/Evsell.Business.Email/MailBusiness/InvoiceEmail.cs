namespace Evsell.Business.Email.MailBusiness
{
    public class InvoiceEmail
    {
        public void SendUserPurchaseEmail(string mailTo, List<string> productName, decimal productPrice, string htmlPage)
        {
            List<string> productnamelist = new List<string>();

            using (Email email = new Email())
            {
                foreach (string item in productName)
                {
                    productnamelist.Add(item);
                }

                string name = "";
                productName.ForEach(x =>
                {
                    name += " , " + x.ToString();

                });

                htmlPage = htmlPage.Replace("{ProductName}", $"{name}");
                htmlPage = htmlPage.Replace("{ProductPrice}", $"{productPrice} Ürün Satım Alımınız başarıyla Gerçekleştirmiştir");

                email.Send(mailTo, htmlPage);
            }
        }

        public void SendCompanyPurchaseEmail(string mailTo, List<string> productName, decimal productPrice, string htmlPage)
        {
            List<string> productnamelist = new List<string>();

            using (Email email = new Email())
            {
                foreach (string item in productName)
                {
                    productnamelist.Add(item);
                }

                string name = "";
                productName.ForEach(x =>
                {
                    name += x.ToString() + " , ";

                });

                htmlPage = htmlPage.Replace("{ProductName}", $" Sattığınız Ürünler : {name}");
                htmlPage = htmlPage.Replace("{ProductPrice}", $" Satıştan Kazandığınız Miktar :{productPrice} TL");

                email.Send(mailTo, htmlPage);
            }
        }
    }
}
