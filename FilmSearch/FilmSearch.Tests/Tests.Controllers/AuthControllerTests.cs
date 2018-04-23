using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            SignInManager<AppUser> sim = new FakeSignInManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            RoleManager<IdentityRole> rm = new FakeRoleManager();
            AuthController AC = new AuthController(um, sim,uow.Object,rm);

            AC.Should().NotBeNull();
        }
        [Fact]
        public void RegisterTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            SignInManager<AppUser> sim = new FakeSignInManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            RoleManager<IdentityRole> rm = new FakeRoleManager();
            AuthController AC = new AuthController(um, sim, uow.Object, rm);

            var result = AC.Register() as ViewResult;
            result.Should().NotBeNull();

            var result2 = AC.Register(fakeRVM).Result;
            result2.Should().NotBeNull();
            fakeRVM.Name = "another";
            result2 = AC.Register(fakeRVM).Result;
            result2.Should().NotBeNull();
        }

        [Fact]
        public void LoginTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            SignInManager<AppUser> sim = new FakeSignInManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            RoleManager<IdentityRole> rm = new FakeRoleManager();
            AuthController AC = new AuthController(um, sim, uow.Object, rm);

            var result = AC.Login("https://google.com.ua") as ViewResult;
            result.Should().NotBeNull();

            var result2 = AC.Login(fakelvm, "https://google.com.ua");
            result2.Should().NotBeNull();
        }
        [Fact]
        public void LogoutTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            SignInManager<AppUser> sim = new FakeSignInManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            RoleManager<IdentityRole> rm = new FakeRoleManager();
            AuthController AC = new AuthController(um, sim, uow.Object, rm);

            var result = AC.Logout().Result;
            result.Should().NotBeNull();
        }

        RegisterViewModel fakeRVM = new RegisterViewModel() { Email = "examle@gmail.com", Name = "Name", Password = "12345", ConfirmPassword = "12345", Surname = "Surname" };
        LoginViewModel fakelvm = new LoginViewModel() {Email="example@gmail.com",Password="qwerty" };
    }
}
