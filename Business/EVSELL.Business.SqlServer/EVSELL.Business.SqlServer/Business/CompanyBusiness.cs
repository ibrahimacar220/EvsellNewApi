using Evsell.Business.Common.Response;
using Evsell.Business.Email.MailBusiness;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business
{
    public class CompanyBusiness : ICompanyBusiness
    {
        readonly EvsellDbContext dbContext;
        public CompanyBusiness()
        {
            dbContext = new EvsellDbContext();
        }

        public ResponseDto Save(int id, int userId, string name, bool isActive)
        {
            CompanyEmail mailSender = new CompanyEmail();

            try
            {
                Company company = null;

                if (id <= 0)
                {//insert

                    User user = dbContext.Users.Find(userId);
                    if (user == null)
                    {
                        return new ResponseDto().Failed("User Not Found.");
                    }
                    if ((EnumUserTypes)user.UserType != EnumUserTypes.Company && (EnumUserTypes)user.UserType != EnumUserTypes.Admin)
                    {
                        return new ResponseDto().Failed("Only Company Users Can Crate a Company.");
                    }

                    company = new Company()
                    {
                        UserId = userId,
                        Name = name,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUserId = 1
                    };

                    HtmlEmailPage htmlEmailPage = GetHtmlPage((int)EnumHtmlPageTypes.RegisterCompanyEmail).Dto;
                    mailSender.SendRegisterCompany(user.UserName, name, htmlEmailPage.Page);

                    dbContext.Companies.Add(company);
                }

                else
                {//update
                    company = Get(id).Dto;
                    if (company == null)
                    {
                        return null;
                    }
                    company.UserId = userId;
                    company.Name = name;
                }

                dbContext.SaveChanges();

                return new ResponseDto().Success(company?.Id);
            }

            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex, "pls try again");
            }
        }

        public ResponseDto<Company> Get(int id)
        {
            try
            {   
                Company company = dbContext.Companies.Find(id);
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

        public ResponseDto<User> GetUser(int id)
        {
            try
            {
                User user = dbContext.Users.Find(id);
                return new ResponseDto<User>().Success(user);
            }
            catch (Exception ex)
            {
                return new ResponseDto<User>().FailedWithException(ex);
            }
        }

        public ResponseDto<HtmlEmailPage> GetHtmlPage(int id)
        {
            try
            {
                HtmlEmailPage htmlEmailPage = dbContext.HtmlEmailPages.Find(id);
                return new ResponseDto<HtmlEmailPage>().Success(htmlEmailPage);
            }
            catch (Exception ex)
            {
                return new ResponseDto<HtmlEmailPage>().FailedWithException(ex);
            }
        }

        public ResponseDto<List<Company>> GetList(bool? isActive)
        {
            try
            {
                List<Company> companies = (from x in dbContext.Companies
                                           where isActive == null || x.IsActive == isActive.Value
                                           select x).ToList();

                return new ResponseDto<List<Company>>().Success(companies);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<Company>>().FailedWithException(ex);
            }
        }

        public ResponseDto Delete(int id)
        {
            try
            {
                Company company = Get(id).Dto;
                if (company == null)
                {
                    return new ResponseDto().Failed("Company Not Found");
                }
                
                dbContext.Companies.Remove(company);
                dbContext.SaveChanges();

                return new ResponseDto().Success(id);
            }
            catch (Exception ex)
            {
                return new ResponseDto().FailedWithException(ex);
            }
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}