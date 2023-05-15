using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business
{
    public class ProductBusiness : IProductBusiness
    {
        readonly EvsellDbContext dbContext;

        public ProductBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto Save(int id, int companyId, bool isActive, string name, string image, int price, string description, int categoryId, int Stock)
        {
            try
            {
                Product product = null;

                if (id <= 0)
                {//insert
                    product = new Product()
                    {
                        CompanyId = companyId,
                        Name = name,
                        Price = price,
                        Image = image,
                        Description = description,
                        CategoryId = categoryId,
                        Stock = Stock,
                        Tax = 18,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUserId = 1
                    };
                    dbContext.Products.Add(product);
                }

                else
                {//update
                    product = Get(id).Dto;
                    if (product == null)
                    {
                        return null;
                    }
                    product.Name = name;
                    product.Price = price;
                    product.Image = image;
                    product.Description = description;
                }

                dbContext.SaveChanges();
                return new ResponseDto().Success(id);
            }

            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);

            }
        }

        public ResponseDto Delete(int id)
        {
            ResponseDto<Product> product = Get(id);
            if (product == null)
            {
                return new ResponseDto().Failed("Invalid Product");
            }

            dbContext.Products.Remove(product.Dto);
            dbContext.SaveChanges();
            return new ResponseDto().Success(id);
        }

        public ResponseDto ProductStockChange(int id, int change, bool isInput)
        {
            try
            {
                Product product = Get(id).Dto;

                if (product == null)
                {
                    return new ResponseDto().Failed("Product Not Found.");
                }

                if (change <= 0)
                {
                    return new ResponseDto().Failed("Stock Change Amount Must be Bigger Than 0");
                }

                ProductStockLog productStockLog = new ProductStockLog()
                {
                    ProductId = product.Id,
                    Qty = change,
                    IsInput = isInput,

                    CreateUserId = product.CompanyId,
                    CreateDate = DateTime.Now,
                };
                //                       true false
                product.Stock = (isInput ? 1 : -1) * change + product.Stock;
                //if (isInput == true)
                //{
                //    product.Stock += change;
                //}
                //else
                //{
                //    product.Stock -= change;
                //}

                dbContext.ProductStockLogs.Add(productStockLog);
                dbContext.SaveChanges();

                return new ResponseDto().Success(product.Id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public ResponseDto StockControl(int id)
        {
            try
            {
                Product product = Get(id).Dto;
                if (product == null)
                {
                    return new ResponseDto().Failed("Product Not Found");
                }
                return new ResponseDto().Success(product.Stock);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public ResponseDto<Product> Get(int id)
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

        public ResponseDto<List<Product>> GetList(bool? isActive)
        {
            try
            {
                List<Product> products = (from x in dbContext.Products
                                          where isActive == null || x.IsActive == isActive.Value
                                          select x).ToList();

                return new ResponseDto<List<Product>>().Success(products);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Product>>().FailedWithException(ex);
            }//wrap
        }

        public ResponseDto<List<Product>> GetListByCategoryId(int id)
        {
            try
            {
               ProductCategory productCategory = dbContext.ProductCategories.Find(id);
                
                if (productCategory == null)
                {
                    return new ResponseDto<List<Product>>().Failed("Category Not Found.");
                }

                List<Product> products = dbContext.Products.Where(x => x.CategoryId == id).ToList();

                return new ResponseDto<List<Product>>().Success(products);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Product>>().FailedWithException(ex);
            }//wrap
        }

        public ResponseDto<List<Product>> GetListByCompanyId(int id)
        {
            try
            {
                Company company = dbContext.Companies.Find(id);
               
                if (company == null)
                {
                    return new ResponseDto<List<Product>>().Failed("Company Not Found.");
                }

                List<Product> products = dbContext.Products.Where(p => p.CompanyId == id).ToList();

                return new ResponseDto<List<Product>>().Success(products);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Product>>().FailedWithException(ex);
            }//wrap
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}
