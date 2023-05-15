using Evsell.Business.Common.Response;
using Evsell.Business.Email.MailBusiness;
using Evsell.Business.SqlServer.Bo.Basket;
using Evsell.Business.SqlServer.Bo.User;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Dtos;
using Evsell.Business.SqlServer.Models;
using System.Text.RegularExpressions;

namespace Evsell.Business.SqlServer.Business
{
    public class UserBusiness : IUserBusiness
    {
        readonly EvsellDbContext dbContext;

        Get get = new Get();

        public UserBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        /// <summary>
        /// Inserts or update the record due to 'Id' param.
        /// </summary>
        /// <param name="id">Less than 0 means new record otherwise it will be considered as update operation</param>
        /// <param name="isActive">Only for update</param>
        /// <param name="userType"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firtsName"></param>
        /// <param name="lastName"></param>
        /// <returns>Null return means exception or failure.</returns>
        /// 
        public ResponseDto Save(UserBo userBo)
        {
            WelcomeEmail mailSender = new WelcomeEmail();

            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchGmail = regexMail.Match(userBo.UserName);
            if (!matchGmail.Success)
            {
                return new ResponseDto().Failed("Plase Enter a Valid Mail like this: adc@exemple.com");
            }

            //min 8 characters in length, one Upper case ,one lower case, and one number

            Regex regexPassword = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
            Match matchPassword = regexPassword.Match(userBo.Password);

            if (!matchPassword.Success)
            {
                return new ResponseDto().Failed("Plase min 8 characters in length, one Upper case ,one lower case, and one number");
            }

            try
            {
                 User user = null;

                if (userBo.Id <= 0)
                {
                    // insert

                    user = new User()
                    {
                        UserType = (int)userBo.UserType,
                        UserName = userBo.UserName,
                        Password = userBo.Password,
                        FirtsName = userBo.FirtsName,
                        LastName = userBo.LastName,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUserId = 1
                    };

                    HtmlEmailPage htmlEmailPage = get.GetHtmlPage((int)EnumHtmlPageTypes.WelcomePage).Dto;

                    mailSender.SendWelcomeEmail(user.UserName, user.FirtsName, htmlEmailPage.Page);

                    dbContext.Users.Add(user);
                }
                else
                {
                    // update

                    user = dbContext.Users.Find(userBo.Id);

                    if (user == null)
                    {
                        return null;
                    }

                    user.FirtsName = userBo.FirtsName;
                    user.LastName = userBo.LastName;
                    user.Password = userBo.Password;
                    user.IsActive = true;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateUserId = 1;
                }

                dbContext.SaveChanges();

                return new ResponseDto().Success(user?.Id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex, "PLS TRY AGAIN");
            }
        }

        public ResponseDto Delete(UserDelBo userDelBo)
        {
            try
            {
                User user = dbContext.Users.Find(userDelBo.Id);
               
                if (user == null)
                {
                    return new ResponseDto().Failed("User Not Found");
                }

                dbContext.Users.Remove(user);
                dbContext.SaveChanges();

                return new ResponseDto().Success(userDelBo.Id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public ResponseDto<UserBo> Get(UserGetBo userGetBo)
        {
            try
            {
                User user = dbContext.Users.Find(userGetBo.Id);

                if (user == null)
                {
                    return new ResponseDto<UserBo>().Failed("User Not Found");
                }

                UserBo userBo = new UserBo()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirtsName = user.FirtsName,
                    LastName = user.LastName,
                    Password = user.Password,
                    UserType = (EnumUserTypes)user.UserType
                };

                return new ResponseDto<UserBo>().Success(userBo);
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserBo>().FailedWithException(ex);
            }
        }

        public ResponseDto<UserBo> GetByUserName(string userName)
        {
            try
            {
                User user = dbContext.Users.FirstOrDefault(x => x.UserName == userName);
                if (user == null)
                {
                    return new ResponseDto<UserBo>().Failed("User Not Found");
                }
                UserBo userBo = new UserBo()
                {
                    Id = user.Id,
                    FirtsName = user.FirtsName,
                    LastName = user.LastName,
                    Password = user.Password,
                    UserName = user.UserName,
                    UserType = (EnumUserTypes)user.UserType
                };
                return new ResponseDto<UserBo>().Success(userBo);
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserBo>().FailedWithException(ex);
            }
        }

        //public ResponseDto<HtmlEmailPage> GetHtmlPage(int id)
        //{

        //    try
        //    {
        //        HtmlEmailPage htmlEmailPage = dbContext.HtmlEmailPages.Find(id);
        //        return new ResponseDto<HtmlEmailPage>().Success(htmlEmailPage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDto<HtmlEmailPage>().FailedWithException(ex);
        //    }
        //}

        public ResponseDto<List<UserGetListBo>> GetList(UserGetListCriteriaBo bo)
        {

            try
            {
                
                List<User> users = (from x in dbContext.Users
                                    where bo.IsActive == null || x.IsActive == bo.IsActive
                                    select x).ToList();

                List<UserGetListBo> UserListBo = new List<UserGetListBo>();

                foreach (var user in users)
                {
                    List<Basket> baskets = dbContext.Baskets.Where(p => p.UserId == user.Id).ToList();

                    List<BasketBo> basketListBo = new List<BasketBo>();

                    UserGetListBo UserBo = new UserGetListBo()
                    {
                        Id = user.Id,
                        FirtsName = user.FirtsName,
                        UserName = user.UserName,
                        LastName = user.LastName,
                        Password = user.Password,
                        UserType = user.UserType,
                        IsActive = user.IsActive,

                        CreateDate = user.CreateDate,
                        CreateUserId = user.CreateUserId,
                        UpdateDate = user.UpdateDate,
                        UpdateUserId = user.UpdateUserId,

                        baskets = basketListBo,
                    };

                    foreach (var basket in baskets)
                    {

                        Product product = dbContext.Products.Find(basket.ProductId);

                        BasketBo basketBo = new BasketBo()
                        {
                            Image = product.Image,
                            Price = product.Price,
                            ProductId = basket.ProductId,
                            Qty = basket.Qty,
                            ProductName = product.Name
                        };

                        basketListBo.Add(basketBo);
                    
                    }

                    UserListBo.Add(UserBo);

                }

                return new ResponseDto<List<UserGetListBo>>().Success(UserListBo);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<UserGetListBo>>().FailedWithException(ex);
            }
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}