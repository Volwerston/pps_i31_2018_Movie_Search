using FilmSearch.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    
    public class NewsLetterControllerTests
    {
        [Fact]
        public void Index()
        {
            NewsletterController NLC = new NewsletterController();
            var result = NLC.Index() as ViewResult;
            result.Should().NotBeNull();
        }
    }
}
