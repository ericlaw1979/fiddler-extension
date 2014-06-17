using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        /* Temporarily disable tests since we're not doing the OWin stuff anymore
        [Fact] public void TestOAuthDesktopFlow()
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
         */
        //[Fact]
        //public void GetToken()
        //{
        //    var share = new ShareRequest();
        //    var token = share.GetAuthToken();
        //    Assert.NotNull(token);
        //}

        //[Fact]
        //public void CreateMessageLinkRequest()
        //{
        //    var link = new MessagesLink();

        //    var request = link.BuildPOSTRequest("foo",null)
        //    ;
        //    Assert.NotNull(link);
        //}

        [Fact]
        public Task GetBucketsList()
        {
            var client = new HttpClient {BaseAddress = new Uri("https://api.runscope.com")};
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "d1aca20b-d15d-4dc7-8215-83c99664a79d");
            

            var bucketsLink = new BucketsLink();
            var buckets = new List<Bucket>();
            return client.SendAsync(bucketsLink.BuildGetRequest())
                 .ContinueWith(t =>
                 {
                     bucketsLink.ParseResponse(t.Result)
                         .ContinueWith(t2 =>
                         {
                             buckets = bucketsLink.ParseBucketList(t2.Result);
                             Assert.NotNull(buckets);
                         });

                 });

            
            
        }
    }
}
