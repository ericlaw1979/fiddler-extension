using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Runscope
{
    public class BucketsLink
    {
        public Uri Target { get; set; }

        public BucketsLink()
        {
            Target = new Uri("/buckets", UriKind.Relative);
        }
        public HttpRequestMessage BuildGetRequest()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = Target
            };
            return requestMessage;
        }


        public  async Task<JObject> ParseResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return null;
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        public  List<Bucket> ParseBucketList(JObject jBuckets)
        {
            if (jBuckets == null) return new List<Bucket>();
            return ((JArray) jBuckets["data"]).Cast<JObject>()
                .Select(jb => new Bucket(){Name = (string)jb.Property("name"),
                                           Key = (string)jb.Property("key")
                }).ToList();
        }
    }

    public class Bucket
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
