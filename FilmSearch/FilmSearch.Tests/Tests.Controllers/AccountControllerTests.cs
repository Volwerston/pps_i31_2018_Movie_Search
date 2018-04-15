using FilmSearch.Controllers;
using FilmSearch.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class AccountControllerTests
    {
        public class AppDbContext : IdentityDbContext<AppUser>
        {
            public AppDbContext()
                : base()
            {
            }
        }

        [Fact]
        public void Initialize()
        {
            Mock<UserManager<AppUser>> um = new Mock<UserManager<AppUser>>();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um.Object,uv.Object);

            AC.Should().NotBeNull();
        }
        [Fact]
        public void DisableUser()
        {
            Mock<UserManager<AppUser>> um = new Mock<UserManager<AppUser>>();
            //UserManager<AppUser> um = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
            //UserManager<AppUser> um = new UserManager<AppUser>(new IUserStore<AppUser>(),new IOptions<IdentityOptions>(),new IPasswordHasher<AppUser>(), new IEnumerable<IUserValidator<AppUser>>(), new IEnumerable<IPasswordValidator<AppUser>>(),new ILookupNormalizer(),new IdentityErrorDescriber(),new IServiceProvider(),new ILogger<UserManager<AppUser>>());
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um.Object, uv.Object);

            //UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>());
        }

    }
}
