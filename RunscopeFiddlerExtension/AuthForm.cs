using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Runscope;

namespace RunscopeFiddlerExtension
{
    public partial class AuthForm : Form
    {
        public string RedirectUri;
        public string AuthorizationCode { get; set; }

        public AuthForm()
        {
            InitializeComponent();
            var oAuthLink = new OAuth2AuthorizeLink();
            webBrowser1.Navigate(oAuthLink.Target);
            webBrowser1.Navigated += webBrowser1_Navigating;
        }


        void webBrowser1_Navigating(object sender, WebBrowserNavigatedEventArgs e)
        {
            
            if (e.Url.OriginalString.StartsWith(RedirectUri))
            {
                
                var path = e.Url.Query;
                var code = path.Substring(path.IndexOf("code=") + 5);
                AuthorizationCode = code;
                this.Close();
            }
        }
    }
}
