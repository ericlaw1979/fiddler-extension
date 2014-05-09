using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using Owin;

namespace Runscope
{
    public class OAuthDesktopFlow : IDisposable
    {
        private IDisposable _server;
        private AutoResetEvent _Lock = new AutoResetEvent(false);
        private string _AuthCode;
        public string GetAuthCode(Uri authServer, Uri redirectUri)
        {
            //

            _server = CreateCallbackServer(redirectUri);
            Process.Start(authServer.OriginalString);
            _Lock.WaitOne(new TimeSpan(0,0,3,0));
            return _AuthCode;

        }

        public IDisposable CreateCallbackServer(Uri redirectUri)
        {
            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new StateMessageHandler(this));
            config.Routes.MapHttpRoute("", "", new {controller = "home"});

            var server = CreateHttpListenerServer(new List<Uri>() {redirectUri},
                WebApiAdapter.CreateWebApiAppFunc(config));

            return server;
        }

        public void SetAuthCode(string authCode)
        {
            _AuthCode = authCode;
            _Lock.Set();
        }

        public void Dispose()
        {
            if (_server != null) _server.Dispose();
        }


        public static IDisposable CreateHttpListenerServer(List<Uri> baseAddresses, Func<IDictionary<string, object>, Task> appFunc)
        {

            var props = new Dictionary<string, object>();

            var addresses = baseAddresses.Select(baseAddress => new Dictionary<string, object>()
            {
                {"host", baseAddress.Host}, 
                {"port", baseAddress.Port.ToString()}, 
                {"scheme", baseAddress.Scheme}, 
                {"path", baseAddress.AbsolutePath}

            }).Cast<IDictionary<string, object>>().ToList();

            props["host.Addresses"] = addresses;
            //props["server.LoggerFactory"] = LoggerFunc goes here; 
            OwinServerFactory.Initialize(props);
            return OwinServerFactory.Create(appFunc, props);
        }
    }

    public class StateMessageHandler : DelegatingHandler
    {
        private readonly OAuthDesktopFlow _flow;

        public StateMessageHandler(OAuthDesktopFlow flow)
        {
            _flow = flow;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Properties["Runscope_OAuthWorkflow"] = _flow;
            return base.SendAsync(request, cancellationToken);
        }


    }


    public static class WebApiAdapter
    {
        public static Func<IDictionary<string, object>, Task> CreateWebApiAppFunc(HttpConfiguration config)
        {
            var app = new HttpServer(config);
            var options = new HttpMessageHandlerOptions()
            {
                MessageHandler = app,
                BufferPolicySelector = new OwinBufferPolicySelector(),
                ExceptionLogger = new WebApiExceptionLogger(),
                ExceptionHandler = new WebApiExceptionHandler()
            };
            var handler = new HttpMessageHandlerAdapter(null, options);
            return (env) => handler.Invoke(new OwinContext(env));
        }
    }

    public class WebApiExceptionLogger : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }

    public class WebApiExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
    public class HomeController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            // extract auth grant code from request URI
            //request.RequestUri
            var flow = (OAuthDesktopFlow)request.Properties["Runscope_OAuthWorkflow"];
            var parameters = request.RequestUri.ParseQueryString();

            var content = new StringContent("<html>Authentication completed, you can close this now.</html>");
            content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            flow.SetAuthCode(parameters["code"]);
            
            return new HttpResponseMessage() {Content = content};
        }
    }
}
