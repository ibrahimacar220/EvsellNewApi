using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business
{
    public class ProdutCategoryBusiness : IProductCategoryBusiness
    {
        readonly EvsellDbContext dbContext;

        public ProdutCategoryBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto<List<ProductCategory>> GetList()
        {
            try
            {
                List<ProductCategory> productCategory = (from x in dbContext.ProductCategories
                                                         select x).ToList();
                return new ResponseDto<List<ProductCategory>>().Success(productCategory);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ProductCategory>>().FailedWithException(ex);
            }
        }

        public ResponseDto<List<Productcate>> GetListChildren()
        {
            List<ProductCategory> rawList = GetList().Dto;
            Productcate resultItem = null;
            List<Productcate> result = new List<Productcate>();
            try
            {
                foreach (ProductCategory item in rawList.Where(p => p.ParentId == null))
                {
                    resultItem = new Productcate()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        SubList = GetListChildItem(item.Id, rawList),
                    };

                    result.Add(resultItem);
                }
                return new ResponseDto<List<Productcate>>().Success(result);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Productcate>>().FailedWithException(ex);
            }
        }
        List<Productcate> GetListChildItem(int id, List<ProductCategory> rawList)
        {
            try
            {
                List<Productcate> list = (from x in rawList
                                          where x.ParentId == id
                                          select new Productcate()
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              SubList = GetListChildItem(x.Id, rawList),
                                          }).ToList();
                return list;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
    public class Productcate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Productcate> SubList { get; set; }
    }
}