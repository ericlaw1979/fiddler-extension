using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Runscope;
using Xunit;

namespace RunscopeWebPackTests
{
    public class MessageLinkTests
    {
        [Fact]
        public void CreatePOSTRequest()
        {
            var link = new MessagesLink();
            var request = link.BuildPOSTRequest("foo",
                new HttpRequestMessage() {RequestUri = new Uri("http://example.org")},
                new HttpResponseMessage());
            var result = request.Content.ReadAsStringAsync().Result;
        }
    }
}
