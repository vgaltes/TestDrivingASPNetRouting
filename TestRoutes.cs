using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using System.Web;
using Moq;
using System.Web.Mvc;

namespace TestDrivingASPNetRouting.Tests
{
    [TestClass]
    public class TestRoutes
    {
        RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
        }

        [TestMethod]
        public void TestSimpleRoute()
        {           
            RouteData result = routes.GetRouteData(CreateHttpContext("~/Admin/Index"));
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
        }       

        [TestMethod]
        public void TestDefaults()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/"));

            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultController", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("DefaultIndex", GetRouteValueFor(result, "action"));
        }

        [TestMethod]
        public void TestStaticUrlSegments()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/Public/Admin/Index"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
        }

        [TestMethod]
        public void TestMixedSegments()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/MixedAdmin/Index"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
        }

        [TestMethod]
        public void TestAlias()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/OldAdmin/OldIndex"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
        }

        [TestMethod]
        public void TestCustomSegment()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/Admin/Index/SomeId"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
            Assert.AreEqual("SomeId", GetRouteValueFor(result, "id"));
        }

        [TestMethod]
        public void TestOptionalSegment()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/OptionalAdmin/Index"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
            Assert.AreEqual(UrlParameter.Optional, GetRouteValueFor(result, "id"));
        }

        [TestMethod]
        public void TestOptionalSegmentWithValue()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/OptionalAdmin/Index/4"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
            Assert.AreEqual("4", GetRouteValueFor(result, "id"));
        }

        [TestMethod]
        public void TestVariableLengthRoute()
        {
            RouteData result = routes.GetRouteData(CreateHttpContext("~/CatchAllAdmin/Index/SubIndex/Step1/Step2/Step3"));

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", GetRouteValueFor(result, "controller"));
            Assert.AreEqual("Index", GetRouteValueFor(result, "action"));
            Assert.AreEqual("SubIndex", GetRouteValueFor(result, "id"));
            Assert.AreEqual("Step1/Step2/Step3", GetRouteValueFor(result, "catchAll"));
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

        private object GetRouteValueFor(RouteData result, string key)
        {
            return result.Values[key];
        }
    }
}
