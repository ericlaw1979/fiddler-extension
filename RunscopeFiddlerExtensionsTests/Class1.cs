using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fiddler;
using Microsoft.Win32;
using Runscope;
using RunscopeFiddlerExtension;
using Xunit;

namespace RunscopeFiddlerExtensionsTests
{
    public class MessageLinkTests
    {
       

        [Fact]
        public void TestOAuth()
        {
            var form = new AuthForm();
            form.ShowDialog();
            Assert.NotNull(form.AuthorizationCode);
        }

        [Fact]
        public void TestOAuthDesktopFlow()
        {
            using (var flow = new OAuthDesktopFlow())
            {
                var oauthLink = new OAuth2AuthorizeLink();

                var token = flow.GetAuthCode(oauthLink.Target, new Uri("http://localhost:9696/"));
                Thread.Sleep(5000);
                Assert.NotNull(token);
            }
        }


        [Fact]
        public void CreateCallBackServer()
        {
            using (var flow = new OAuthDesktopFlow())
            {
                var server = flow.CreateCallbackServer(new Uri("http://localhost:9696/"));
              
                Thread.Sleep(60000);
                Assert.NotNull(server);
                server.Dispose();
            }
        }
        [Fact]
        public void GetToken()
        {
            var share = new ShareRequest();
            var token = share.GetAuthToken();
            Assert.NotNull(token);
        }

        //[Fact]
        //public void CreateMessageLinkRequest()
        //{
        //    var link = new MessagesLink();

        //    var request = link.BuildPOSTRequest("foo",null)
        //    ;
        //    Assert.NotNull(link);
        //}
    }
}
