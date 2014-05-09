using System;

namespace Runscope
{
    public class OAuth2AuthorizeLink
    {
        public Uri Target { get; set; }

        public OAuth2AuthorizeLink()
        {
            // replace PLACEHOLDER
            Target = new Uri("https://www.runscope.com/signin/oauth/authorize?response_type=code&client_id=1acc6a2e-dd01-49ad-a856-e24789c77529&redirect_uri=http://localhost:9696&scope=api:read%20message:write&state=PLACEHOLDER");
        }
    }
}
