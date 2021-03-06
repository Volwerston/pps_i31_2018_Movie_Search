﻿using FilmSearch.Controllers;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public void DisableUser()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um, uv.Object);

            string id = "1";
            var result = AC.DisableUser(id).Result;
            result.Should().NotBeNull();

        }
        [Fact]
        public void EnableUser()
        {
            //var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "Add_writes_to_database").Options;
            ////////////////////////
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um, uv.Object);

            string id = "1";
            var result =  AC.EnableUser(id).Result;
            result.Should().NotBeNull();

        }
        [Fact]
        public void IndexTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            var failed = Task.FromResult(IdentityResult.Failed());
            //Console.Write(failed.Errors);
            uv.Setup(x => x.ValidateAsync(um, It.IsAny<AppUser>())).Returns(failed);
            //uv.Setup(x => x.ValidateAsync(um, fakeUser)).Returns(Task.FromResult(IdentityResult.Success));
            uv.Setup(x => x.ValidateAsync(um, It.Is<AppUser>(a => a == fakeUser))).Returns(Task.FromResult(IdentityResult.Success));
            AccountController AC = new AccountController(um, uv.Object);

            var result = AC.Index();
            result.Should().NotBeNull();
           var result2 = AC.Index(new AppUserViewModel() {Name="name",Surname="surname" });
            result2 = AC.Index(new AppUserViewModel() { Name = "asd", Surname = "sde" });
            result2.Should().NotBeNull();
        }
        [Fact]
        public void BanTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um, uv.Object);

            var result = (AC.Ban() as ViewResult).Model as List<AppUser>;
            result.Should().NotBeNull();
        }
        [Fact]
        public void PersonStatsTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUserValidator<AppUser>> uv = new Mock<IUserValidator<AppUser>>();
            AccountController AC = new AccountController(um, uv.Object);

            var result = (AC.PersonStats() as ViewResult);
            result.Should().NotBeNull();
        }
        AppUser fakeUser = new AppUser()
        {
            Id = "1",
            AccessFailedCount = 1,
            Email = "examle@smth.com",
            UserName = "name",
            EmailConfirmed = false,
            LockoutEnabled = false,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Surname = "surname"
        };
    }
}
