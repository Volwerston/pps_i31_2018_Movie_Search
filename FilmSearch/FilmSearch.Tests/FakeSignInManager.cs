using FilmSearch.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FilmSearch.Tests
{
    public class FakeSignInManager : SignInManager<AppUser>
    {
        public FakeSignInManager()
            : base
            (
            new FakeUserManager(),
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object
            )
        { }
        public override Task SignOutAsync()
        {
            return Task.FromResult(1);
        }
        public override Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult<SignInResult>(new FakeSignInResult(user.UserName=="Name"));
        }

    }
    public class FakeSignInResult : SignInResult
    {
        public FakeSignInResult(bool success): base()
        {
            Succeeded = success;
        }
    }
}
