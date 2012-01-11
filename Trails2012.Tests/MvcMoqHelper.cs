using System;
using System.Web;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Trails2012.Tests
{
    /// <summary>
    /// http://www.hanselman.com/blog/ASPNETMVCSessionAtMix08TDDAndMvcMockHelpers.aspx
    /// http://stackoverflow.com/questions/1228179/mocking-httpcontextbase-with-moq 
    /// http://blogs.teamb.com/craigstuntz/2008/07/10/37824/
    /// http://haacked.com/archive/2007/11/05/rhino-mocks-extension-methods-mvc-crazy-delicious.aspx
    /// </summary>
    public static class MvcMoqHelper
    {
            public static HttpContextBase FakeHttpContext()
            {
                var context = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();
                var response = new Mock<HttpResponseBase>();
                var session = new Mock<HttpSessionStateBase>();
                var server = new Mock<HttpServerUtilityBase>();

                context.Setup(ctx => ctx.Request).Returns(request.Object);
                context.Setup(ctx => ctx.Response).Returns(response.Object);
                context.Setup(ctx => ctx.Session).Returns(session.Object);
                context.Setup(ctx => ctx.Server).Returns(server.Object);

                return context.Object;
            }

            public static HttpContextBase FakeHttpContext(string url)
            {
                HttpContextBase context = FakeHttpContext();
                context.Request.SetupRequestUrl(url);
                return context;
            }

            public static void SetFakeControllerContext(this Controller controller)
            {
                var httpContext = FakeHttpContext();
                ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
                controller.ControllerContext = context;
            }

            static string GetUrlFileName(string url)
            {
                if (url.Contains("?"))
                    return url.Substring(0, url.IndexOf("?"));
                return url;
            }

        static NameValueCollection GetQueryStringParameters(string url)
        {
            if (url.Contains("?"))
                {
                    NameValueCollection parameters = new NameValueCollection();

                    string[] parts = url.Split("?".ToCharArray());
                    string[] keys = parts[1].Split("&".ToCharArray());

                    foreach (string key in keys)
                    {
                        string[] part = key.Split("=".ToCharArray());
                        parameters.Add(part[0], part[1]);
                    }

                    return parameters;
                }
            return null;
        }

        public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
            {
                Mock.Get(request)
                    .Setup(req => req.HttpMethod)
                    .Returns(httpMethod);
            }

            public static void SetupRequestUrl(this HttpRequestBase request, string url)
            {
                if (url == null)
                    throw new ArgumentNullException("url");

                if (!url.StartsWith("~/"))
                    throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

                var mock = Mock.Get(request);

                mock.Setup(req => req.QueryString)
                    .Returns(GetQueryStringParameters(url));
                mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
                    .Returns(GetUrlFileName(url));
                mock.Setup(req => req.PathInfo)
                    .Returns(string.Empty);
            }

    }
}
