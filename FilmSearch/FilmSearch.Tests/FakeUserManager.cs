using FilmSearch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FilmSearch.Tests
{

    public class FakeUserManager : UserManager<AppUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<AppUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<AppUser>>().Object,
                  new IUserValidator<AppUser>[0],
                  new IPasswordValidator<AppUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<AppUser>>>().Object)
        {
        }
        public override Task<AppUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult<AppUser>(new AppUser()
            {
                Id = "1",
                AccessFailedCount = 1,
                Email = "examle@smth.com",
                UserName = "name"
            });
        }


        public override IQueryable<AppUser> Users => new List<AppUser>()
        {
                new AppUser()
                {
                    Id = "1",
                    AccessFailedCount =1,
                    Email="examle@smth.com",
                    UserName="name"
                }
        }.AsQueryable();

        public override Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            //return new Task<IdentityResult>(() => { return new IdentityResult(); });
            return Task.FromResult<IdentityResult>(new FakeIdentityResult(user.UserName == "Name"));
        }
        public override Task<AppUser> FindByEmailAsync(string email)
        {
            return Task.FromResult<AppUser>(new AppUser() { Email="example@gmail.com",Id="1",UserName="Name"});
        }
    }
    class FakeIdentityResult : IdentityResult
        {
            public FakeIdentityResult(bool toRet) : base()
        {
            Succeeded = toRet;
        }
        }
}
