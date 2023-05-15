using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.Basket;
using Evsell.Business.SqlServer.Business;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Dtos;
using Evsell.Business.SqlServer.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVSELL.Business.SqlServer.Business
{
    public class BasketBusiness : IBasketBusiness
    {
        readonly EvsellDbContext dbContext;
        public BasketBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto Save(int buyerId, List<InvoiceProductDto> productProperties)
        {
            Product product = null;
            List<Basket> basketProductList = new List<Basket>();
            Basket basket = null;
            Basket OldBasket = null;

            foreach (var item in productProperties)
            {

               User user = dbContext.Users.Find(buyerId);

                if (user == null)
                {
                  return new ResponseDto().Failed("User Not Found");
                  
                }

                product = GetProduct(item.productId).Dto;

                var existingBasket = basketProductList.FirstOrDefault(p => p.ProductId == product.Id);

                BasketLog basketLog = new BasketLog()
                {
                    IsInput = true,
                    CreateDate = DateTime.Now,
                    CreateUserId = buyerId,
                    ProductId = item.productId,
                    IsDeleted = false,
                    Qty = item.qty,
                };

                dbContext.BasketLogs.Add(basketLog);
                dbContext.SaveChanges();

                if (product.Stock < item.qty)
                {
                    return new ResponseDto().Failed("Temporarily out of stock.");
                }

                if (existingBasket == null)
                {
                    basket = new Basket()
                    {
                        UserId = buyerId,

                        ProductId = item.productId,

                        Qty = item.qty,

                        CreateDate = DateTime.Now,

                        CreateUserId = buyerId,

                    };
                    basketProductList.Add(basket);
                }
                else
                {
                    basket = new Basket()
                    {
                        UserId = buyerId,

                        ProductId = product.Id,

                        Qty = item.qty += existingBasket.Qty,

                        CreateDate = DateTime.Now,

                        CreateUserId = buyerId,

                    };

                    basketProductList.Remove(existingBasket);

                    basketProductList.Add(basket);

                }

            }

            foreach (var item2 in basketProductList)
            {

                OldBasket = (from x in dbContext.Baskets
                             where x.ProductId == item2.ProductId && x.UserId == buyerId
                             select x).FirstOrDefault();

                if (OldBasket != null)
                {
                    if (item2.ProductId == OldBasket.ProductId)
                    {
                        dbContext.Remove(OldBasket);
                        item2.Qty += OldBasket.Qty;
                    }
                }
            }

            dbContext.Baskets.AddRange(basketProductList);
            dbContext.SaveChanges();

            return new ResponseDto().Success();
        }

        public ResponseDto<List<BasketBo>> GetBaskets(int userId)
        {
            try
            {

                List<BasketBo> basketDtos = (from B in dbContext.Baskets
                                              join P in dbContext.Products
                                              on B.ProductId equals P.Id
                                              where B.UserId == userId
                                              select new BasketBo
                                              {
                                                  ProductId = P.Id,
                                                  ProductName = P.Name,
                                                  Image = P.Image,
                                                  Qty = B.Qty,
                                                  Price = P.Price
                                              }).ToList();

                return new ResponseDto<List<BasketBo>>().Success(basketDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<BasketBo>>().FailedWithException(ex);
            }
        }

        public ResponseDto<Product> GetProduct(int id)
        {
            try
            {
                Product product = dbContext.Products.Find(id);

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

        public ResponseDto Delete(int userId, int qty, int productId)
        {
            try
            {

                List<Basket> baskets = dbContext.Baskets.Where(b => b.UserId == userId).ToList();

                Basket basket = baskets.FirstOrDefault(p => p.ProductId == productId);

                BasketLog basketLog = new BasketLog()
                {

                    IsDeleted = false,
                    IsInput = false,
                    ProductId = productId,
                    Qty = qty,
                    CreateDate = DateTime.Now,
                    CreateUserId = userId

                };

                basket.Qty -= qty;

                if (basket.Qty == 0)
                {
                    dbContext.Baskets.Remove(basket);
                    dbContext.SaveChanges();

                    basketLog.IsDeleted = true;
                }

                dbContext.BasketLogs.Add(basketLog);

                dbContext.SaveChanges();

                return new ResponseDto().Success();

            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }

        }
    }
}
