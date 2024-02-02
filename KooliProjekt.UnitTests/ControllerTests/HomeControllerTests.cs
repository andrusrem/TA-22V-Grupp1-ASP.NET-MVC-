using Xunit;
using KooliProjekt.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Http;
using KooliProjekt.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        
        [Fact]
        public void Index_should_return_index_view()
        {

            // Arrange
            var controller = new HomeController();
            

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) ||
                        result.ViewName == "Index");
        }
        [Fact]
        public void Privacy_should_return_privacy_view()
        {

            // Arrange
            var controller = new HomeController();
            

            // Act
            var result = controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) ||
                        result.ViewName == "Privacy");
        }

       [Fact]
        public void Error_should_return_error_view_with_current_trace_identifier() {
            // Arrange
            var controller = new HomeController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Error_should_return_error_view_with_current_activity() {
            // Arrange
            var controller = new HomeController();
            new Activity("TestActivity").Start();

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Construct_With_Logger() {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();

            // Act
            var controller = new HomeController(logger.Object);

            // Assert
            Assert.NotNull(controller);
        }
    }
}
