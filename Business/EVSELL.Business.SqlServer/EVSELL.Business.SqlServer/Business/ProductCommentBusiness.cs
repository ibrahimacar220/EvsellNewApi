using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business
{
    public class ProductCommentBusiness:IProductCommentBusiness
    {
        readonly EvsellDbContext dbContext;
        public ProductCommentBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto Save(int id, int userId, bool IsActive, int productId, string comments, string header, int productRate)
        {
            if (productRate > 5 || productRate <= 0)
            {
               return new  ResponseDto().Failed("Enter a Rate Between 5 And 0");
            }

            try
            {
                ProductComment comment = null;

                if (id <= 0)
                {//insert 
                 // 1 => productRate <= 5 and cant null

                    comment = new ProductComment()
                    {
                        Id = id,
                        UserId = userId,
                        ProductId = productId,
                        Comments = comments,
                        Header = header,
                        IsActive = true,
                        ProductRate = productRate
                    };

                    dbContext.ProductComments.Add(comment);

                }
                else
                {//update

                    comment = Get(id).Dto;

                    if (comment == null)
                    {
                        return null;
                    }

                    comment.Comments = comments;
                    comment.Header = header;

                }

                dbContext.SaveChanges();

                return new ResponseDto().Success(comment.Id);
            }

            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public ResponseDto Delete(int id)
        {
            ProductComment productComment = Get(id).Dto;
            
            if (productComment == null)
            {
                return new ResponseDto().Failed("ProductComment Not Found");
            }

            dbContext.ProductComments.Remove(productComment);
            dbContext.SaveChanges();
            
            return new ResponseDto().Success(id);
        }

        public ResponseDto<ProductComment> Get(int id)
        {
            try
            {
                ProductComment productComment = dbContext.ProductComments.Find(id);
                return new ResponseDto<ProductComment>().Success(productComment);
            }
            catch (Exception ex)
            {
              return new ResponseDto<ProductComment>().FailedWithException(ex);
            } 
        }

        public  ResponseDto<List<ProductComment>> GetList(bool? isActive)
        {
            try
            {
                List<ProductComment> productComments = (from x in dbContext.ProductComments
                                                        where isActive == null || x.IsActive == isActive.Value
                                                        select x).ToList();
               return new ResponseDto<List<ProductComment>>().Success(productComments);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ProductComment>>().FailedWithException(ex);
            }   
        }

        public void Dispose()
        {
            if (dbContext == null) return;
            {
                dbContext.Dispose();
            }
        }
    }
}
