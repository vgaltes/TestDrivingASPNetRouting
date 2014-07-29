using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using System.Web;
using Moq;

namespace TestDrivingASPNetRouting.Tests
{
    [TestClass]
    public class TestRoutes
    {
        [TestMethod]
        public void TestSimpleRoute()
        {
            // Arrange - register routes
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            // Act - process the route
            RouteData result
            = routes.GetRouteData(CreateHttpContext("~/Admin/Index"));
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result.Values["controller"]);
            Assert.AreEqual("Index", result.Values["action"]);
        }

        [TestMethod]
        public void TestDefaults()
        {
            // Arrange - register routes
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            // Act - process the route
            RouteData result
            = routes.GetRouteData(CreateHttpContext("~/"));
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultController", result.Values["controller"]);
            Assert.AreEqual("DefaultIndex", result.Values["action"]);
        }

        [TestMethod]
        public void TestStaticUrlSegments()
        {
            // Arrange - register routes
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            // Act - process the route
            RouteData result
            = routes.GetRouteData(CreateHttpContext("~/Public/Admin/Index"));
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result.Values["controller"]);
            Assert.AreEqual("Index", result.Values["action"]);
        }

        private HttpContextBase CreateHttpContext(string targetUrl = null)
        {
            var mockRequest = new Mock<HttpRequestBase>();

            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns("GET");

            var mockResponseBase = new Mock<HttpResponseBase>();
            mockResponseBase.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponseBase.Object);

            return mockContext.Object;
        }
    }
}
