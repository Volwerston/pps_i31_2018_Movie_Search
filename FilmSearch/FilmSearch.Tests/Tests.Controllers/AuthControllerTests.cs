using FilmSearch.Controllers;
using FilmSearch.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public void Initialize()
        {
            UserManager<AppUser> um = new FakeUserManager();
        }
    }
}
