using Evsell.Business.Common.Response;
using Evsell.Business.Email.MailBusiness;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Dtos;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Business
{
    public class InvoiceBusiness : IInvoiceBusiness
    {
        readonly EvsellDbContext dbContext;

        public InvoiceBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto Save(int buyerId)
        {
            List<Basket> basket = null;

            InvoiceEmail mailSender = new InvoiceEmail();

            User buyerUser = null;
            Company sellerCompany = null;
            User sellerUser = null;

            List<string> mailProductNameList = new List<string>();
            List<InvoiceProduct> productList = new List<InvoiceProduct>();
            InvoiceProduct invoiceProduct = null;
            InvoiceProductDto properties = null;
            Invoice invoice = null;
            Product product = null;

            try
            {
                basket = dbContext.Baskets.Where(basket => basket.UserId == buyerId).ToList();

                decimal invoiceTotal = 0;
                 decimal invoiceGrandTotal = 0;
                decimal invoiceVatTotal = 0;

                foreach (var item in basket)
                {

                    product = GetProduct(item.ProductId).Dto;

                    invoice = new Invoice()
                    {

                        BuyerUserId = buyerId,
                        SellerCompanyId = product.CompanyId,
                        Total = 0,
                        VatTotal = 0,
                        GrandTotal = 0,
                        StatusId = 0,
                        IsCancelled = false,
                        TransactionDateTime = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUserId = buyerId,

                    };

                    decimal total = 0;
                    decimal tax = 0;
                    decimal lineTotal = 0;

                    properties = new InvoiceProductDto()
                    {
                        productId = item.ProductId,
                        qty = item.Qty
                    };

                    if (properties.qty == 0)
                    {
                        return new ResponseDto().Failed("quantity cannot be 0 please try again");
                    }

                    if (product == null)
                    {
                        return new ResponseDto().Failed("Invalid Data");
                    }

                    var existingProduct = productList.FirstOrDefault(p => p.ProductId == product.Id);

                    var totalQty = properties.qty;

                    total = properties.qty * product.Price;
                    tax = total * 18 / 100;
                    lineTotal = tax + total;

                    #region InvoiceProductSave

                    if (existingProduct == null)
                    {
                        invoiceProduct = new InvoiceProduct()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            ProductVat = product.Tax,
                            Qty = totalQty,
                            Total = total,
                            Tax = tax,
                            LineTotal = lineTotal,

                        };
                        productList.Add(invoiceProduct);
                    }
                    else
                    {
                        invoiceProduct = new InvoiceProduct()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            ProductVat = product.Tax,
                            Qty = existingProduct.Qty + totalQty,
                            Total = existingProduct.Total + total,
                            Tax = existingProduct.Tax + tax,
                            LineTotal = existingProduct.LineTotal + lineTotal
                        };
                        productList.Remove(existingProduct);
                        productList.Add(invoiceProduct);
                    }

                    #endregion

                    invoiceTotal += total;
                    invoiceGrandTotal += lineTotal;
                    invoiceVatTotal += tax;
                    
                }

                invoice.Total = invoiceTotal;
                invoice.GrandTotal = invoiceGrandTotal;
                invoice.VatTotal = invoiceVatTotal;
                #region StockControl

                if (product.Stock >= invoiceProduct.Qty)
                {
                    product.Stock -= invoiceProduct.Qty;
                }
                else
                {
                    return new ResponseDto().Failed("Temporarily out of stock.");
                }

                #endregion

                dbContext.Invoices.Add(invoice);
                dbContext.SaveChanges();

                productList.ForEach(item =>
                {
                    item.InvoiceId = invoice.Id;
                });

                dbContext.InvoiceProducts.AddRange(productList);
                dbContext.SaveChanges();

                foreach (var item in basket)
                {
                    dbContext.Baskets.Remove(item);
                }

                dbContext.SaveChanges();

                #region Email

                sellerCompany = GetCompany(product.CompanyId).Dto;
                sellerUser = GetUser(sellerCompany.UserId).Dto;
                buyerUser = GetUser(buyerId).Dto;

                productList.ForEach(item =>
                {
                    mailProductNameList.Add(item.ProductName);
                });

                HtmlEmailPage htmlEmailPage = GetHtmlPage((int)EnumHtmlPageTypes.SendPurchaseEmail).Dto;

                mailSender.SendUserPurchaseEmail(buyerUser.UserName, mailProductNameList, invoice.GrandTotal, htmlEmailPage.Page);

                mailSender.SendCompanyPurchaseEmail(sellerUser.UserName, mailProductNameList, invoice.GrandTotal, htmlEmailPage.Page);

                #endregion

                return new ResponseDto().Success(invoice.Id);

            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        /// <summary>
        /// TODO:UpdateStatuse company id eklenicek yetki kontrolü yapılıcak
        /// TODO:Invioce de IsCancelled nereye konulucak ? 2 ayrı yerde olcucakmı ?
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="enumInvoiceStatus"></param>
        /// <returns></returns>

        public ResponseDto UpdateStatus(int id, EnumInvoiceStatusTypes enumInvoiceStatus)
        {
            try
            {
                Invoice invoice = dbContext.Invoices.Find(id);

                if (invoice == null)
                {
                    return new ResponseDto().Failed("Invoice Not Found.");
                }

                if (invoice.StatusId >= (int)enumInvoiceStatus)
                {
                    return new ResponseDto().Failed("");
                }

                invoice.StatusId = (int)enumInvoiceStatus;

                InvoiceStatusLog invoiceStatusLog = new InvoiceStatusLog()
                {
                    InvoiceId = invoice.Id,
                    Status = (int)enumInvoiceStatus,
                    CreateDate = DateTime.Now,
                    CreateUserId = invoice.SellerCompanyId,
                };

                dbContext.InvoiceStatusLogs.Add(invoiceStatusLog);
                dbContext.SaveChanges();

                return new ResponseDto().Success(invoice.Id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public ResponseDto Cancel(int Id)
        {
            try
            {
                Invoice Invoice = dbContext.Invoices.Find(Id);

                if (Invoice == null)
                {
                    return new ResponseDto().Failed("Invaild Data");
                }

                if (Invoice.IsCancelled == true)
                {
                    return new ResponseDto().Failed("Cancel Failed");
                }

                Invoice.CancelDate = DateTime.Now;
                Invoice.IsCancelled = true;

                dbContext.SaveChanges();

                return new ResponseDto().Success(Id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex, "Failed Cancel");
            }
        }

        public ResponseDto<bool> IsCanceled(int Id)
        {
            try
            {
                Invoice invoice = dbContext.Invoices.Find(Id);

                if (invoice == null)
                {
                    return new ResponseDto<bool>().Failed("Invoice Not Found.");
                }

                return new ResponseDto<bool>().Success(invoice.IsCancelled);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>().FailedWithException(ex);
            }
        }

        public ResponseDto<List<Invoice>> GetList(bool? isCancelled)
        {
            try
            {
                List<Invoice> invoices = dbContext.Invoices.Where(p => p.IsCancelled == isCancelled).ToList();

                return new ResponseDto<List<Invoice>>().Success(invoices);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Invoice>>().FailedWithException(ex);
            }
        }

        public ResponseDto<Product> GetProduct(int Id)
        {
            try
            {
                Product product = dbContext.Products.Find(Id);

                if (product == null)
                {
                    return new ResponseDto<Product>().Failed("Product Not Found");
                }

                return new ResponseDto<Product>().Success(product);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Product>().FailedWithException(ex);
            }
        }

        public ResponseDto<User> GetUser(int Id)
        {
            try
            {
                User user = dbContext.Users.Find(Id);

                if (user == null)
                {
                    return new ResponseDto<User>().Failed("User Not Found");
                }

                return new ResponseDto<User>().Success(user);
            }
            catch (Exception ex)
            {
                return new ResponseDto<User>().FailedWithException(ex);
            }

        }

        public ResponseDto<Company> GetCompany(int Id)
        {
            try
            {
                Company company = dbContext.Companies.Find(Id);

                if (company == null)
                {
                    return new ResponseDto<Company>().Failed("Company Not Found");
                }

                return new ResponseDto<Company>().Success(company);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Company>().FailedWithException(ex);
            }
        }

        public ResponseDto<HtmlEmailPage> GetHtmlPage(int Id)
        {
            try
            {
                HtmlEmailPage htmlEmailPage = dbContext.HtmlEmailPages.Find(Id);
                return new ResponseDto<HtmlEmailPage>().Success(htmlEmailPage);
            }
            catch (Exception ex)
            {
                return new ResponseDto<HtmlEmailPage>().FailedWithException(ex);
            }
        }
        public ResponseDto<Basket> GetBasket(int Id)
        {
            try
            {
                Basket basket = dbContext.Baskets.Find(Id);
                return new ResponseDto<Basket>().Success(basket);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Basket>().FailedWithException(ex);
            }
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}